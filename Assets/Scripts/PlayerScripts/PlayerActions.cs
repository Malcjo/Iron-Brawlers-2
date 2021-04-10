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

    public bool IsDoubleJump = false;
    [SerializeField] private float dashCounter;
    [SerializeField] private bool canDash;
    [SerializeField] private bool dashParticle;

    [SerializeField] public GameObject torsoShield;
    [SerializeField] public GameObject legShield;



    [Header("AnimationTiming")]
    [SerializeField]private float JabCounter;
    private float MaxJabTime;

    [SerializeField] private float HeavyCounter;
    private float MaxHeavyTime;

    [SerializeField] private float SweepCounter;
    private float MaxSweepTime;

    [SerializeField] private float NeutralAerialCounter;
    private float MaxNeutralAerialTime;

    [SerializeField] private float BlockCounter;
    private float MaxBlocktime = 0.5f;



    [Header("Particles")]
    [SerializeField] ParticleSystem JabParticle;
    [SerializeField] ParticleSystem HeavyParticle;
    [SerializeField] ParticleSystem SweepParticle;
    [SerializeField] ParticleSystem AerialParticle;
    [SerializeField] ParticleSystem BackAirParticle;
    [SerializeField] ParticleSystem UpAirParticle;
    [SerializeField] ParticleSystem DashParticle;
    [SerializeField] ParticleSystem RollParticle;
    [SerializeField] ParticleSystem canDashParitcle;






    //public int comboStep;
    //public float comboTimer;
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
    const string DASHKEY = "DASH";

    [Header("Crossfade Timing")]
    [SerializeField] private float idleCrossfade;
    [SerializeField] private float runCrossfade;
    [SerializeField] private float jabCrossfade;
    [SerializeField] private float heavyCrossfade;
    [SerializeField] private float crouchCrossfade;
    [SerializeField] private float sweepCrossfade;
    [SerializeField] private float neutralAerialCrossfade;

    [SerializeField] private float dashCrossfade;
    [SerializeField] private float blockCrossfade;

    [SerializeField] private float jumpCrossfade;
    [SerializeField] private float doubleJumpCrossfade;
    [SerializeField] private float fallingCrossfade;
    [SerializeField] private float landingCrossfade;

    [SerializeField] private float getUpCrossfade;
    [SerializeField] private float normalHitCrossfade;
    [SerializeField] private float knockDownCrossfade;

    [Header("Attack Variables")]
    public JabVariables jabVariables;
    public HeavyVariables heavyVariables;
    public SweepVariables sweepVariables;
    public NeutralAerialVariables neutralAerialVariables;
    public BackAirVariables backAirVariables;
    public UpAirVariables upAirVariables;
    public ArmourBreakVariables armourBreakVariables;
    public DashVariables dashVariables;
    public RollVariables rollVariables;



    [System.Serializable]
    public struct JabVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
        [Range(0, 1)]
        public float HitBoxTurnOn;
        [Range(0, 1)]
        public float HitBoxTurnOff;

    }
    [System.Serializable]
    public struct HeavyVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
        [Range(0, 1)]
        public float HitBoxTurnOn;
        [Range(0, 1)]
        public float HitBoxTurnOff;

    }
    [System.Serializable]
    public struct SweepVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
        [Range(0, 1)]
        public float HitBoxTurnOn;
        [Range(0, 1)]
        public float HitBoxTurnOff;
    }
    [System.Serializable]
    public struct NeutralAerialVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
        [Range(0, 1)]
        public float HitBoxTurnOn;
        [Range(0, 1)]
        public float HitBoxTurnOff;

    }
    [System.Serializable]
    public struct BackAirVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
    }
    [System.Serializable]
    public struct UpAirVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
    }
    [System.Serializable]
    public struct ArmourBreakVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        [Range(0, 1)]
        public float CancelTime;
        [Range(0, 1)]
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
        [Range(0, 1)]
        public float HitBoxTurnOn;
        [Range(0, 1)]
        public float HitBoxTurnOff;
    }
    [System.Serializable]
    public struct DashVariables
    {
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public float MaxDashTime; public float DashAnimationLength;
    }
    [System.Serializable]
    public struct RollVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime;
        public float WhenToMoveCharacterInAnimation;
        public bool MoveOnX;
        public float MoveCharacterOnXStrength; public float MoveCharacterOnXMaxCounter;
        public bool MoveOnY;
        public float MoveCharacterOnYStrength; public float MoveCharacterOnYMaxCounter;
    }

    private void Start()
    {
        torsoShield.SetActive(false);
        legShield.SetActive(false);
        MaxJabTime = jabVariables.CancelTime;
        JabCounter = MaxJabTime;
        MaxHeavyTime = heavyVariables.CancelTime;
        HeavyCounter = MaxHeavyTime;
        MaxNeutralAerialTime = neutralAerialVariables.CancelTime;
        NeutralAerialCounter = MaxNeutralAerialTime;
        MaxSweepTime = sweepVariables.CancelTime;
        SweepCounter = MaxSweepTime;
        BlockCounter = MaxBlocktime;
    }

    private void Awake()
    {
        hitboxScript = hitBox.GetComponent<Hitbox>();
        HeavyParticle.Stop();
    }
    private void SetParticleTrail(bool control)
    {
        if (control == true)
        {
            HeavyParticle.Play();
        }
        else if (control == false)
        {
            HeavyParticle.Stop();
        }
    }
    private void SetParticleEmmisionToZero(bool control)
    {
        if (control == true)
        {
            var emmision = HeavyParticle.main;
            emmision.startLifetime = 0.1f;
        }
        else
        {
            var emmision = HeavyParticle.main;
            emmision.startLifetime = 0;
        }
    }
    private void Update()
    {
        animPercentageTracker = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        JabCounter += 1 * Time.deltaTime;
        HeavyCounter += 1 * Time.deltaTime;
        SweepCounter += 1 * Time.deltaTime;
        NeutralAerialCounter += 1 * Time.deltaTime;
        BlockCounter += 1 * Time.deltaTime;

        dashCounter += 1 * Time.deltaTime;
        if(dashCounter >= dashVariables.MaxDashTime)
        {
            canDash = true;
            if (dashParticle)
            {
                if(canDashParitcle != null)
                {
                    canDashParitcle.Play();
                }
                dashParticle = false;
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
            case AttackType.Heavy:
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
        StartCoroutine(DashAction());
    }
    IEnumerator DashAction()
    {
        if (canDash)
        {
            TransitionToAnimation(true, DASHKEY, dashCrossfade);
            dashParticle = true;
            if (DashParticle != null)
            {
                DashParticle.Play();
            }

            dashCounter = 0;
            canDash = false;
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                if (dashCounter <= dashVariables.MoveCharacterOnXMaxCounter)
                {
                    self.MoveCharacterOnXMaxValue = dashVariables.MoveCharacterOnXMaxCounter;
                    self.SetMoveCharacterOnXStrength(dashVariables.MoveCharacterOnXStrength);
                }
                if (dashCounter > dashVariables.MoveCharacterOnXMaxCounter)
                {
                    self.SetState(new IdleState());
                    break;
                }
                yield return null;
            }
            //TransitionToAnimation(true, IDLEKEY, idleCrossfade);
            self.SetState(new IdleState());
        }

        self.SetState(new IdleState());
    }

    public void Jab() 
    {
        StartCoroutine(JabAction());
        if (anim.GetCurrentAnimatorStateInfo(0).IsName())
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.HitBoxTurnOff)
            {
                hitboxManager.TurnOffHitBox();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.HitBoxTurnOn && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= heavyVariables.HitBoxTurnOff && !self.HaveHitPlayer)
            {
                hitboxManager.TurnOnHitBox();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.CancelTime)
            {
                self.CanActOutOf = true;
                self.HaveHitPlayer = false;
            }
            if (self.ActionTriggered)
            {
                hitboxManager.TurnOffHitBox();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(HEAVYKEY))
                {
                    self.SetState(new IdleState());
                }
            }
        }
    }

    private IEnumerator JabAction()
    {
        if(JabCounter >= MaxJabTime)
        {
            JabCounter = 0;
            self.CanActOutOf = false;
            TransitionToAnimation(true, JABKEY, jabCrossfade);
            self.inAction = true;
            anim.speed = 1;

            self.MoveCharacterOnXMaxValue = jabVariables.MoveCharacterOnXMaxCounter;


            yield return null;
            if (JabParticle != null)
            {
                JabParticle.Play();
            }
            FindObjectOfType<AudioManager>().Play(AudioManager.JABMISS);
            hitboxScript._attackType = AttackType.Jab;
            //hitboxManager.JabAttack(0.5f);
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(JABKEY))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= jabVariables.HitBoxTurnOff)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= jabVariables.HitBoxTurnOn && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= jabVariables.HitBoxTurnOff && !self.HaveHitPlayer)
                    {
                        hitboxManager.TurnOnHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= jabVariables.CancelTime)
                    {
                        self.CanActOutOf = true;
                        self.HaveHitPlayer = false;
                    }
                    if (self.ActionTriggered)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
                    {
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName(JABKEY))
                        {
                            self.SetState(new IdleState());
                        }
                    }
                }
                yield return null;
            }
            self.CanActOutOf = true;
        }
        //TransitionToAnimation(false, IDLEKEY, idleCrossfade);
        self.SetState(new IdleState());
    }

    public void Heavy() 
    {
        if (self.DebugModeOn == true)
        {
        }
        StartCoroutine(HeavyAction()); 
    }
    [SerializeField] float animPercentageTracker;
    private IEnumerator HeavyAction()
    {
        if(HeavyCounter >= MaxHeavyTime)
        {

            self.CanDoAttack = false;
            if (HeavyParticle != null)
            {
                HeavyParticle.Play();
            }
            HeavyCounter = 0;
            self.CanActOutOf = false;
            self.MoveCharacterOnXMaxValue = heavyVariables.MoveCharacterOnXMaxCounter;
            self.MoveCharacterOnYMaxValue = heavyVariables.MoveCharacterOnYMaxCounter;
            bool canMove = true;

            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
            hitboxScript._attackType = AttackType.Heavy;
            TransitionToAnimation(true, HEAVYKEY, heavyCrossfade);
            anim.speed = 1;
            yield return null;
            //hitboxManager.JabAttack(0.5f);


            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(HEAVYKEY))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.WhenToMoveCharacterInAnimation)
                    {
                        if (canMove == true)
                        {
                            if (heavyVariables.MoveOnY)
                            {
                                self.SetMoveCharacterOnYStrength(heavyVariables.MoveCharacterOnYStrength);
                            }
                            if (heavyVariables.MoveOnX)
                            {
                                self.SetMoveCharacterOnXStrength(heavyVariables.MoveCharacterOnXStrength);
                            }
                            canMove = false;
                        }
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.HitBoxTurnOff)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.HitBoxTurnOn && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= heavyVariables.HitBoxTurnOff && !self.HaveHitPlayer)
                    {
                        hitboxManager.TurnOnHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= heavyVariables.CancelTime)
                    {
                        self.CanActOutOf = true;
                        self.HaveHitPlayer = false;
                    }

                    if (self.ActionTriggered)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
                    {
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName(HEAVYKEY))
                        {
                            self.SetState(new IdleState());
                        }
                    }
                }
                yield return null;
            }
            self.CanActOutOf = true;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName(HEAVYKEY))
            {
                Debug.Log("turn off hitbox from animation finishing");
                hitboxManager.TurnOffHitBox();
            }
        }
        self.SetState(new IdleState());

    }
    public void LegSweep()
    {

        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        if(SweepCounter >= MaxSweepTime)
        {
            if (SweepParticle != null)
            {
                SweepParticle.Play();
            }
            StopCrouchBlock();
            SweepCounter = 0;
            self.CanActOutOf = false;
            TransitionToAnimation(true, SWEEPKEY, sweepCrossfade);
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
            anim.speed = 1;
            self.CanTurn = false;
            yield return null;
            hitboxScript._attackType = AttackType.LegSweep;
            //hitboxManager.LegSweep(0.5f);
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(SWEEPKEY))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= sweepVariables.HitBoxTurnOff)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= sweepVariables.HitBoxTurnOn && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= sweepVariables.HitBoxTurnOff && !self.HaveHitPlayer)
                    {
                        hitboxManager.TurnOnHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= sweepVariables.CancelTime)
                    {
                        self.CanActOutOf = true;
                        self.HaveHitPlayer = false;
                    }
                    if (self.ActionTriggered)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
                    {
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName(SWEEPKEY))
                        {
                            self.SetState(new IdleState());
                        }
                    }
                }
                yield return null;
            }
            self.CanActOutOf = true;
        }

        self.SetState(new IdleState());
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
        if (NeutralAerialCounter >= MaxNeutralAerialTime)
        {
            if (AerialParticle != null)
            {
                AerialParticle.Play();
            }
            NeutralAerialCounter = 0;
            self.CanActOutOf = false;
            self.MoveCharacterOnXMaxValue = neutralAerialVariables.MoveCharacterOnXMaxCounter;
            self.MoveCharacterOnYMaxValue = neutralAerialVariables.MoveCharacterOnYMaxCounter;
            TransitionToAnimation(true, AERIALKEY, neutralAerialCrossfade);
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
            anim.speed = 1;
            self.CanTurn = false;
            self.StopMovingCharacterOnYAxis();
            yield return null;
            hitboxScript._attackType = AttackType.Aerial;
            //hitboxScript._attackDir = Attackdirection.Aerial;
            //hitboxManager.AeiralAttack();
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(AERIALKEY))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= neutralAerialVariables.HitBoxTurnOff)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= neutralAerialVariables.WhenToMoveCharacterInAnimation)
                    {
                        if (neutralAerialVariables.MoveOnY)
                        {
                            self.SetMoveCharacterOnYStrength(neutralAerialVariables.MoveCharacterOnYStrength);
                        }
                        if (neutralAerialVariables.MoveOnX)
                        {
                            self.SetMoveCharacterOnXStrength(neutralAerialVariables.MoveCharacterOnXStrength);
                        }
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= neutralAerialVariables.HitBoxTurnOn && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= neutralAerialVariables.HitBoxTurnOff && !self.HaveHitPlayer)
                    {
                        hitboxManager.TurnOnHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= neutralAerialVariables.CancelTime)
                    {
                        self.CanActOutOf = true;
                        self.HaveHitPlayer = false;
                    }
                    if (self.ActionTriggered)
                    {
                        hitboxManager.TurnOffHitBox();
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
                    {
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName(AERIALKEY))
                        {
                            self.SetState(new IdleState());
                        }
                    }
                }
                yield return null;
            }

            self.CanActOutOf = true;
        }

        self.moveCharacterOnXCounter = 5;
        self.moveCharacterOnYCounter = 5;
        self.SetMoveStrengthXTo0();
        self.SetMoveStrengthYTo0();
        if (self.VerticalState == Player.VState.grounded)
        {

            self.SetState(new IdleState());
        }
        else
        {
            self.SetState(new JumpingState());
        }

    }
    public void ArmourBreak()
    {
        StartCoroutine(_ArmourBreak());
    }

    private IEnumerator _ArmourBreak()
    {
        if (armourCheck.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none && armourCheck.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none && armourCheck.GetHeadArmourCondition() == ArmourCheck.ArmourCondition.none)
        {
            RevertBackToIdleState();
            yield return null;
        }
        else
        {
            self.SetOverrideToZero();
            StopCrouchBlock();
            self.MoveCharacterOnYMaxValue = armourBreakVariables.MoveCharacterOnYMaxCounter;
            self.SetMoveCharacterOnYStrength(armourBreakVariables.MoveCharacterOnYStrength);
            TransitionToAnimation(true, ARMOURBREAKKEY, 0.01f);
            anim.speed = 1;
            FindObjectOfType<AudioManager>().Play(AudioManager.ARMOURBREAK);
            self.CanTurn = false;
            yield return null;
            hitboxScript._attackDir = Attackdirection.Down;
            hitboxScript._attackType = AttackType.ArmourBreak;
            armourCheck.SetAllArmourOff();
            hitboxManager.ArmourBreak();

            self.MinusOneToJumpIndex();
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;

            }
            RevertBackToIdleState();
        }
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

    public void Landing()
    {
        StartCoroutine(_Landing());
    }

    IEnumerator _Landing()
    {
        self.landing = true;
        TransitionToAnimation(false, LANDKEY, landingCrossfade);
        anim.speed = 1;
        self.SetState(new BusyState());
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.landing = false;
        self.CanActOutOf = true;
        self.SetState(new IdleState());
    }

    [SerializeField] private string LastState;
    [SerializeField] private string LastAnimation;
    [SerializeField]private bool useHitSmearOnHeavy;
    private void TransitionToAnimation(bool canOverride, string animation, float time)
    {

        if (!canOverride)
        {
            if (LastState != animation)
            {
                anim.CrossFade(animation, time);
                LastState = animation;
            }
        }
        else
        {
            anim.CrossFade(animation, time, -1, 0);
        }
        LastAnimation = animation;
    }
    public void Running()
    {
        TransitionToAnimation(false, RUNKEY,runCrossfade);
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
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName(IDLEKEY))
            {
                anim.speed = 1;
                TransitionToAnimation(false, IDLEKEY, idleCrossfade);
                self.SetState(new IdleState());
            }
            else
            {
                self.SetState(new IdleState());
            }
        }

    }

    public void Crouching()
    {
        StartCoroutine(CrouchBlocking());
    }
    IEnumerator CrouchBlocking()
    {
        TransitionToAnimation(false, CROUCHKEY, crouchCrossfade);
        anim.speed = 1;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(CROUCHKEY))
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
            {
                yield return null;
            }
            for (int i = 0; i < playerLegGeometry.Length; i++)
            {
                var tempMaterial = playerLegGeometry[i].GetComponent<SkinnedMeshRenderer>();
                tempMaterial.material = legBlocking;
            }
            for (int i = 0; i < playerArmourLegGeometry.Length; i++)
            {
                var tempMaterial = playerArmourLegGeometry[i].GetComponent<MeshRenderer>();
                tempMaterial.material = armourBlocking;
            }
            //SetArmourToCrouchBlock();
            legShield.SetActive(true);
            self.CrouchBlocking = true;
        }
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
        legShield.SetActive(false);
        self.CrouchBlocking = false;
        self.SetState(new IdleState());
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
        TransitionToAnimation(false, FALLINGKEY, fallingCrossfade);
        anim.speed = 1;
    }

    public void Jumping()
    {
        TransitionToAnimation(false, JUMPINGKEY, jumpCrossfade);
        anim.speed = 1;
    }
    public void DoubleJump()
    {
        StartCoroutine(_DoulbeJump());
    }

    IEnumerator _DoulbeJump()
    {
        TransitionToAnimation(false, DOUBLEJUMPKEY, doubleJumpCrossfade);
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
        StartCoroutine(_Block());
    }
    IEnumerator _Block()
    {
        TransitionToAnimation(false, BLOCKKEY, blockCrossfade);

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(BLOCKKEY))
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
            torsoShield.SetActive(true);
        }

    }

    public void ExitBlock()
    {
        torsoShield.SetActive(false);
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

    public void HitKnockBack()
    {
        StartCoroutine(KnockBack());
    }

    IEnumerator KnockBack()
    {
        self.HitStun = true;
        self.SetState(new BusyState());
        yield return null;
        TransitionToAnimation(false, NORMALHITSTUNKEY, 0.01f);
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
        TransitionToAnimation(false, NORMALHITSTUNKEY, normalHitCrossfade);
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
    public void BlockKnockback()
    {
        StartCoroutine(_BlockKnockBack());
    }
    private IEnumerator _BlockKnockBack()
    {
        BlockCounter = 0;
        self.SetState(new BusyState());
        while (BlockCounter < MaxBlocktime)
        {
            yield return null;
        }
        self.CanActOutOf = true;
        self.SetState(new IdleState());
    }
    public void knockDown()
    {
        StartCoroutine(_KnockDown());
    }

    private IEnumerator _KnockDown()
    {
        self.CanActOutOf = false;
        self.HitStun = true;
        self.CanTurn = false;
        self.SetState(new BusyState());
        anim.speed = 2.5f;
        TransitionToAnimation(false, KNOCKDOWNKEY, knockDownCrossfade);
        //anim.Play("HITSTUN_NORMAL_HIT");
        //anim.Play("KNOCKDOWN_NORMAL");
        //grab stuff off server and do boots

        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            {
                yield return null;
            }
                yield return null;
        }
        _GetBackUp();
        //StopCoroutine(_KnockDown());
    }
    private void _GetBackUp()
    {
        //self.SetState(new BusyState());
        StartCoroutine(GetBackUp());
    }

    private IEnumerator GetBackUp()
    {
        self.HitStun = true;
        self.CanTurn = false;
        TransitionToAnimation(false, GETTINGUPKEY, getUpCrossfade);
        //anim.Play("GETTING_UP_NORMAL");

        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        self.HitStun = false;
        self.CanActOutOf = true;
        RevertBackToIdleState();
    }
}
