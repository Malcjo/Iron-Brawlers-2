using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerCharacterEnum.Characters character;
    public LevelEnum.LevelTypes levelTypes;
    [SerializeField]
    private Player player;
    private PlayerInput playerInput;
    //private PlayerControls playerControls;
    private Player.PlayerIndex _PlayerNumber;
    [SerializeField] public GameObject playerPrefab;

    [SerializeField] private GameObject sol = null;
    [SerializeField] private GameObject solAlt = null;
    [SerializeField] private GameObject goblin = null;
    [SerializeField] private GameObject goblinAlt = null;
    GameObject playerCharacter = null;

    public int PlayerIndex;
    [SerializeField] private float testfloat;
    [SerializeField] Scene currentScene;
    [SerializeField] Scene menuScene;
    [SerializeField] private bool readyAndWaiting = false;

    public bool canAct = false;
    public bool read = false;

    public bool CharaReadied = false;
    public bool LevelReadied = false;
    public float chara = 0;
    public bool primed = false;

    public float level = 0;

    [SerializeField]
    private bool JumpInputQueued;
    [SerializeField]
    private bool BlockInputQueued;
    [SerializeField]
    private bool AttackInputQueued;
    [SerializeField]
    private bool ArmourBreakQueued;
    [SerializeField]
    private bool CrouchInputHeld;
    [SerializeField]
    private bool blockInputHeld;
    [SerializeField]
    private bool ArmourBreakInputQueued;
    [SerializeField]
    private float HorizontalValue;
    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private bool heavyQueued;
    [SerializeField]
    private bool UpDirectionHeld;

    [SerializeField] Player.Wall currentWall;
    [SerializeField] private bool Standalone = false;

    private CameraScript cameraScript;
    [SerializeField] private bool rightBumperHeld;
    [SerializeField] private bool leftBumperHeld;
    [SerializeField] private bool leftTriggerHeld;
    [SerializeField] private bool rightTriggerHeld;
    [SerializeField] float joyStickDelay;
    [SerializeField] float contextValue;
    public bool exitingGame;
    private CharacterSelect characterSelect;
    private bool _inRoundStarter;
    [Space(10)]
    [SerializeField] public GameObject player1SolPortrait;
    [SerializeField] public GameObject player1SolAltPortrait;
    [SerializeField] public GameObject player1GoblinPortrait;
    [SerializeField] public GameObject player1GoblinAltPortrait;
    [Space(10)]
    [SerializeField] public GameObject player2SolPortrait;
    [SerializeField] public GameObject player2SolAltPortrait;
    [SerializeField] public GameObject player2GoblinPortrait;
    [SerializeField] public GameObject player2GoblinAltPortrait;

    [SerializeField] public Slider player1Slider;
    [SerializeField] public Slider player2Slider;

    public bool InRoundStarter { get { return _inRoundStarter; } set { _inRoundStarter = value; } }
    private BindToPlayer bind;
    public BindToPlayer Bind { get { return bind; } set { bind = value; } }



    private void Start()
    {
        player1SolPortrait.SetActive(false);
        player1SolAltPortrait.SetActive(false);
        player1GoblinPortrait.SetActive(false);
        player1GoblinAltPortrait.SetActive(false);

        player2SolPortrait.SetActive(false);
        player2SolAltPortrait.SetActive(false);
        player2GoblinPortrait.SetActive(false);
        player2GoblinAltPortrait.SetActive(false);
    }
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        StartCoroutine(DelayedStart());
        DontDestroyOnLoad(this.gameObject);

        exitingGame = false;
    }
    public void SetUpCharacterSelect(CharacterSelect CharSel)
    {
        characterSelect = CharSel;
    }
    public void SetUpBind(BindToPlayer _bind)
    {
        bind = _bind;
    }
    private void Update()
    {
        //if (GameManager.instance.TurnOffCanAct == true)
        //{
        //    canAct = false;
        //}
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4)) // check if in character select scene
        {
            if (PlayerIndex == 0)
            {
                if (bind.AnyKeyToContinue == true)
                {
                    if (Input.anyKey)
                    {
                        bind.SetLoadLevelToContinue();
                        //GameManager.instance.LoadLevelContinue();
                    }
                }
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex || SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(2).buildIndex)
        {
            ispaused = GameManager.instance.Paused;
        }

        joyStickDelay += 1 * Time.deltaTime;
        if (joyStickDelay >= 10)
        {
            joyStickDelay = 10;
        }
        //currentWall = player.GetCurrentWall();

        if (playerCharacter == null)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex || SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(2).buildIndex)
            {
                currentScene = SceneManager.GetActiveScene();
                StartGame();
            }
        }
        if (readyAndWaiting)
        {
            bind.ReadyPlayer(PlayerIndex);
            //GameManager.instance.ReadyPlayer(PlayerIndex);
        }
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex || SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(2).buildIndex)
        {
            ShouldPause();
        }
        if (ispaused)
        {
            player.CanTurn = false;
            if (rightBumperHeld == true && leftBumperHeld == true)
            {
                GameManager.instance.PausedGame(false);
                GameManager.instance.ExitBackToMenu();

            }
        }
        else if (ispaused == false)
        {
            if (player != null)
            {
                player.CanTurn = true;
            }
        }
    }
    public void SetAllInputsToZero()
    {
        Debug.Log("inputs to zero");
        HorizontalValue = 0;
        JumpInputQueued = false;
        AttackInputQueued = false;
        heavyQueued = false;
        blockInputHeld = false;
        CrouchInputHeld = false;

    }
    public bool CanControlCharacters;
    private bool CanControlCharacter()
    {
        return !GameManager.instance.StartRound;
    }
    [SerializeField] private bool ispaused;
    //private void FixedUpdate() 
    //{ 
    //    HorizontalValue = 1; 
    //    if (AttackInputQueued) 
    //    { 
    //        AttackInputQueued = false; 
    //    } 
    //    if (AttackInputQueued == false) 
    //    { 
    //        AttackInputQueued = true; 
    //    } 
    //}
    private void StartGame()
    {
        playerCharacter = Instantiate(playerPrefab);
        playerCharacter.SetActive(true);
        SetAllInputsToZero();
        playerCharacter.GetComponent<Player>().SetState(new IdleState());
        GameManager.instance.AddPlayerToList(playerCharacter);
        cameraScript = GameManager.instance.GetCameraScript();
        cameraScript.AddPlayers(playerCharacter);
        player = playerCharacter.GetComponent<Player>();
        player.SetUpInputDetectionScript(this);
        player.playerNumber = _PlayerNumber;
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());

    }
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.5f);
        canAct = true;
    }
    [SerializeField] public bool ChooseLevel = false;

    public void SetInput(PlayerInput input)
    {
        this.playerInput = input;
    }

    public void SetPlayerNumber(PlayerInputManager inputManager)
    {
        if (inputManager.playerCount == 1)
        {
            _PlayerNumber = Player.PlayerIndex.Player1;
        }
        else if (inputManager.playerCount == 2)
        {
            _PlayerNumber = Player.PlayerIndex.Player2;
        }
    }
    #region GetInputs
    public float GetHorizontal()
    {
        return HorizontalValue;
    }
    public bool ShouldJump()
    {
        if (JumpInputQueued)
        {
            JumpInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldAttack()
    {
        if (AttackInputQueued)
        {
            AttackInputQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldCrouch()
    {
        return CrouchInputHeld;
    }
    public bool ShouldBlock()
    {
        return blockInputHeld;

    }
    public bool ShouldArmourBreak()
    {
        if (ArmourBreakInputQueued)
        {
            ArmourBreakInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldHeavy()
    {
        if (heavyQueued)
        {
            heavyQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldUpDirection()
    {
        return UpDirectionHeld;
    }
    #endregion

    private bool _Paused;
    public bool Paused { get { return _Paused; } set { _Paused = value; } }
    private bool pausedQueued;

    public bool ShouldPause()
    {
        if (pausedQueued)
        {
            if (GameManager.instance.Paused)
            {
                GameManager.instance.PausedGame(false);
            }
            else if (GameManager.instance.Paused == false)
            {
                GameManager.instance.PausedGame(true);
            }
            pausedQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldRightBumper()
    {
        return rightBumperHeld;
    }
    public bool ShouldLeftBumper()
    {
        return leftBumperHeld;
    }
    public bool ShouldRightTrigger()
    {
        return rightTriggerHeld;
    }
    public bool ShouldLeftTrigger()
    {
        return leftTriggerHeld;
    }

    void CharacterSwitch()
    {
        character = (PlayerCharacterEnum.Characters)chara;
        switch (character)
        {
            case PlayerCharacterEnum.Characters.Sol:
                if (PlayerIndex == 1)
                {
                    characterSelect.player1Character1PortraitPuck.SetActive(true);
                    characterSelect.player1Character2PortraitPuck.SetActive(false);

                    characterSelect.player1Character1Background.SetActive(true);
                    characterSelect.player1Character2Background.SetActive(false);
                }
                else if (PlayerIndex == 2)
                {
                    characterSelect.player2Character1PortraitPuck.SetActive(true);
                    characterSelect.player2Character2PortraitPuck.SetActive(false);

                    characterSelect.player2Character1Background.SetActive(true);
                    characterSelect.player2Character2Background.SetActive(false);
                }
                if (bind.SolBeenPicked == false)
                {
                    playerPrefab = sol;
                }
                else if (bind.SolBeenPicked == true)
                {
                    playerPrefab = solAlt;
                }
                break;
            case PlayerCharacterEnum.Characters.Goblin:
                if (PlayerIndex == 1)
                {
                    characterSelect.player1Character1PortraitPuck.SetActive(false);
                    characterSelect.player1Character2PortraitPuck.SetActive(true);
                    
                    characterSelect.player1Character1Background.SetActive(false);
                    characterSelect.player1Character2Background.SetActive(true);
                }
                else if (PlayerIndex == 2)
                {
                    characterSelect.player2Character1PortraitPuck.SetActive(false);
                    characterSelect.player2Character2PortraitPuck.SetActive(true);
                    
                    characterSelect.player2Character1Background.SetActive(false);
                    characterSelect.player2Character2Background.SetActive(true);
                }
                if (bind.GoblinBeenPicked == false)
                {
                    playerPrefab = goblin;
                }
                else if (bind.GoblinBeenPicked == true)
                {
                    playerPrefab = goblinAlt;
                }
                break;
        }
    }
    //chara  = sol
    //chara 1 = goblin

    void LevelSwitch()
    {
        levelTypes = (LevelEnum.LevelTypes)level;
        switch (levelTypes)
        {
            case LevelEnum.LevelTypes.Bridge:
                characterSelect.LevelDisplay1Obj.SetActive(true);
                characterSelect.LevelDisplay2Obj.SetActive(false);
                
                characterSelect.level1DisplayImage.SetActive(false);
                characterSelect.level1HighlightImage.SetActive(true);
                characterSelect.level2DisplayImage.SetActive(true);
                characterSelect.level2HighlightImage.SetActive(false);
                break;
            case LevelEnum.LevelTypes.Clouds:
                characterSelect.LevelDisplay1Obj.SetActive(false);
                characterSelect.LevelDisplay2Obj.SetActive(true);
                
                characterSelect.level1DisplayImage.SetActive(true);
                characterSelect.level1HighlightImage.SetActive(false);
                characterSelect.level2DisplayImage.SetActive(false);
                characterSelect.level2HighlightImage.SetActive(true);
                break;
            case LevelEnum.LevelTypes.End:
                characterSelect.LevelDisplay1Obj.SetActive(false);
                characterSelect.LevelDisplay2Obj.SetActive(true);
                
                characterSelect.level1DisplayImage.SetActive(true);
                characterSelect.level1HighlightImage.SetActive(false);
                characterSelect.level2DisplayImage.SetActive(false);
                characterSelect.level2HighlightImage.SetActive(true);
                break;
        }
    }

    public void HorizontalInput(CallbackContext context)
    {

        if (currentScene.buildIndex == SceneManager.GetSceneByBuildIndex(4).buildIndex && canAct)
        {
            if (context.started)
            {
                if (joyStickDelay > 0.1f)
                {
                    contextValue = context.ReadValue<float>();
                    if (contextValue < 0)
                    {
                        contextValue = -1;
                    }
                    else if (contextValue > 0)
                    {
                        contextValue = 1;
                    }
                    joyStickDelay = 0;
                }

                testfloat = contextValue;

                primed = false;
                if (contextValue >= 1f)
                {
                    if (!ChooseLevel)
                    {
                        chara--;
                        if (chara < 0)
                        {
                            chara = (int)PlayerCharacterEnum.Characters.End - 1;//goblin 
                        }
                        CharacterSwitch();
                    }
                    else if (ChooseLevel)
                    {
                        level--;
                        if (level < 0)
                        {
                            level = (int)LevelEnum.LevelTypes.End - 1; // clouds 

                        }
                        LevelSwitch();
                    }
                }
                else if (contextValue <= 1f)
                {
                    if (!ChooseLevel)
                    {
                        chara++;
                        if (chara == (int)PlayerCharacterEnum.Characters.End)
                        {
                            chara = 0;//sol 
                        }
                        CharacterSwitch();
                    }
                    else if (ChooseLevel)
                    {
                        level++;
                        if (level == (int)LevelEnum.LevelTypes.End)
                        {
                            level = 0; // Bridge 

                        }
                        LevelSwitch();
                    }
                }
            }
            if (context.canceled)
            {
                primed = true;
            }
        }
        else
        {
            if (player != null)
            {
                if (CanControlCharacters)
                {
                    if (!_inRoundStarter)
                    {
                        horizontalInput = context.ReadValue<float>();
                        if (!_Paused)
                        {
                            if (horizontalInput <= 0.35f && horizontalInput >= -0.35f)
                            {
                                horizontalInput = 0;
                            }
                            HorizontalValue = horizontalInput;
                            player.GetPlayerInputFromInputScript(HorizontalValue);
                            WallCheck();
                        }

                    }

                }
            }
        }
    }
    //try determining with if the same character is chosen with the chara int variable
    public void SwitchModel(GameObject character, float currentCharacter)
    {
        if (CharaReadied == false)
        {
            if (chara == currentCharacter)
            {
                playerPrefab = character;
            }
        }
    }
    public void Activate(CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(4).buildIndex)
        {
            if (canAct)
            {
                if (context.started)
                {
                    primed = false;
                    if (!ChooseLevel)
                    {
                        //    switch (chara) 
                        //    { 
                        //        case 0: 

                        //            switch (PlayerIndex) 
                        //            { 

                        //            } 
                        //            break; 

                        //        case 1: 

                        //            break; 
                        //    } 
                        if (chara == 0) //Sol Picked 
                        {
                            if (PlayerIndex == 1) // player 1 
                            {
                                characterSelect.player1CharacterPuck.SetActive(true);
                                //GameManager.instance.player1Character1Selected.SetActive(true); 
                                characterSelect.player1Character1PortraitPuck.SetActive(false);
                                if (bind.SolBeenPicked == true)
                                {
                                    characterSelect.player1SolAltAnimated.SetActive(true);
                                    player1SolAltPortrait.SetActive(true);

                                    bind.ChangeCharacterModelIfSameIsChosen(1, solAlt, 0);
                                    Animator anim = characterSelect.player1SolAltAnimated.GetComponentInChildren<Animator>();
                                    anim.SetTrigger("Emote");

                                }
                                else if (bind.SolBeenPicked == false)
                                {
                                    Animator anim = characterSelect.player1SolAnimated.GetComponentInChildren<Animator>();
                                    anim.SetTrigger("Emote");
                                   characterSelect.player1SolAnimated.SetActive(true);
                                    player1SolPortrait.SetActive(true);
                                    bind.SolBeenPicked = true;
                                }
                                CharaReadied = true;
                            }

                            else if (PlayerIndex == 2) // player 2 
                            {
                                characterSelect.player2CharacterPuck.SetActive(true);
                                //GameManager.instance.player2Character1Selected.SetActive(true); 
                                characterSelect.player2Character1PortraitPuck.SetActive(false);
                                if (bind.SolBeenPicked == true)
                                {
                                    Animator anim = characterSelect.player2SolAltAnimated.GetComponentInChildren<Animator>();
                                    anim.SetTrigger("Emote");
                                    characterSelect.player2SolAltAnimated.SetActive(true);
                                    player2SolAltPortrait.SetActive(true);

                                    bind.ChangeCharacterModelIfSameIsChosen(2, solAlt, 0);
                                }
                                else if (bind.SolBeenPicked == false)
                                {
                                    Animator anim = characterSelect.player2SolAnimated.GetComponentInChildren<Animator>();
                                    anim.SetTrigger("Emote");
                                    characterSelect.player2SolAnimated.SetActive(true);
                                    player2SolPortrait.SetActive(true);
                                    bind.SolBeenPicked = true;
                                }
                                CharaReadied = true;
                            }
                        }


                        else if (chara == 1) //Goblin Picked 
                        {
                            Debug.Log("Goblin");
                            if (PlayerIndex == 1) // player 1 
                            {
                                characterSelect.player1CharacterPuck.SetActive(true);
                                //GameManager.instance.player1Character2Selected.SetActive(true); 
                                characterSelect.player1Character2PortraitPuck.SetActive(false);
                                if (bind.GoblinBeenPicked == true)
                                {
                                    characterSelect.player1GoblinAltAnimated.SetActive(true);
                                    player1GoblinAltPortrait.SetActive(true);

                                    bind.ChangeCharacterModelIfSameIsChosen(1, goblinAlt, 1);
                                }
                                else if (bind.GoblinBeenPicked == false)
                                {
                                    characterSelect.player1GoblinAnimated.SetActive(true);
                                    player1GoblinPortrait.SetActive(true);
                                    bind.GoblinBeenPicked = true;
                                }
                                CharaReadied = true;
                            }

                            else if (PlayerIndex == 2) //player 2 
                            {
                                characterSelect.player2CharacterPuck.SetActive(true);
                                //GameManager.instance.player2Character2Selected.SetActive(true); 
                                characterSelect.player2Character2PortraitPuck.SetActive(false);
                                if (bind.GoblinBeenPicked == true)
                                {
                                    characterSelect.player2GoblinAltAnimated.SetActive(true);
                                    player2GoblinAltPortrait.SetActive(true);

                                    bind.ChangeCharacterModelIfSameIsChosen(2, goblinAlt, 1);
                                }
                                else if (bind.GoblinBeenPicked == false)
                                {
                                    characterSelect.player2GoblinAnimated.SetActive(true);
                                    player2GoblinPortrait.SetActive(true);
                                    bind.GoblinBeenPicked = true;
                                }
                                CharaReadied = true;
                            }
                        }
                        if (CharaReadied)
                        {
                            ChooseLevel = true;

                            readyAndWaiting = true;
                        }


                    } // Choose Character 
                    else if (ChooseLevel)
                    {
                        bind.TurnOffCanAct();
                        //GameManager.instance.TurnOffCanAct = true;
                        //canAct = false;
                        bind.SetLevelNumber((int)level + 1);
                        SetAllInputsToZero();
                        ChooseLevel = false;
                        bind.StartGame = true;

                    }
                }
            }
        }
    }
    public void BackInput(CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(4).buildIndex)
        {
            if (canAct)
            {
                if (!ChooseLevel)
                {

                    //GameManager.instance.player1pressAToJoinUI.SetActive(true); 
                    //GameManager.instance.player1Character1PortraitPuck.SetActive(false); 
                    //GameManager.instance.player1Character1Background.SetActive(false); 
                    //GameManager.instance.player2pressAToJoinUI.SetActive(true); 
                    //GameManager.instance.player2Character1PortraitPuck.SetActive(false); 
                    //GameManager.instance.player2Character1Background.SetActive(false); 
                    //GameManager.instance.Character1BeenPicked = false; 
                    //GameManager.instance.Character2BeenPicked = false; 
                    //GameManager.instance.TurnOffCharacterSelectObj(); 
                    //GameManager.instance.TurnOnMenuObj(); 
                    SceneManager.LoadScene(3);
                }
                else
                {
                    if (PlayerIndex == 1)
                    {
                        chara = 0;
                    }
                    else if (PlayerIndex == 2)
                    {
                        chara = 0;
                    }
                    primed = true;
                    ChooseLevel = false;
                    CharaReadied = false;
                }
            }

        }
    }
    public void JumpInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started && primed)
                {
                    primed = false;
                    JumpInputQueued = true;
                }
                if (context.canceled)
                {
                    primed = true;
                }
            }

        }
    }

    public void AttackInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started && primed)
                {
                    AttackInputQueued = true;
                    primed = false;
                }
                if (context.canceled)
                {
                    primed = true;
                }
            }

        }

    }
    public void CrouchInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started)
                {
                    CrouchInputHeld = true;
                }
                if (context.canceled)
                {
                    CrouchInputHeld = false;
                }
            }

        }

    }

    public void BlockInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started)
                {
                    blockInputHeld = true;
                }
                if (context.canceled)
                {
                    blockInputHeld = false;
                }
            }
        }
    }
    public void RightBumperInput(CallbackContext context)
    {
        if (CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started)
                {
                    rightBumperHeld = true;
                }
                if (context.canceled)
                {
                    rightBumperHeld = false;
                }
            }
        }
    }
    public void LeftBumperInput(CallbackContext context)
    {
        if (CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started)
                {
                    leftBumperHeld = true;
                }
                if (context.canceled)
                {
                    leftBumperHeld = false;
                }
            }
        }
    }

    public void RightTriggerInput(CallbackContext context)
    {
        if (CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started)
                {
                    rightTriggerHeld = true;
                }
                if (context.canceled)
                {
                    rightTriggerHeld = false;
                }
            }
        }
    }
    public void LeftTriggerInput(CallbackContext context)
    {
        if (CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started && primed)
                {
                    primed = false;
                    leftTriggerHeld = true;
                }
                if (context.canceled)
                {
                    primed = true;
                    leftTriggerHeld = false;
                }
            }
        }
    }
    public void ArmourBreakInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started & primed)
                {
                    primed = false;
                    ArmourBreakInputQueued = true;
                }
                if (context.canceled)
                {
                    primed = true;
                }
            }
        }

    }
    public void HeavyInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacters)
        {
            if (!_inRoundStarter)
            {
                if (context.started && primed)
                {
                    primed = false;
                    heavyQueued = true;
                }
                if (context.canceled)
                {
                    primed = true;
                }
            }
        }
    }

    public void UpDirectionInput(CallbackContext context)
    {
        if (!ispaused || CanControlCharacter())
        {
            if (context.started)
            {
                UpDirectionHeld = true;
            }
            if (context.canceled)
            {
                UpDirectionHeld = false;
            }
        }

    }

    public void StartButton(CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex || SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(2).buildIndex)
        {
            if (CanControlCharacter())
            {
                if (context.started)
                {
                    pausedQueued = true;
                }
            }

        }
    }

    public void WallCheck()
    {
        switch (player.GetCurrentWall())
        {
            case Player.Wall.none:
                break;
            case Player.Wall.leftWall:
                if (HorizontalValue < 0)
                {
                    HorizontalValue = 0;
                }
                break;
            case Player.Wall.rightWall:
                if (HorizontalValue > 0)
                {
                    HorizontalValue = 0;
                }
                break;
        }
    }
}

