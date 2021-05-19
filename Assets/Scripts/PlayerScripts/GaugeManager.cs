using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GaugeManager : MonoBehaviour
{
    public Slider gauge;
    [SerializeField] ArmourCheck armourCheck;

    private WaitForSeconds repairTick = new WaitForSeconds(0.1f);
    private Coroutine repair;
    [SerializeField] PlayerActions actions;

    [SerializeField] private float currentGauge;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float noHealth = 0;
    [SerializeField] private float armourBreakingPoint;

    [SerializeField] Slider playerUI;
    [SerializeField] Player self;
    [SerializeField] private float WaitTime;

    private bool StopRepair = false;

    //[SerializeField] private float lowDamamgeValue = 2;
    //[SerializeField] private float midDamamgeValue = 4;
    //[SerializeField] private float highDamamgeValue = 7;
    //[SerializeField] private float breakingPoint = 9f;


    //private float DamageTick;
    //private float lowDamamgeTick = 10;
    //private float midDamamgeTick = 20;
    //private float highDamamgeTick = 30;
    //private float breakingPointTick = 50;


    //private float lowDamamgeWait = 1.5f;
    //private float midDamamgeWait = 2;
    //private float highDamamgeWait = 3.5f;
    //private float breakingPointWait = 4;

    private void Start()
    {
        
        if(self.playerNumber == Player.PlayerIndex.Player1)
        {
            playerUI = self.PlayerInputHandler.player1Slider;
            //playerUI = GameManager.instance.GetPlayer1UI();
        }
        else if(self.playerNumber == Player.PlayerIndex.Player2)
        {
            playerUI = self.PlayerInputHandler.player2Slider;
            //playerUI = GameManager.instance.GetPlayer2UI();
        }

        currentGauge = maxHealth;
        gauge.maxValue = maxHealth;
        gauge.minValue = noHealth;
        gauge.value = maxHealth;
        //if (currentGauge >= maxDamage)
        //{
        //    if (repair != null)
        //    {
        //        StopCoroutine(repair);
        //    }
        //    repair = StartCoroutine(RepairGauge());
        //}
    }
    private float repairCount;
    private void Update()
    {
        if(self.Blocking || self.CrouchBlocking)
        {
            repairCount = 0;
        }
        else if (!self.Blocking || !self.CrouchBlocking)
        {
            repairCount = 1;
        }
        StopGaugeFromGettingMoreThanMinGauge();
        playerUI.value = currentGauge;
        GaugeVariableChecker();
        ManualRepairGauge();
        delayRepiarStart();
    }
    private void StopGaugeFromGettingMoreThanMinGauge()
    {
        if (currentGauge > maxHealth) { currentGauge = maxHealth; }
    }
    public float GetArmourGaugeValue()
    {
        return currentGauge;
    }
    private void GaugeVariableChecker()
    {
        if(currentGauge < armourBreakingPoint)
        {
            actions.ArmourDamage();
        }
        else if (currentGauge > armourBreakingPoint)
        {
            if (!self.Crouching && !self.Blocking)
            {
                actions.ResetArmourDamage();
            }
            else if (self.Crouching)
            {
                actions.Crouching();
                //actions.SetArmourToCrouchBlock();
            }
            else if (self.Blocking)
            {
                actions.SetArmourToNormalBlock();
            }

        }
    }

    public void TakeDamage(float amount, ArmourCheck.ArmourPlacement Placement, Player defendingPlayer, AttackType attackType, Vector3 hitPosition)
    {

        if(currentGauge - amount > noHealth)
        {
            currentGauge -= amount;
            gauge.value = currentGauge;
            StopRepair = true;
            repairDeleyCounter = 0;

            //if (repair != null)
            //{
            //    StopCoroutine(repair);
            //}
            //repair = StartCoroutine(RepairGauge());

        }
        else
        {
            self.PlayParticle(ParticleType.BreakArmourPiece, hitPosition);
            armourCheck.DestroyArmour(Placement, defendingPlayer, attackType);
            resetGauge();
        }
    }
    float repairDeleyCounter;
    private void delayRepiarStart()
    {
        if(!self.Blocking || !self.CrouchBlocking)
        {
            repairDeleyCounter += (repairCount * Time.deltaTime);
            if (repairDeleyCounter > 2)
            {
                repairDeleyCounter = 0;
                StopRepair = false;
            }
        }
        else if (self.Blocking || self.CrouchBlocking)
        {
            StopRepair = true;
            repairDeleyCounter = 0;
        }


    }
    //private void restartRepair()
    //{
    //    StopCoroutine(RepairGauge());
    //}
    //private IEnumerator RepairGauge()
    //{
    //    yield return new WaitForSeconds(4);
    //    while (currentGauge > minDamage)
    //    {
    //        if(StopRepair == true)
    //        {
    //            StopRepair = false;
    //            repair = null;
    //            restartRepair();
    //        }
    //        currentGauge +=  maxDamage / 15;
    //        gauge.value = currentGauge;

    //        yield return repairTick;
    //        if (currentGauge == minDamage)
    //        {
    //            SetGaugeToMax();
    //        }
    //    }
    //    repair = null;
    //}
    [SerializeField] float counterDelay;
    private void ManualRepairGauge()
    {
        if(!self.Blocking || !self.CrouchBlocking)
        {
            if (StopRepair == false)
            {
                counterDelay += (repairCount * Time.deltaTime);
                if (counterDelay >= 0.25)
                {
                    counterDelay = 0;
                    currentGauge += (maxHealth / 7);
                }
                gauge.value = currentGauge;
                if (currentGauge == noHealth)
                {
                    SetGaugeToMax();
                }
            }
        }
    }
    private void SetGaugeToMax()
    {
        currentGauge = maxHealth;
        gauge.value = currentGauge;
        actions.ResetArmourDamage();
    }
    public void resetGauge()
    {
        actions.ResetArmourDamage();
        currentGauge = maxHealth;
        gauge.value = currentGauge;
    }
}
