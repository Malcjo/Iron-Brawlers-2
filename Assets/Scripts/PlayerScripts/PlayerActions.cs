using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject[] playerGeometry, playerHeadGeometry, playerArmourTorsoGeometry, playerArmourHeadGeometry, playerLegGeometry, playerArmourLegGeometry;
    [SerializeField] private Material normalSkinMaterial, normalHeadMaterial, normalSkinBlocking, normalHeadBlocking, armourMaterial, headArmourMaterial, armourBlocking, legMaterial, legBlocking, armourDamage;
    public Animator anim;
    [SerializeField] Player self;
    private Hitbox hitboxScript;
    [SerializeField] GameObject hitBox;
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
    const string HEAVYKEY = "HEAVY";
    const string LANDKEY = "LANDING";
    const string JABKEY = "JAB";
    const string AERIALKEY = "AERIAL";
    const string DOUBLEJUMPKEY = "DOUBLE_JUMP";
    const string FALLINGKEY = "FALLING";
    const string JUMPINGKEY = "JUMP";
    const string ARMOURBREAKKEY = "ARMOUR_BREAK";
    const string BLOCKKEY = "BLOCK_IDLE";
    const string NORMALHITSTUNKEY = "HITSTUN_NORMAL_HIT";
    const string KNOCKDOWNKEY = "KNOCKDOWN_NORMAL";
    const string GETTINGUPKEY = "GETTING_UP_NORMAL";

    /*
 * Sol
 * jab- Position: left hand |Scale: 0.4  |gague damage: 2.5f |Strength: 25, 10 |Cancel time: 0.5 |MaxMoveTimeValue: 0.25 |Move strength: 0.5 |WhenToMoveCharacter: 0.2
 * heavy- Position: right hand |Scale: 0.6  |gague damage: 4 |Strength: 40, 2 |Cancel time: 0.8 |MaxMoveTimeValue: 3 |Move strength: 6  |WhenToMoveCharacter: 0.2
 * sweep- Position: right hand |Scale: 0.5 | gague damage: 3 |Strength: 20, 30 |Cancel time: 0.6 |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
 * aerial- Position: right elbow |Scale: 0.5 |gague damage: 3 |Strength: 35, -0.5 |Cancel time: 0.7 |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
 * armourbreak- Position: Center |Scale: 2 |gague damage: 5 |Strength: 50, 2 |Cancel time: |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
 * dash- Move Character On X Strength: 12 |MoveCharacterMaxCounter: 0.1 MaxDashTime: 0.5 
 * 
 * Goblin
 * jab- gague damage :Scale :Position: left hand |Strength:
 * heavy- gague damage :Scale :Position: head |Strength:
 * sweep- gague damage :Scale :Position: left hand |Strength:
 * aerial- gague damage :Scale :Position:head |Strength:
 * armourbreak- gague damage :Scale :Position: |Strength:
 * 
 * 
 * 
 */



    public JabVariables jabVariables;
    public HeavyVariables heavyVariables;
    public SweepVariables sweepVariables;
    public NeutralAerialVariables neutralAerialVariables;
    public ArmourBreakVariables armourBreakVariables;
    public DashVariables dashVariables;



    [System.Serializable]
    public struct JabVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter;

    }
    [System.Serializable]
    public struct HeavyVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize, DamageOnGague; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter;
                public float WhenToPlaySmearParticles;
    }
    [System.Serializable]
    public struct SweepVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct NeutralAerialVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct ArmourBreakVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct DashVariables
    {
        public float MoveCharacterOnXStrength; public float MoveCharacterMaxCounter; public float MaxDashTime;
    }
    [SerializeField]private float dashCounter;
    [SerializeField] private bool canDash;


    private void Awake()
    {
        hitboxScript = hitBox.GetComponent<Hitbox>();
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
        dashCounter += 1 * Time.deltaTime;
        if(dashCounter >= dashVariables.MaxDashTime)
        {
            canDash = true;
        }
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0)
            {
                comboStep = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (hitboxScript._attackType)
        {
            case AttackType.Jab:
                hitboxScript._followDes = jabVariables.FollowingDestination;
                hitboxScript.gaugeDamageValue = jabVariables.DamageOnGauge;
                hitBox.transform.localScale = Vector3.one * jabVariables.HitBoxSize;
                hitboxScript._knockbackStrength = new Vector3(self.GetFacingDirection() * jabVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
                break;
            case AttackType.HeavyJab:
                hitboxScript._followDes = heavyVariables.FollowingDestination;
                hitboxScript.gaugeDamageValue = heavyVariables.DamageOnGauge;
                hitBox.transform.localScale = Vector3.one * heavyVariables.HitBoxSize;
                hitboxScript._knockbackStrength = new Vector3(self.GetFacingDirection() * heavyVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
                break;
            case AttackType.LegSweep:
                hitboxScript._followDes = sweepVariables.FollowingDestination;
                hitboxScript.gaugeDamageValue = sweepVariables.DamageOnGauge;
                hitBox.transform.localScale = Vector3.one * sweepVariables.HitBoxSize;
                hitboxScript._knockbackStrength = new Vector3(self.GetFacingDirection() * sweepVariables.KnockbackXStrength, sweepVariables.KnockbackYStrength, 0);
                break;
            case AttackType.Aerial:
                hitboxScript._followDes = neutralAerialVariables.FollowingDestination;
                hitboxScript.gaugeDamageValue = neutralAerialVariables.DamageOnGauge;
                hitBox.transform.localScale = Vector3.one * neutralAerialVariables.HitBoxSize;
                hitboxScript._knockbackStrength = new Vector3(self.GetFacingDirection() * neutralAerialVariables.KnockbackXStrength, neutralAerialVariables.KnockbackYStrength, 0);
                break;
            case AttackType.ArmourBreak:
                hitboxScript._followDes = armourBreakVariables.FollowingDestination;
                hitboxScript.gaugeDamageValue = armourBreakVariables.DamageOnGauge;
                hitBox.transform.localScale = Vector3.one * armourBreakVariables.HitBoxSize;
                hitboxScript._knockbackStrength = new Vector3(self.GetFacingDirection() * armourBreakVariables.KnockbackXStrength, armourBreakVariables.KnockbackYStrength, 0);
                break;
        }
    }
    public void Dash()
    {
        if (canDash)
        {
            self.SetState(new BusyState());
            dashCounter = 0;
            canDash = false;
            StartCoroutine(DashAction());
        }
    }
    IEnumerator DashAction()
    {
        while (dashCounter <= dashVariables.MoveCharacterMaxCounter)
        {
            self.MoveCharacterMaxValue = dashVariables.MoveCharacterMaxCounter;
            self.SetMoveCharacterStrength(dashVariables.MoveCharacterOnXStrength);
            yield return null;
        }
        self.SetState(new IdleState());
    }

    public void Jab() 
    {
        StartCoroutine(JabAction()); 
    }

    private IEnumerator JabAction()
    {
        self.CanActOutOf = false;
        //self.MoveCharacterMaxValue = jabMoveStrength;
        self.MoveCharacterMaxValue = jabVariables.MoveCharacterMaxCounter;
        bool canMove = true;
        //anim.Play(animlist[comboStep]);
        TransitionToAnimation(JABKEY, 0.03f);
        anim.speed = 1;
        FindObjectOfType<AudioManager>().Play(AudioManager.JABMISS);
        //comboStep++;
        //comboTimer = 1;
        yield return null;
        //hitboxManager.SwapHands(0);
        //hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.Jab;
        hitboxManager.JabAttack(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < jabVariables.CancelTime)
            {
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < jabVariables.WhenToMoveCharacterInAnimation)
                {
                    yield return null;
                }
                if (canMove == true)
                {
                    //self.SetMoveCharacterStrength(hitboxScript.jabVariables.MoveCharacterStrength);
                   // self.SetMoveCharacterStrength(jabMoveStrength);
                }
                canMove = false;

                yield return null;
            }
            yield return null;
            self.CanActOutOf = true;
        }


        self.SetState(new IdleState());
    }

    public void Heavy() 
    {
        if (self.DebugModeOn == true)
        {
        }
        StartCoroutine(HeavyAction()); 
    }

    private IEnumerator HeavyAction()
    {
        if (self.CanDoAttack)
        {
            self.CanActOutOf = false;
            self.MoveCharacterMaxValue = heavyVariables.MoveCharacterMaxCounter;
            bool canMove = true;
            TransitionToAnimation(HEAVYKEY, 0.02f);
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
            anim.speed = 1;
            yield return null;
            //hitboxManager.SwapHands(1);
            //hitboxScript._attackDir = Attackdirection.Forward;
            hitboxScript._attackType = AttackType.HeavyJab;
            hitboxManager.JabAttack(0.5f);
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < heavyVariables.CancelTime)
                {
                    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.65f)
                    {
                        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < heavyVariables.WhenToMoveCharacterInAnimation)
                        {
                            yield return null;
                        }
                        yield return null;
                        if (useHitSmearOnHeavy)
                        {
                            ParticleSmearLines.Play();
                        }
                        if (canMove == true)
                        {
                            self.SetMoveCharacterStrength(heavyVariables.MoveCharacterOnXStrength);
                            //self.MoveCharacterWithAttacks(heavyAttackMoveValue);
                            canMove = false;
                        }
                    }
                    ParticleSmearLines.Stop();
                    yield return null;
                }
                self.CanActOutOf = true;
                ParticleSmearLines.Stop();
                yield return null;
            }
            self.SetState(new IdleState());
        }
        else
        {
            self.CanDoAttack = true;
            self.SetState(new IdleState());
        }

    }
    public void LegSweep()
    {
        StopCrouchBlock();
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        //ResetMaterial(); // might cause issues with the damage material resetting as well


        TransitionToAnimation(SWEEPKEY, 0.01f);
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
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < sweepVariables.CancelTime)
            {
                yield return null;
            }
            self.CanActOutOf = true;
            yield return null;
        }
        self.SetState(new CrouchingState());
    }
    public void AerialAttack()
    {
        if (self.DebugModeOn == true)
        {
        }
        StartCoroutine(_AerialAttack());
    }

    private IEnumerator _AerialAttack()
    {
        //self.MoveCharacterWithAttacks(200);

        TransitionToAnimation(AERIALKEY, 0.01f);
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
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < neutralAerialVariables.CancelTime)
            {
                yield return null;

            }
            self.CanActOutOf = true;

            //self.UseGravity = true;
            yield return null;
        }
        self.SetState(new JumpingState());
    }

    public void ForwardAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
        }
        self.SetState(new JumpingState());
    }
    public void BackAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
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

    private string LastState;
    [SerializeField]private bool useHitSmearOnHeavy;

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
        if(self.VerticalState != Player.VState.grounded)
        {
            self.SetState(new JumpingState());
        }
        else
        {
            anim.speed = 1;
            TransitionToAnimation(IDLEKEY, 0.02f);
        }

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

        StartCoroutine(_StopCrouchBlock());
    }
    private IEnumerator _StopCrouchBlock()
    {

        yield return new WaitForSeconds(0.1f);
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
        //SetArmourToNormalBlock();
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
            tempMaterial.material = headArmourMaterial;
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
        anim.speed = 2;
        TransitionToAnimation(NORMALHITSTUNKEY, 0.01f);
        //anim.Play("HITSTUN_NORMAL_HIT");

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
        anim.speed = 2.5f;
        TransitionToAnimation(KNOCKDOWNKEY, 0.01f);
        //anim.Play("HITSTUN_NORMAL_HIT");
        //anim.Play("KNOCKDOWN_NORMAL");
        //grab stuff off server and do boots

        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            anim.speed = 3f;
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            {
                yield return null;
            }
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
        anim.speed = 3;
        TransitionToAnimation(GETTINGUPKEY, 0.01f);
        //anim.Play("GETTING_UP_NORMAL");

        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        self.HitStun = false;
        RevertBackToIdleState();
    }
}
