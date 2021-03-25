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
    const string RUNKEY = "RUN";
    const string IDLEKEY = "IDLE";
    const string CROUCHKEY = "CROUCH_IDLE";
    const string SWEEPKEY = "SWEEP";
    const string LANDKEY = "LANDING";
    const string JABKEY = "JAB";
    const string DOUBLEJUMPKEY = "DOUBLE_JUMP";
    const string FALLINGKEY = "FALLING";
    const string JUMPINGKEY = "JUMP";
    const string ARMOURBREAKKEY = "ARMOUR_BREAK";
    const string BLOCKKEY = "BLOCK_IDLE";
    const string NORMALHITSTUNKEY = "HITSTUN_NORMAL_HIT";
    const string KNOCKDOWNKEY = "KNOCKDOWN_NORMAL";
    const string GETTINGUPKEY = "GETTING_UP_NORMAL";

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
        TransitionToAnimation(JABKEY, 0.01f);
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
        TransitionToAnimation(LANDKEY, 0.01f);
        anim.speed = 1;
        self.SetState(new BusyState());
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.landing = false;
        self.SetState(new IdleState());
    }


    private void TransitionToAnimation(string animation, float time)
    {
        if (LastState != animation)
        {
            anim.CrossFade(animation, time);
            LastState = animation;
        }
    }
    public void Running()
    {
        TransitionToAnimation(RUNKEY, 0.02f);
        anim.speed = self.GetAbsolutInputValueForMovingAnimationSpeed();
    }

    public void Idle()
    {
        anim.speed = 1;
        anim.Play("IDLE");

    }

    public void Crouching()
    {

        for(int i = 0; i < playerLegGeometry.Length; i++)
        {
            var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = legBlocking;
        }
        SetArmourToCrouchBlock();

        TransitionToAnimation(CROUCHKEY, 0.01f);
        //anim.Play("CROUCH_IDLE");
        anim.speed = 1;
    }
    public void ExitCrouch()
    {
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
        //StartCoroutine(_ExitCrouch());
    }
    private IEnumerator _ExitCrouch()
    {

        return null;
    }
    public void StopCrouchBlock()
    {
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
        //StartCoroutine(_StopCrouchBlock());
    }
    private IEnumerator _StopCrouchBlock()
    {

        return null;
    }
    public void LegSweep()
    {
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        //ResetMaterial(); // might cause issues with the damage material resetting as well
        StopCrouchBlock();
        TransitionToAnimation(SWEEPKEY, 0);
        //anim.Play("SWEEP");
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
        self.SetState(new CrouchingState());
    }

    public void Falling()
    {
        TransitionToAnimation(FALLINGKEY, 0.01f);
        anim.speed = 1;
    }

    public void Jumping()
    {
        TransitionToAnimation(JUMPINGKEY, 0.01f);
        anim.speed = 1;
    }
    public void DoubleJump()
    {
        StartCoroutine(_DoulbeJump());
    }

    IEnumerator _DoulbeJump()
    {
        TransitionToAnimation(DOUBLEJUMPKEY, 0.01f);
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
            TransitionToAnimation(ARMOURBREAKKEY, 0.01f);
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
    public void Block()
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
        SetArmourToNormalBlock();
        TransitionToAnimation(BLOCKKEY, 0.01f);
        //StartCoroutine(_EnterBlock());
    }

    IEnumerator _EnterBlock()
    {

        hitboxManager.Block();
        yield return null;
        while(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        StartCoroutine(BlockIdle());
    }
    IEnumerator BlockIdle()
    {

        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }
    public void ExitBlock()
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
        //StartCoroutine(_ExitBlock());
    }
    //IEnumerator _ExitBlock()
    //{

    //    //yield return null;
    //    //while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //    //{
    //    //    yield return null;
    //    //}

    //}

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
    public void SetArmourToNormalBlock()
    {
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
    }
    public void SetArmourToCrouchBlock()
    {
        for (int i = 0; i < playerArmourLegGeometry.Length; i++)
        {
            var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourBlocking;
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
        TransitionToAnimation(NORMALHITSTUNKEY, 0.01f);
        //anim.Play("HITSTUN_NORMAL_HIT");
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
        TransitionToAnimation(NORMALHITSTUNKEY, 0.01f);
        //anim.Play("HITSTUN_NORMAL_HIT");
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
        TransitionToAnimation(KNOCKDOWNKEY, 0.01f);
        //anim.Play("HITSTUN_NORMAL_HIT");
        //anim.Play("KNOCKDOWN_NORMAL");
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
        TransitionToAnimation(GETTINGUPKEY, 0.01f);
        //anim.Play("GETTING_UP_NORMAL");
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
