using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2, NullPlayer };

    public PlayerIndex playerNumber;
    [SerializeField] private VState _currentVerticalState;
    [SerializeField] private VState _previousVerticalState;
    [SerializeField] private bool standalone;
    [SerializeField] private string CurrentStateName;

    [SerializeField] public float gravityValue = -10f;
    [SerializeField] private float friction = 0.25f;
    [SerializeField] private float normalFriction, slideFriction;
    public bool landing = false;
    [SerializeField] private float speed = 6.5f;

    [SerializeField] private float weight = 22;
    [SerializeField] private Vector3 knockbackResistance;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject hitbox, blockObj;

    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private ArmourCheck armourCheck;
    [SerializeField] private Raycasts raycasts;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private GaugeManager gaugeManager;
    [SerializeField] private ParticleManager particleManager;


    private bool _DecreaseSlide;
    public bool DecreaseSlide { get { return _DecreaseSlide; } set { _DecreaseSlide = value; } }
    [Range(0, 5)]
    [SerializeField] private float _SlideValue;
    public float maxSliderValue;
    public int SliderCountUpSetValue;
    public float SlideValue { get { return _SlideValue; } set { _SlideValue = value; } }


    [Header("UI")]
    [SerializeField] public TMP_Text playerLives;
    [SerializeField] public Image playerImage;

    [Header("Observation Values")]
    [SerializeField] private float CurrentVelocity;
    [SerializeField] private float YVelocity;

    public bool DebugModeOn;

    private float distanceToGround;
    private float distanceToCeiling;
    private float distanceToRight;
    private float distanceToLeft;
    [SerializeField] private float jumpForce = 9;
    private float hitStunTimer;
    public float jabHitStun, sweepHitStun, heavyHitStun, aerialHitStun, armourBreakHitStun;
    public float HitStunTimer { get { return hitStunTimer; } set { hitStunTimer = value; } }
    [SerializeField] private float maxHitStunTime;
    public float MaxHitStun { get { return maxHitStunTime; } set { maxHitStunTime = value; } }
    [SerializeField] private bool _hitStun;

    [SerializeField] private bool _blocking;
    private bool _canTurn;
    private bool _canBlock;
    [SerializeField] private bool _canMove;
    [SerializeField] private bool crouchBlocking;
    public bool CrouchBlocking { get { return crouchBlocking; } set { crouchBlocking = value; } }

    private bool canAirMove;

    [SerializeField] private bool _gravityOn;

    public bool canDoubleJump;

    [SerializeField] private int _currentJumpIndex;
    private int maxJumps = 2;

    [SerializeField] private Vector3 overrideForce;

    private Wall currentWall;
    private PlayerState MyState;

    private bool _wasAttacking;
    [SerializeField] private int facingDirection;
    public int lives;
    private bool _inAir;
    [SerializeField] private bool _moving;


    public bool Moving { get { return _moving; } set { _moving = value; } }
    public bool WasAttacking { get { return _wasAttacking; } set { _wasAttacking = value; } }
    public bool GravityOn { get { return _gravityOn; } set { _gravityOn = value; } }
    public bool InAir { get { return _inAir; } set { _inAir = value; } }
    public bool CanTurn { get { return _canTurn; } set { _canTurn = value; } }
    public bool CanBlock { get { return _canBlock; } set { _canBlock = value; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public int CanJumpIndex { get { return _currentJumpIndex; } set { _currentJumpIndex = value; } }
    public bool GetCanAirMove() { return canAirMove; }
    public bool Blocking { get { return _blocking; } set { _blocking = value; } }
    public bool HitStun { get { return _hitStun; } set { _hitStun = value; } }
    public int GetMaxJumps() { return maxJumps; }
    [SerializeField] private float _SlideFriction;
    public float SlideFricton { get { return _SlideFriction; } }
    public VState VerticalState { get { return _currentVerticalState; } set { _currentVerticalState = value; } }
    //public VState PreviousVerticalState { get { return _previousVerticalState; } set { _previousVerticalState = value; } } 
    private bool _crouching;
    public bool Crouching { get { return _crouching; } set { _crouching = value; } }

    [SerializeField] private float attackedFreezeCounter;
    [SerializeField] private float MaxFreezeCounter;
    [SerializeField] private bool freezeAttackedPlayer;

    [SerializeField] private float attackingFreezeCounter;
    [SerializeField] private Vector3 _TempAttackingVelocity;
    [SerializeField] private bool freezeAttackingPlayer;
    private float _PlayerInput;
    private Vector3 _TempDirection;
    private Vector3 _tempPower;
    private Vector3 _attackersFacingDirection;

    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform StandaloneSpawnPoint;

    [SerializeField] private float moveCharacterOnXStrength;
    public float MoveCharacterOnXStrength {set { moveCharacterOnXCounter = value; } }

    [SerializeField] private float moveCharacterOnYStrength;
    public float MoveCharacterOnYStrength { set{ MoveCharacterOnYStrength = value; } }
    public float moveCharacterOnXCounter;
    public float moveCharacterOnYCounter;

    private float moveCharacterOnXMaxCounter;
    private float moveCharacterOnYMaxCounter;
    public bool primed;
    public bool inAction;
    public bool CanActOutOf;
    public void SetUpInputDetectionScript(PlayerInputHandler _playerInputDetection)
    {
        playerInputHandler = _playerInputDetection;
    }

    public void SetSpawnPoint(Transform _spawnPoint)
    {
        SpawnPoint = _spawnPoint;
    }
    //public int GetPlayerIndex() 
    //{ 
    //    return (int)playerNumber; 
    //} 
    public enum Wall
    {
        leftWall,
        rightWall,
        none
    }
    public enum VState
    {
        grounded,
        jumping,
        falling
    }
    //Turn back button UI back on later 
    void Awake()
    {
        moveCharacterOnXCounter = 5;
        moveCharacterOnYCounter = 5;
        MyState = new IdleState();
        _gravityOn = true;
        _canTurn = true;
        canAirMove = true;
        _canMove = true;
    }
    [SerializeField] private bool canDoAttack = false;
    public bool CanDoAttack { get { return canDoAttack; } set { canDoAttack = value; } }
    public float MoveCharacterCounter { set { moveCharacterOnXCounter = value; } }
    private void Start()
    {
        primed = true;
        _interuptSliderSetToZero = true;
        DecreaseSlide = true;
        if (playerNumber == PlayerIndex.Player1)
        {
            hitbox.gameObject.layer = 8;
            blockObj.gameObject.layer = 8;
            SpawnPoint = GameManager.instance.player1Spawn;
        }
        else if (playerNumber == PlayerIndex.Player2)
        {
            hitbox.gameObject.layer = 9;
            blockObj.gameObject.layer = 9;
            SpawnPoint = GameManager.instance.player2Spawn;
        }
        transform.position = SpawnPoint.transform.position;
        transform.rotation = SpawnPoint.transform.rotation;
    }

    private void Update()
    {
        moveCharacterOnXCounter += 3 * Time.deltaTime;
        moveCharacterOnYCounter += 1 * Time.deltaTime;
        if (moveCharacterOnXCounter > moveCharacterOnXMaxCounter)
        {
            moveCharacterOnXStrength = Mathf.Lerp(moveCharacterOnXStrength, 0, 2);
        }
        if (moveCharacterOnYCounter > moveCharacterOnYMaxCounter)
        {
            moveCharacterOnYStrength = Mathf.Lerp(moveCharacterOnYStrength, 0, 2);
        }
        CallRunState();
        if (_SlideValue < 0)
        {
            _SlideValue = 0;
        }
        if (_SlideValue >= maxSliderValue)
        {
            _SlideValue = maxSliderValue;
            friction = slideFriction;
        }
        else if (_SlideValue < maxSliderValue)
        {
            friction = normalFriction;
        }

        Delaycounter -= 1 * Time.deltaTime;
        if (Delaycounter <= 0)
        {
            if (_moving == false)
            {
                _SlideValue = 0;
            }
            Delaycounter = 0;
        }


    }
    [SerializeField] private bool _interuptSliderSetToZero;
    public bool InteruptSliderSetToZero { get { return _interuptSliderSetToZero; } set { _interuptSliderSetToZero = value; } }
    public void SetSlideValueToZero()
    {
        if (_moving == true)
        {
            return;
        }
        else if (_moving == false)
        {
            DelaySliderDecrease();
        }
    }
    [SerializeField] private float Delaycounter;
    private void DelaySliderDecrease()
    {
        if (_SlideValue == maxSliderValue)
        {
            Delaycounter = 0.5f;
        }
        else
        {
            Delaycounter = 0.2f;
        }

    }
    public void SetOverrideToZero()
    {
        overrideForce = Vector3.zero;
    }
    public void SetMoveStrengthXTo0()
    {
        moveCharacterOnXStrength = 0;
    }
    public void SetMoveStrengthYTo0()
    {
        moveCharacterOnYStrength = 0;
    }
    public void SetMoveCharacterOnXStrength(float strengthValue)
    {
        Debug.Log("Set X counter to 0 throught strength setter");
        moveCharacterOnXCounter = 0;
        moveCharacterOnXStrength = strengthValue;
    }
    public void SetMoveCharacterOnYStrength(float strengthValue)
    {
        moveCharacterOnYCounter = 0;
        moveCharacterOnYStrength = strengthValue;
    }
    private void MoveCharacterOnX()
    {
        if (!HitStun)
        {
            Debug.Log("Move Character On X");
            rb.velocity = new Vector3(facingDirection * moveCharacterOnXStrength, rb.velocity.y, 0);
        }

    }
    private void MoveCharacterOnY()
    {
        if (!HitStun)
        {
            Debug.Log("Move Character On Y");
            rb.velocity = new Vector3(rb.velocity.x, moveCharacterOnYStrength, 0);
        }

    }
    public void changeHeavyMoveValue(AttackType attackType, float value)
    {
        switch (attackType)
        {
            case AttackType.Heavy:
                playerActions.heavyVariables.MoveCharacterOnXStrength = value;
                break;
            case AttackType.Jab:
                playerActions.jabVariables.MoveCharacterOnXStrength = value;
                break;
        }

    }
    public void SetAddforceZero()
    {
        overrideForce = Vector3.zero;
    }



    public float MoveCharacterOnXMaxValue { set { moveCharacterOnXMaxCounter = value; } }
    public float MoveCharacterOnYMaxValue { set { moveCharacterOnYMaxCounter = value; } }
    private void FixedUpdate()
    {
        if (moveCharacterOnXCounter < moveCharacterOnXMaxCounter)
        {
            MoveCharacterOnX();

        }
        if (moveCharacterOnYCounter < moveCharacterOnYMaxCounter)
        {
            MoveCharacterOnY();
        }
        CheckDirection();
        FindFacingDirection();
        CharacterStates();
        Observation();
        GravityCheck();
        ReduceCounter();
    }
    private void CallRunState()
    {
        if (MyState != null)
        {
            MyState.RunState
        (
        this,
        rb,
        playerActions,
        armourCheck,
        new PlayerState.InputState()
        {
            horizontalInput = playerInputHandler.GetHorizontal(),
            attackInput = playerInputHandler.ShouldAttack(),
            jumpInput = playerInputHandler.ShouldJump(),
            crouchInput = playerInputHandler.ShouldCrouch(),
            armourBreakInput = playerInputHandler.ShouldArmourBreak(),
            blockInput = playerInputHandler.ShouldBlock(),
            heavyInput = playerInputHandler.ShouldHeavy(),
            upDirectionInput = playerInputHandler.ShouldUpDirection(),
            rightTriggerInput = playerInputHandler.ShouldRightTrigger(),
            leftTriggerInput = playerInputHandler.ShouldLeftTrigger(),
            rightBumperInput = playerInputHandler.ShouldRightBumper(),
            leftBumperInput = playerInputHandler.ShouldLeftBumper()

        },
        new PlayerState.Calculating()
        {
            jumpForce = JumpForceCalculator(),
            friction = friction,
            characterSpeed = SetPlayerSpeed(),
            overrideForce = overrideForce,
            gravityValue = _gravityOn ? totalGravityValue : 0
        }
        );
        }

    }
    #region State Machine 
    private void CharacterStates()
    {
        JumpingOrFallingTracker();
        CurrentStateName = MyState.GiveName();
        if (playerInputHandler == null)
        {
            return;
        }

        Debug.DrawRay(rb.position, overrideForce * 10, Color.yellow);
    }

    public PlayerState GetMyState()
    {
        return MyState;
    }
    public void SetState(PlayerState state)
    {
        MyState = state;
    }
    #endregion
    private void FindFacingDirection()
    {
        if (transform.rotation.y == 0)
        {
            facingDirection = -1;
        }
        else if (transform.rotation.y == -180)
        {
            facingDirection = 1;
        }
    }
    public void HideHitBoxes()
    {
        hitbox.gameObject.GetComponent<Hitbox>().HideHitBoxes();
    }
    private IEnumerator StopCharacter()
    {
        yield return new WaitForSeconds(0.1f);
        StopMovingCharacterOnXAxis();
    }
    public void StopMovingCharacterOnXAxis()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    public void StopMovingCharacterOnYAxis()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
    }

    #region Jumping 
    //void DoubleJumpCheck() 
    //{ 
    //    if (canDoubleJump == true) 
    //    { 
    //        _canTurn = true; 
    //        playerActions.DoubleJump(true); 
    //        canDoubleJump = false; 
    //    } 
    //} 

    public void MinusOneToJumpIndex()
    {
        _currentJumpIndex--;
    }
    public void AddOneToJumpIndex()
    {
        _currentJumpIndex++;
    }
    //void MinusJumpIndexWhenNotOnGround() 
    //{ 
    //    if(_currentJumpIndex == 0) 
    //    { 
    //        _currentJumpIndex = 1; 
    //    } 
    //} 
    #endregion
    public void ResetBlocking()
    {
        playerActions.ExitBlock();
    }

    //public void ResetCharacterMaterialToStandard() 
    //{ 
    //    playerActions.ResetMaterial(); 
    //} 
    #region Gravity methods 
    void TerminalVelocity()
    {
        if (rb.velocity.y < -10)
        {
            rb.velocity = new Vector3(playerInputHandler.GetHorizontal() * SetPlayerSpeed(), -10, 0) + overrideForce;
        }
    }
    void GravityCheck()
    {
        if (_gravityOn == false)
        {
            return;
        }
        else if (_gravityOn == true)
        {
            Gravity();
        }
    }
    public void MoveCharacterWithAttacks(float MoveStrength)
    {
        rb.velocity = new Vector3((facingDirection * (rb.velocity.x * MoveStrength)), rb.velocity.y, 0) * Time.deltaTime;
        rb.AddForce(new Vector3((facingDirection * MoveStrength), rb.velocity.y, 0));
        StartCoroutine(StopCharacter());
    }
    private float totalGravityValue;
    void Gravity()
    {
        TerminalVelocity();

        if (_currentVerticalState != VState.grounded)
        {
            if (_currentVerticalState == VState.falling)
            {
                gravityValue = 10;
            }
            else if (_currentVerticalState == VState.jumping)
            {
                if (gravityValue > 10)
                {
                    gravityValue = 10;
                }
                gravityValue = 10;
            }
            totalGravityValue = gravityValue * ((weight) / 10);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + -totalGravityValue * Time.deltaTime, 0);
        }
        else if (_currentVerticalState == VState.grounded && MyState.StickToGround())
        {
            //rb.velocity = new Vector3(rb.velocity.x, 0, 0); 
            //gravityValue = 0; 
        }
    }
    #endregion
    #region ReduceValues 
    void ReduceCounter()
    {
        ReduceHitForce();
        ReduceHitStun();
        ReduceAttackedFreezeFrameCounter();
        ReduceAttackingFreezeFrameCounter();
    }
    void ReduceAttackedFreezeFrameCounter()
    {
        attackedFreezeCounter -= 8f * Time.deltaTime;
        if (attackedFreezeCounter <= 0.001f)
        {
            attackedFreezeCounter = 0;
            if (freezeAttackedPlayer)
            {
                //rb.velocity = _TempAttackedVelocity; 
                GravityOn = true;
                freezeAttackedPlayer = false;
                playerActions.ResumeCurrentAnimation();
                Damage(_tempPower);
            }
        }
    }
    void ReduceAttackingFreezeFrameCounter()
    {
        attackingFreezeCounter -= 8f * Time.deltaTime;
        if (attackingFreezeCounter <= 0.001f)
        {
            attackingFreezeCounter = 0;
            if (freezeAttackingPlayer == true)
            {
                //rb.velocity = _TempAttackedVelocity; 
                //GravityOn = true; 
                freezeAttackingPlayer = false;
                playerActions.ResumeCurrentAnimation();
            }
        }
    }
    void ReduceHitForce()
    {
        overrideForce.x = Mathf.Lerp(overrideForce.x, 0, 7f * Time.deltaTime);
        overrideForce.y = Mathf.Lerp(overrideForce.y, 0, 7f * Time.deltaTime);
        //reducing to zero if small value 
        if (overrideForce.sqrMagnitude < 0.05f)
        {
            overrideForce = Vector3.zero;
        }
    }

    void ReduceHitStun()
    {
        if (_hitStun == true)
        {
            hitStunTimer -= 1 * Time.deltaTime;
            if (hitStunTimer < 0.001f)
            {
                CanActOutOf = true;
                hitStunTimer = 0;
                _hitStun = false;
            }
        }
    }
    #endregion
    #region Attacking and Damaging 
    public bool InKnockBack = false;
    public void SetHitStun(bool var, AttackType _attackType)
    {
        if (_attackType == AttackType.Jab)
        {
        }
    }
    public void KnockUp()
    {
        playerActions.KnockUp();
    }
    public void KnockDown()
    {
        playerActions.knockDown();
    }
    public void JabKnockBack()
    {
        playerActions.JabKnockBack();
    }
    public void KnockBack()
    {
        playerActions.HitKnockBack();
    }
    public void BlockKnockBack()
    {
        playerActions.BlockKnockback();
    }

    public void Damage(Vector3 Power)
    {
        //hitStun = true; 
        hitStunTimer = maxHitStunTime;
        overrideForce = Power - new Vector3(_attackersFacingDirection.x * armourCheck.knockBackResistance.x, 1 * armourCheck.knockBackResistance.y, 0) + knockbackResistance;
        // use knockbackValue x and y to determine reaction 
        // if y > 15, you're being kicked into the air 
        // if total magnitude > 40, you're going flying 
    }
    public void FreezeCharacterAttacking()
    {
        playerActions.PauseCurrentAnimation();
        freezeAttackingPlayer = true;
        _TempAttackingVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        GravityOn = false;
        attackingFreezeCounter = MaxFreezeCounter;
        _tempPower = Vector3.zero;
    }
    public void FreezeCharacterBeingAttacked(Vector3 Power, int facingDirection)
    {
        playerActions.PauseCurrentAnimation();
        freezeAttackedPlayer = true;
        GravityOn = false;
        attackedFreezeCounter = MaxFreezeCounter;
        _tempPower = Power;
        _attackersFacingDirection = new Vector3(facingDirection, 0, 0);

    }
    public void ResetGuage()
    {
        gaugeManager.resetGauge();
    }
    public void TakeDamageOnGauge(float amount, ArmourCheck.ArmourPlacement Placement, AttackType attackType, Vector3 _hitPosiiton)
    {

        gaugeManager.TakeDamage(amount, Placement, this, attackType, _hitPosiiton);
    }
    public void PlayArmourHitSound(bool Armour, AttackType attackType)
    {
        if (Armour)
        {
            switch (attackType)
            {
                case AttackType.Jab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.JABHITARMOUR);
                    break;
                case AttackType.LegSweep:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITARMOUR);
                    break;
                case AttackType.Aerial:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITARMOUR);
                    break;
                case AttackType.ArmourBreak:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITARMOUR);
                    break;
                case AttackType.Heavy:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITARMOUR);
                    break;
            }
        }
        else
        {
            switch (attackType)
            {
                case AttackType.Jab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.JABHITUNARMOURED);
                    break;
                case AttackType.LegSweep:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
                    break;
                case AttackType.Aerial:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
                    break;
                case AttackType.ArmourBreak:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
                    break;
                case AttackType.Heavy:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
                    break;
            }
        }
    }
    #endregion

    public float GetAbsolutInputValueForMovingAnimationSpeed()
    {
        return Mathf.Abs(_PlayerInput);
    }

    public void GetPlayerInputFromInputScript(float PlayerInput)
    {
        _PlayerInput = PlayerInput;
    }
    public float SetPlayerSpeed()
    {
        float characterSpeed = speed;
        if (_hitStun == true)
        {
            characterSpeed *= 0 + (5 * Time.deltaTime);
        }
        return characterSpeed;
    }
    public float JumpForceCalculator()
    {
        float jumpForceValue;
        if (_currentJumpIndex == maxJumps)
        {
            return rb.velocity.y;
        }
        else if (_currentJumpIndex < maxJumps)
        {
            if (_currentVerticalState == VState.grounded)
            {
                jumpForceValue = jumpForce;
                return jumpForceValue;
            }
            else if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling)
            {
                jumpForceValue = (jumpForce + 2);
                return jumpForceValue;
            }
        }
        return 0;
    }
    public void PlayParticle(ParticleType type, Vector3 hitPosition)
    {
        switch (type)
        {
            case ParticleType.Landing:
                particleManager.PlayLandOnGroundParticle();
                break;
            case ParticleType.DoubleJump:
                particleManager.PlayDoubleJumpParticle();
                break;
            case ParticleType.Running:
                break;
            case ParticleType.ArmourBreak:
                particleManager.PlayArmourBreakParticle();
                break;
            case ParticleType.BreakArmourPiece:
                particleManager.PlayerBreakArmourPiece(hitPosition);
                break;
        }
    }
    #region Direction Player is Facing 
    public int GetFacingDirection()
    {
        var _facingDirection = facingDirection;
        return _facingDirection;
    }
    private void CheckDirection()
    {
        if (playerInputHandler == null)
        {
            return;
        }
        int _facingDirection;
        if (transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            _facingDirection = 1;
            facingDirection = (int)_facingDirection;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            _facingDirection = -1;
            facingDirection = (int)_facingDirection;
        }
    }
    #endregion
    #region Jumping or Falling 
    public void JumpingOrFallingAnimations()
    {
        if (HitStun == false)
        {
            switch (_currentVerticalState)
            {
                case VState.falling:
                    playerActions.Falling();
                    break;
                case VState.jumping:
                    playerActions.Jumping();
                    break;
            }
        }

    }
    [SerializeField] private float previousYVelocity;
    void JumpingOrFallingTracker()
    {
        previousYVelocity = rb.velocity.y;
        if (_currentVerticalState != VState.grounded)
        {
            if (!HitStun)
            {
                if (previousYVelocity >= 0)
                {
                    _currentVerticalState = VState.jumping;
                    _gravityOn = true;
                }
                else if (previousYVelocity < 0)
                {
                    _currentVerticalState = VState.falling;
                    _gravityOn = true;
                }
            }
        }

        //if (rb.velocity.y != 0) 
        //{ 
        //    if (rb.velocity.y > 0f) 
        //    { 
        //        _currentVerticalState = VState.jumping; 
        //    } 
        //    else if (rb.velocity.y < -0f) 
        //    { 
        //        _currentVerticalState = VState.falling; 
        //    } 
        //} 

    }
    public void StartVerticalTracker()
    {
        StartCoroutine(VerticalTracker());
    }
    public IEnumerator VerticalTracker()
    {
        while (_currentVerticalState == VState.grounded)
        {
            yield return null;
        }
    }
    #endregion
    #region Raycasts 
    public void SetVelocityToZero()
    {
        rb.velocity = Vector3.zero;
    }
    public Wall GetCurrentWall()
    {
        return currentWall;
    }
    public void SetCurrentWallNone()
    {
        currentWall = Wall.none;
    }

    public void RaycastGroundCheck(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Platform")))
        {
            if (_currentVerticalState == VState.falling)
            {
                _gravityOn = false;
                PlayParticle(ParticleType.Landing, Vector3.zero);
                if (_hitStun != true)
                {
                    playerActions.Landing();
                }
                LandOnGround(hit);
            }
            AdjustPlayerOnGround(hit);
        }
        else if (!hit.collider.CompareTag("Ground") || (!hit.collider.CompareTag("Platform")))
        {

            //_gravityOn = true; 
        }

    }
    private void AdjustPlayerOnGround(RaycastHit hit)
    {
        distanceToGround = hit.distance;
        if (distanceToGround >= 0 && distanceToGround <= 0.5f)
        {
            rb.MovePosition(new Vector3(hit.point.x, hit.point.y, 0));
        }
        distanceToGround = hit.distance;
    }
    public void SetJumpIndexTo1()
    {
        _currentJumpIndex = 1;
    }
    private void LandOnGround(RaycastHit hit)
    {
        if (InKnockBack)
        {
            HitStun = false;
            InKnockBack = false;
            playerActions.GetBackUp();
        }
        _gravityOn = false;
        AdjustPlayerOnGround(hit);
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        //if (distanceToGround >= 0 && distanceToGround <= 0.37f) 
        //{ 
        //    rb.MovePosition(new Vector3(hit.point.x, hit.point.y, 0)); 
        //} 

        _currentVerticalState = VState.grounded;

        canDoubleJump = true;
        _currentJumpIndex = 0;
    }
    public void PlayerGroundedIsFalse()
    {
        //_gravityOn = true;
        _currentVerticalState = VState.falling;
    }
    public void RayCasterLeftWallCheck(RaycastHit hit)
    {
        if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling || _currentVerticalState == VState.grounded)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentWall = Wall.leftWall;
                HitLeft(hit);
            }
        }
    }
    public void HitLeft(RaycastHit hit)
    {
        distanceToLeft = hit.distance;
        if (distanceToLeft >= 0 && distanceToLeft <= 0.2f)
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        distanceToLeft = hit.distance;
        currentWall = Wall.leftWall;
    }
    public void RayCasterRightWallCheck(RaycastHit hit)
    {
        if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling || _currentVerticalState == VState.grounded)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentWall = Wall.rightWall;
                HitRight(hit);
            }
        }
    }
    public void HitRight(RaycastHit hit)
    {
        distanceToRight = hit.distance;
        if (distanceToRight >= 0 && distanceToRight <= 0.2f)
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        distanceToRight = hit.distance;
        currentWall = Wall.rightWall;
    }
    public void RayCastCeilingCheck(RaycastHit hit)
    {
        if (_currentVerticalState == VState.jumping)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                HitCeiling(hit);
            }
        }
    }
    public void HitCeiling(RaycastHit hit)
    {
        distanceToCeiling = hit.distance;
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        if (distanceToCeiling >= 0 && distanceToCeiling <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.9f, 0);
        }
        distanceToCeiling = hit.distance;
    }
    #endregion
    [SerializeField] Vector3 V3Velocity;
    void Observation()
    {
        V3Velocity = rb.velocity;
    }

}

