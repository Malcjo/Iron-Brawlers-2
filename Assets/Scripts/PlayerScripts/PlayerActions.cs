using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject[] playerGeometry, playerHeadGeometry, playerArmourTorsoGeometry, playerArmourHeadGeometry, playerLegGeometry, playerArmourLegGeometry;
    [SerializeField] private Material normalSkinMaterial, normalHeadMaterial, normalSkinBlocking, normalHeadBlocking, armourMaterial, headArmourMaterial, armourBlocking, legMaterial, legBlocking, armourDamage;
    public List<string> animlist = new List<string>();
    public Animator anim;
    [SerializeField] Player self;
    [SerializeField] Hitbox hitboxScript;
    [SerializeField] HitBoxManager hitboxManager;
    [SerializeField] ArmourCheck armourCheck;
    [SerializeField] ParticleSystem ParticleSmearLines;
    public bool IsDoubleJump = false;


    public int comboStep;
    public float comboTimer;
    private void Awake()
    {
        ParticleSmearLines.Stop();
    }
    private void SetParticleTrail(bool control)
    {
        if (control == true)
        {
            ParticleSmearLines.Play();
        }
        else if (control == false)
        {
            ParticleSmearLines.Stop();
        }
    }
    private void SetParticleEmmisionToZero(bool control)
    {
        if (control == true)
        {
            var emmision = ParticleSmearLines.main;
            emmision.startLifetime = 0.1f;
        }
        else
        {
            var emmision = ParticleSmearLines.main;
            emmision.startLifetime = 0;
        }
    }
    private void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0)
            {
                comboStep = 0;
            }
        }
    }

    public void JabCombo() 
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Jab");
        }
        StartCoroutine(Jab()); 
    }
    public float JabMoveValue = 200;
    private IEnumerator Jab()
    {
        bool canMove = true;
        //anim.Play(animlist[comboStep]);
        anim.Play("JAB");
        anim.speed = 1;
        FindObjectOfType<AudioManager>().Play(AudioManager.JABMISS);
        comboStep++;
        comboTimer = 1;
        yield return null;
        hitboxManager.SwapHands(0);
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.Jab;
        hitboxManager.JabAttack(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f)
                {
                    yield return null;
                }
                if (canMove == true)
                {
                    Debug.Log("Move Character");
                    self.MoveCharacterWithAttacks(JabMoveValue);
                }
                canMove = false;

                yield return null;
            }
            yield return null;
        }


        self.SetState(new IdleState());
    }

    public void Heavy() 
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Heavy");
        }
        StartCoroutine(_Heavy()); 
    }
    public float heavyAttackMoveValue = 600;
    private IEnumerator _Heavy()
    {
        bool canMove = true;
        anim.Play("HEAVY");
        FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
        anim.speed = 1;
        yield return null;
        hitboxManager.SwapHands(1);
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.HeavyJab;
        hitboxManager.JabAttack(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            {
                ParticleSmearLines.Stop();
                yield return null;
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.65f)
                {
                    ParticleSmearLines.Play();
                    yield return null;
                    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
                    {
                        yield return null;
                    }
                    if (canMove == true)
                    {
                        Debug.Log("Move Character");
                        self.MoveCharacterWithAttacks(heavyAttackMoveValue);
                        canMove = false;
                    }
                }
            }
            ParticleSmearLines.Stop();
            yield return null;
        }
        self.SetState(new IdleState());
    }
   
    public void AerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Aerial Attack");
        }
        StartCoroutine(_AerialAttack());
    }

    private IEnumerator _AerialAttack()
    {
        //self.MoveCharacterWithAttacks(200);
        anim.Play("AERIAL");
        FindObjectOfType<AudioManager>().Play(AudioManager.AERIALMISS);
        anim.speed = 1;
        self.CanTurn = false;
        //self.UseGravity = false;
        self.StopMovingCharacterOnYAxis();
        yield return null;
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
        hitboxManager.AeiralAttack();

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f)
            {
                yield return null;
            }
            //self.UseGravity = true;
            yield return null;
        }
        self.SetState(new JumpingState());
    }

    public void ForwardAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Forward Aerial Attack");
        }
        self.SetState(new JumpingState());
    }
    public void BackAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Back Aerial Attack");
        }
        self.SetState(new JumpingState());
    }
    //public void JumpLanding()
    //{
    //    StartCoroutine(_Jumplanding());
    //}
    //private IEnumerator _Jumplanding()
    //{
    //    anim.Play("LANDING");
    //    yield return null;
    //    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.80f)
    //    {
    //        yield return null;
    //    }
    //    self.SetState(new IdleState());
    //}
    public void Running()
    {
        anim.Play("RUN");
        anim.speed = self.GetAbsolutInputValueForMovingAnimationSpeed();
    }
    public void Landing()
    {
        StartCoroutine(_Landing());
    }
    IEnumerator _Landing()
    {
        self.landing = true;
        anim.Play("LANDING");
        anim.speed = 2;
        self.SetState(new BusyState());
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.landing = false;
        self.Landed = true;
        self.SetState(new IdleState());
    }

    public void Idle()
    {
        anim.Play("IDLE");
        anim.speed = 1;
    }

    public void Crouching()
    {
        for(int i = 0; i < playerLegGeometry.Length; i++)
        {
            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = legBlocking;
        }
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourBlocking;
        }
        anim.Play("CROUCH_IDLE");
        anim.speed = 1;
    }
    public void ExitCrouch()
    {
        StartCoroutine(_ExitCrouch());
    }
    private IEnumerator _ExitCrouch()
    {
        yield return null;
        for (int i = 0; i < playerLegGeometry.Length; i++)
        {
            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = legMaterial;
        }
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
        self.SetState(new IdleState());
    }
    public void StopCrouchBlock()
    {
        StartCoroutine(_StopCrouchBlock());
    }
    private IEnumerator _StopCrouchBlock()
    {
        yield return null;
        for (int i = 0; i < playerLegGeometry.Length; i++)
        {
            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = legMaterial;
        }
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
    }
    public void LegSweep()
    {
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        //ResetMaterial(); // might cause issues with the damage material resetting as well
        StopCrouchBlock();
        anim.Play("SWEEP");
        anim.speed = 1;
        self.CanTurn = false;
        yield return null;
        hitboxManager.SwapHands(1);
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
        hitboxManager.LegSweep(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }
    public void Falling()
    {
        anim.Play("FALLING");
        anim.speed = 1;
    }
    public void Jumping()
    {
        anim.Play("JUMP");
        anim.speed = 1;
    }
    public void DoubleJump()
    {
        StartCoroutine(_DoulbeJump());
    }
    IEnumerator _DoulbeJump()
    {
        anim.Play("DOUBLE_JUMP");
        anim.speed = 1;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        IsDoubleJump = false;
    }
    public void PauseCurrentAnimation()
    {
        anim.speed = 0;
    }
    public void ResumeCurrentAnimation()
    {
        anim.speed = 1;
    }
    public void ArmourBreak()
    {
        StartCoroutine(_ArmourBreak());
    }
    private IEnumerator _ArmourBreak()
    {
        if (armourCheck.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none && armourCheck.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none)
        {
            RevertBackToIdleState();
            yield return null;
        }
        else
        {
            StopCrouchBlock();
            anim.Play("ARMOUR_BREAK");
            anim.speed = 1;
            FindObjectOfType<AudioManager>().Play(AudioManager.ARMOURBREAK);
            self.CanTurn = false;
            yield return null;
            hitboxScript._attackDir = Attackdirection.Down;
            hitboxScript._attackType = AttackType.ArmourBreak;
            armourCheck.SetAllArmourOff();
            hitboxManager.ArmourBreak();
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            RevertBackToIdleState();
        }
    }
    private void RevertBackToIdleState()
    {
        if (self.VerticalState == Player.VState.grounded)
        {
            Debug.Log("End hit stun");
            self.SetState(new IdleState());
        }
        else
        {
            self.SetState(new JumpingState());
        }
    }
    public void EnterBlock()
    {
        StartCoroutine(_EnterBlock());
    }
    IEnumerator _EnterBlock()
    {
        for (int i = 0; i < playerHeadGeometry.Length; i++)
        {
            var tempMaterial = playerHeadGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalHeadBlocking;
        }
        for (int i = 0; i < playerGeometry.Length; i++)
        {
            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalSkinBlocking;
        }
        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
        {
            var tempMaterial = playerArmourHeadGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourBlocking;
        }
        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
        {
            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourBlocking;
        }
        hitboxManager.Block();
        anim.Play("BLOCK");
        yield return null;
        while(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        StartCoroutine(BlockIdle());
    }
    IEnumerator BlockIdle()
    {
        anim.Play("BLOCK_IDLE");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }
    public void ExitBlock()
    {
        StartCoroutine(_ExitBlock());
    }
    IEnumerator _ExitBlock()
    {
        for (int i = 0; i < playerHeadGeometry.Length; i++)
        {
            var tempMaterial = playerHeadGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalHeadMaterial;
        }
        for (int i = 0; i < playerGeometry.Length; i++)
        {
            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalSkinMaterial;
        }
        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
        {
            var tempMaterial = playerArmourHeadGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = headArmourMaterial;
        }
        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
        {
            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
        hitboxManager.StopBlock();
        anim.Play("BLOCK_EXIT");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }

    public void ArmourDamage()
    {
        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
        {
            var tempMaterial = playerArmourHeadGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourDamage;
        }
        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
        {
            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourDamage;
        }
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourDamage;
        }
    }
    public void ResetArmourDamage()
    {
        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
        {
            var tempMaterial = playerArmourHeadGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
        {
            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
    }
    //public void ResetMaterial()
    //{
    //    if (self.Blocking)
    //    {
    //        for (int i = 0; i < playerHeadGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerHeadGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = normalHeadBlocking;
    //        }
    //        for (int i = 0; i < playerGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = normalSkinBlocking;
    //        }
    //        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerArmourHeadGeometry[i].GetComponent<MeshRenderer>();
    //            tempMaterial.material = armourBlocking;
    //        }
    //        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
    //            tempMaterial.material = armourBlocking;
    //        }
    //        for (int i = 0; i < playerLegGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = legBlocking;
    //        }
    //        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
    //            tempMaterial.material = armourBlocking;
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < playerHeadGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerHeadGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = normalHeadMaterial;
    //        }
    //        for (int i = 0; i < playerArmourHeadGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerHeadGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = armourMaterial;
    //        }
    //        for (int i = 0; i < playerGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = normalSkinMaterial;
    //        }
    //        for (int i = 0; i < playerLegGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
    //            tempMaterial.material = normalSkinMaterial;
    //        }
    //        for (int i = 0; i < playerArmourTorsoGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerArmourTorsoGeometry[i].GetComponent<MeshRenderer>();
    //            tempMaterial.material = armourMaterial;
    //        }
    //        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
    //        {
    //            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
    //            tempMaterial.material = armourMaterial;
    //        }
    //    }

    //}

    public void HitKnockBack()
    {
        StartCoroutine(KnockBack());
    }
    IEnumerator KnockBack()
    {
        self.HitStun = true;
        self.SetState(new BusyState());
        yield return null;
        anim.Play("HITSTUN_NORMAL_HIT");
        anim.speed = 2;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.HitStun = false;
        self.SetState(new IdleState());
    }
    public void JabKnockBack()
    {
        StartCoroutine(_JabKnockBack());
    }
    IEnumerator _JabKnockBack()
    {
        self.HitStun = true;
        self.CanTurn = false;
        self.SetState(new BusyState());
        anim.Play("HITSTUN_NORMAL_HIT");
        anim.speed = 2;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
            if (self.VerticalState == Player.VState.grounded)
            {
                self.HitStun = false;
            }
        }
        self.HitStun = false;
        self.SetState(new IdleState());
    }
    public void knockDown()
    {
        StartCoroutine(_KnockDown());
    }
    private IEnumerator _KnockDown()
    {
        self.HitStun = true;
        self.CanTurn = false;
        self.SetState(new BusyState());
        //anim.Play("HITSTUN_NORMAL_HIT");
        anim.Play("KNOCKDOWN_NORMAL");
        anim.speed = 2.5f;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        _GetBackUp();
        StopCoroutine(_KnockDown());
    }
    private void _GetBackUp()
    {
        self.SetState(new BusyState());
        StartCoroutine(GetBackUp());
    }
    private IEnumerator GetBackUp()
    {
        self.HitStun = true;
        self.CanTurn = false;
        anim.Play("GETTING_UP_NORMAL");
        anim.speed = 2;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        self.HitStun = false;
        RevertBackToIdleState();
    }
}
