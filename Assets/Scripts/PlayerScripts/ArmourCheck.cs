using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public ArmourCondition LegArmourCondition;
    public ArmourCondition ChestArmourCondition;
    public ArmourCondition HeadArmourCondiditon;
    public enum ArmourPlacement { Head, Chest, Legs}
    public ArmourPlacement armourPlacement;
    public enum ArmourCondition { none, armour};

    public Vector3 knockBackResistance;
    public float armourWeight, armourReduceSpeed, reduceJumpForce;
    Vector3 chestKnockBackResistance, legsKnockBackResistance, headKnockBackResistance;
    float chestWeight, legsWeight, headWeight;
    float chestArmourReduceSpeed, legsArmourReduceSpeed, headArmourReduceSpeed;
    float chestReduceJump, legsReduceJump, headReduceJump;

    [SerializeField] private ArmourStats armourStats;

    public GameObject[] HeadArmourMesh;
    public GameObject[] ChestArmourMesh;
    public GameObject[] LegArmourMesh;

    private void Start()
    {
        SetAllArmourOn();
    }
    private void Update()
    {
        ArmourStatsCheck();
        ChangeArmourInputs();
        amountOfArmourLeft = AmountOfArmourLeft();
    }
    public bool HasArmour()
    {
        if (ChestArmourCondition == ArmourCondition.armour || LegArmourCondition == ArmourCondition.armour)
        {
            return true;
        }
        else if (ChestArmourCondition == ArmourCondition.none && LegArmourCondition == ArmourCondition.none)
        {
            return false;
        }
        return false;
    }
    [SerializeField] private int amountOfArmourLeft;
    public int AmountOfArmourLeft()
    {
        if(HeadArmourCondiditon == ArmourCondition.armour 
            && ChestArmourCondition == ArmourCondition.armour 
            && LegArmourCondition == ArmourCondition.armour)
        {
            return 3;
        }
        else if (HeadArmourCondiditon == ArmourCondition.armour
            && ChestArmourCondition == ArmourCondition.armour
            && LegArmourCondition == ArmourCondition.none)
        {
            return 2;
        }
        else if (HeadArmourCondiditon == ArmourCondition.armour
            && ChestArmourCondition == ArmourCondition.none
            && LegArmourCondition == ArmourCondition.none)
        {
            return 1;
        }
        else if (HeadArmourCondiditon == ArmourCondition.none
            && ChestArmourCondition == ArmourCondition.none
            && LegArmourCondition == ArmourCondition.none)
        {
            return 0;
        }
        return 0;
    }
    void ArmourStatsCheck()
    {
        switch (ChestArmourCondition)
        {
            case ArmourCondition.armour:
                chestWeight = armourStats.weightChest;
                chestArmourReduceSpeed = armourStats.reduceSpeedChest;
                chestReduceJump = armourStats.reduceJumpChest;
                chestKnockBackResistance = armourStats.knockBackResistanceChest;
                break;

            case ArmourCondition.none:
                chestWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = Vector3.zero;
                break;
        }
        switch (LegArmourCondition)
        {
            case ArmourCondition.armour:
                legsWeight = armourStats.weightLegs;
                legsArmourReduceSpeed = armourStats.reduceSpeedLegs;
                legsReduceJump = armourStats.reduceJumpLegs;
                legsKnockBackResistance = armourStats.knockBackRsistanceLegs;
                break;
            case ArmourCondition.none:
                legsWeight = 0;
                legsArmourReduceSpeed = 0;
                legsReduceJump = 0;
                legsKnockBackResistance = Vector3.zero;
                break;      
        }
        switch (HeadArmourCondiditon)
        {
            case ArmourCondition.armour:
                headWeight = armourStats.weightHead;
                headArmourReduceSpeed = armourStats.reduceSpeedHead;
                headReduceJump = armourStats.reduceJumpHead;
                headKnockBackResistance = armourStats.knockBackResistanceHead;
                break;
            case ArmourCondition.none:
                headWeight = 0;
                headArmourReduceSpeed = 0;
                headReduceJump = 0;
                headKnockBackResistance = Vector3.zero;
                break;
        }


        knockBackResistance = chestKnockBackResistance + legsKnockBackResistance;
        reduceJumpForce = chestReduceJump + legsReduceJump;
        armourReduceSpeed = chestArmourReduceSpeed + legsArmourReduceSpeed;
        armourWeight = chestWeight + legsWeight;
    }

    public void SetAllArmourOff()
    {
        SetArmourOff(ArmourPlacement.Legs);
        SetArmourOff(ArmourPlacement.Chest);
        SetArmourOff(ArmourPlacement.Head);
    }
    public void SetAllArmourOn()
    {
        SetArmourOn(ArmourPlacement.Legs, ArmourCondition.armour);
        SetArmourOn(ArmourPlacement.Chest, ArmourCondition.armour);
        SetArmourOn(ArmourPlacement.Head, ArmourCondition.armour);
    }
    public void SetArmourOn(ArmourPlacement placement, ArmourCondition type)
    {
        switch (placement)
        {
            case ArmourPlacement.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(true);
                    ChestArmourCondition = type;
                }
                break;
            case ArmourPlacement.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(true);
                    LegArmourCondition = type;
                }
                break;
            case ArmourPlacement.Head:
                for (int i = 0; i < HeadArmourMesh.Length; i++)
                {
                    HeadArmourMesh[i].SetActive(true);
                    HeadArmourCondiditon = type;
                }
                break;
        }
    }

    public void DestroyArmour(ArmourCheck.ArmourPlacement placement, Player defendingPlayer, AttackType attackType)
    {
        if (placement == ArmourCheck.ArmourPlacement.Head)
        {
            if (HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }
            RemoveHeadArmour();
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }

        else if (placement == ArmourCheck.ArmourPlacement.Chest)
        {
            if (ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }

            RemoveChestArmour();
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }

        else if (placement == ArmourCheck.ArmourPlacement.Legs)
        {
            if (LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }

            RemoveLegArmour();
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }
    }
    public void SetArmourOff(ArmourPlacement armourPlacement)
    {
        switch (armourPlacement)
        {
            case ArmourPlacement.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(false);
                    ChestArmourCondition = ArmourCondition.none;
                }
                break;
            case ArmourPlacement.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(false);
                    LegArmourCondition = ArmourCondition.none;
                }
                break;
            case ArmourPlacement.Head:
                for (int i = 0; i < HeadArmourMesh.Length; i++)
                {
                    HeadArmourMesh[i].SetActive(false);
                    HeadArmourCondiditon = ArmourCondition.none;
                }
                break;
            default:
                break;
        }
    }
    public ArmourCondition GetChestArmourCondiditon()
    {
        return ChestArmourCondition;
    }
    public ArmourCondition GetLegArmourCondition()
    {
        return LegArmourCondition;
    }
    public ArmourCondition GetHeadArmourCondition()
    {
        return HeadArmourCondiditon;
    }
    public void RemoveLegArmour()
    {
        SetArmourOff(ArmourPlacement.Legs);
    }

    public void RemoveChestArmour()
    {
        SetArmourOff(ArmourPlacement.Chest);
    }
    public void RemoveHeadArmour()
    {
        SetArmourOff(ArmourPlacement.Head);
    }

    void ChangeArmourInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAllArmourOff();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAllArmourOn();
        }
    }
}
