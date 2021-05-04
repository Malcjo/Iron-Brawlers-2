using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
//using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net.Http.Headers;

public enum MenuLayer { Title, Main_Menu, Character_Select, Stage_Select, Settings, credits, GameScreen}
public class GameManager : MonoBehaviour
{
    public bool AnyKeyToContinue;
    public bool Continue;
    MenuLayer currentScreen;
    MenuLayer PreviousLayer;
    public PlayerInputManager inputManager;
    //[SerializeField] private Timer timerScript;
    [SerializeField] public List<GameObject> players = new List<GameObject>();
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] GameObject Title, MenuGroup, MainMenu, CharacterSelect, GameUIGroup, MainMenuAssetsGRP, ChooseYourStageText, ChooseYourCharacterText;
    [SerializeField] Animator blackFadeAnim, RoundsAnim;
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject mainCamera;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] BindToPlayer bindToPlayer;
    [SerializeField] TitleScreen titleScreen;
    public bool inGame = false;
    [SerializeField] bool player1Ready;
    [SerializeField] bool player2Ready;
    public Transform player1Spawn, player2Spawn;
    public int player1Rounds, player2Rounds;
    [SerializeField] public int sceneIndex;
    private CameraScript cameraScript;
    public GameObject uimodule;
    [SerializeField] private float leftBounds, rightBounds, belowBounds, highBounds;

    [Header("In Game UI")]
    [SerializeField] GameObject player1Wins, player2Wins, player1Loses, bothLose;
    
    [SerializeField] private Slider player1UI, player2UI;

    [SerializeField] private GameObject player1Round1, player1Round2, player1Round3;
    [SerializeField] private GameObject player2Round1, player2Round2, player2Round3;

    [SerializeField] private GameObject player1Head, player1Chest, player1Legs;
    [SerializeField] private GameObject player2Head, player2Chest, player2Legs;




    public bool ChooseLevel = false;

    [Header("Menu UI")]

    public GameObject player1SolAnimated, player1SolAltAnimated, player1GoblinAnimated, player1GoblinAltAnimated;
    public GameObject player2SolAnimated, player2SolAltAnimated, player2GoblinAnimated, player2GoblinAltAnimated;
    public GameObject player1Character1PortraitPuck, player1Character2PortraitPuck;
    public GameObject player2Character1PortraitPuck, player2Character2PortraitPuck;
    public GameObject player1CharacterPuck, player2CharacterPuck;

    public GameObject Display1, Display2, Display3, Display4;

    public GameObject player1Character1Selected, player1Character2Selected;
    public GameObject player2Character1Selected, player2Character2Selected;
    public GameObject character1ButtonSelected, character2ButtonSelected;

    public GameObject LevelDisplay1Obj, LevelDisplay2Obj;
    public GameObject level1DisplayImage, level1HighlightImage;
    public GameObject level2DisplayImage, level2HighlightImage;

    public GameObject player1pressAToJoinUI, player2pressAToJoinUI;
    public GameObject player1Character1Background, player1Character2Background;
    public GameObject player2Character1Background, player2Character2Background;

    public GameObject player1SolPortrait, player1SolAltPortrait, player1GoblinPortrait, player1GoblinAltPortrait;
    public GameObject player2SolPortrait, player2SolAltPortrait, player2GoblinPortrait, player2GoblinAltPortrait;

    [SerializeField] private LoadLevel loadLevelScript;

    [SerializeField] private GameObject PauseScreen;
    private void ResetCharacterAndLevelUI()
    {

        ChooseLevel = false;
        ChooseYourStageText.SetActive(false);
        //ChooseYourCharacterText.SetActive(false);
        LevelDisplay1Obj.SetActive(false);
        LevelDisplay2Obj.SetActive(false);
        Display2.GetComponent<Animator>().Play("CharacterIdle");
        Display3.GetComponent<Animator>().Play("CharacterIdle");

        player1SolAnimated.SetActive(false);
        player1SolAltAnimated.SetActive(false);
        player1GoblinAnimated.SetActive(false);
        player1GoblinAltAnimated.SetActive(false);

        player1Character1PortraitPuck.SetActive(false);
        player1Character2PortraitPuck.SetActive(false);

        //player1CharacterPuck.SetActive(false);


        player2SolAnimated.SetActive(false);
        player2SolAltAnimated.SetActive(false);
        player2GoblinAnimated.SetActive(false);
        player2GoblinAltAnimated.SetActive(false);

        player2Character1PortraitPuck.SetActive(false);
        player2Character2PortraitPuck.SetActive(false);

        //player2CharacterPuck.SetActive(false);
    }

    public bool Character1BeenPicked = true;
    public bool Character2BeenPicked = true;
    public bool canJoin = false;

    public bool StartGame = false;
    public bool levelSelect = false;
    public bool CanChooseLevel = true;
    /*
     * 0 = title
     * 1 = main meuu
     * 2 = Character select
     * 3 = stage select
     * 4 = settings
     * 5 = credtis
     * 6 = concept art
     * 7 = In game
     * 8 = Pause Screen
     * 9 = Victory screen
     * 10 = CONTROLS
     * 11 = AUDIO
     * 12 = GRAPHICS
     */

    /* Armour & no Armour Hit Particles
     * Tweak knockback strength and direction
     * Armour Gauge repairing rate
     * Armour Effecting knockback
     * Animation Exit time and cancelling animations
     * roll
     * back aerial
     * up air
     * cross fade
     * 
     */
    public static GameManager instance;
    [SerializeField] private bool _RoundStart;
    public bool RoundStart {get { return _RoundStart; } set { _RoundStart = value; } }
    [SerializeField] private float roundStartCounter;
    [SerializeField] private GameObject ReadyUI;
    [SerializeField] private GameObject FightUI;

    private bool paused;
    public bool Paused { get{ return paused; }}
    public void PausedGame(bool val)
    {
        if(val == true)
        {
            PauseScreen.SetActive(true);
            paused = val;
            Time.timeScale = 0;
        }
        else if (val == false)
        {
            PauseScreen.SetActive(false);
            paused = val;
            Time.timeScale = 1;
        }
    }
    public bool TurnOffCanAct;

    public void LoadLevelContinue()
    {
        bindToPlayer.SetLoadLevelToContinue();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        sceneIndex = 0;
        if (eventSystem != null)
        {
            eventSystem = GameObject.FindWithTag("Event");
            inputManager = eventSystem.GetComponent<PlayerInputManager>();
        }
        Debug.Log("Reset On Awake");
        ResetCharacterAndLevelUI();
    }
    private void Start()
    {
        canJoin = false;
        ConnectToGameManager(0);
        if (sceneIndex == 1)
        {
            ResetMenu();
        }
    }
    private void Update()
    {
        TrackPlayers();
        EnableJoiningManager();
        //TrackPlayerRounds();
        TrackPlayersArmour();
    }
    public void CloseProgram()
    {
        Application.Quit();
    }
    public void ReadyPlayer(int i)
    {
        switch (i)
        {
            case 1:
                player1Ready = true;
                break;
            case 2:
                player2Ready = true;
                break;
        }
    }
    public void EnableEventSystemOBJ()
    {
        eventSystem.SetActive(true);
    }
    public void SelectPlayButton()
    {
        PlayButton.Select();
        eventSystem = GameObject.FindWithTag("Event");
        EventSystem system = eventSystem.gameObject.GetComponent<EventSystem>();
        system.firstSelectedGameObject = PlayButton.gameObject;
        system.SetSelectedGameObject(system.firstSelectedGameObject);
    }
    public void SetLevelNumber(int var)
    {
        loadLevelScript.SetLevelSelectedNumber(var);
    }
    public void TransitionToLevelSelect()
    {
        _TransitionToLevelSelect();
    }
    private bool LevelTransitioned = false;
    private void _TransitionToLevelSelect()
    {
        Animator _display1 = Display1.GetComponent<Animator>();
        Animator _display2 = Display2.GetComponent<Animator>();
        Animator _display3 = Display3.GetComponent<Animator>();
        if (LevelTransitioned == false)
        {
            LevelTransitioned = true;
            StartCoroutine(PlayerDisplay1Animation(_display1));
            StartCoroutine(PlayerDisplay2Animation(_display2));
            StartCoroutine(PlayerDisplay3Animation(_display3));

            ChooseYourCharacterText.SetActive(false);
            ChooseYourStageText.SetActive(true);
            LevelDisplay1Obj.SetActive(true);
            LevelDisplay2Obj.SetActive(false);

            level1DisplayImage.SetActive(false);
            level1HighlightImage.SetActive(true);
            level2DisplayImage.SetActive(true);
            level2HighlightImage.SetActive(false);
        }
    }
    public void ResetLevelSelectDisplay()
    {
        _ResetLevelSelectDispaly();
    }
    private void _ResetLevelSelectDispaly()
    {
        Animator _display1 = Display1.GetComponent<Animator>();
        Animator _display2 = Display2.GetComponent<Animator>();
        Animator _display3 = Display3.GetComponent<Animator>();

        StopCoroutine(PlayerDisplay1Animation(_display1));
        StopCoroutine(PlayerDisplay2Animation(_display2));
        StopCoroutine(PlayerDisplay3Animation(_display3));

        _display1.Play("CharacterIdle");
        _display2.Play("CharacterIdle");
        _display3.Play("CharacterIdle");

        LevelTransitioned = false;
        ChooseYourCharacterText.SetActive(true);
        ChooseYourStageText.SetActive(false);
        LevelDisplay1Obj.SetActive(false);
        LevelDisplay2Obj.SetActive(true);

        level1DisplayImage.SetActive(true);
        level1HighlightImage.SetActive(false);
        level2DisplayImage.SetActive(false);
        level2HighlightImage.SetActive(true);
        if (LevelTransitioned)
        {

        }
    }
    IEnumerator PlayerDisplay1Animation(Animator anim)
    {
        anim.Play("Transition");
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        anim.Play("LevelIdle");
    }
    IEnumerator PlayerDisplay2Animation(Animator anim)
    {
        anim.Play("Transition");
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        anim.Play("LevelIdle");
    }
    IEnumerator PlayerDisplay3Animation(Animator anim)
    {
        anim.Play("Transition");
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        anim.Play("LevelIdle");
    }
    public void ChangeCharacterModelIfSameIsChosen(int index, GameObject character, float currentCharacter)
    {
        GameObject _characterSelect;
        _characterSelect = CharacterSelect;
        if (index == 1)
        {
            BindToPlayer bind;

            bind = _characterSelect.GetComponent<BindToPlayer>();
            bind.players[0].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
        else if(index == 2)
        {
            BindToPlayer bind;

            bind = _characterSelect.GetComponent<BindToPlayer>();
            bind.players[1].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
    }
    public void DisableEventSystemOBJ()
    {
        eventSystem.SetActive(false);
    }
    public void SetCameraScript(CameraScript script)
    {
        cameraScript = script;
    }
    public CameraScript GetCameraScript()
    {
        return cameraScript;
    }
    public void EnableMenuCanvas()
    {
        MenuGroup.SetActive(true);
        GameUIGroup.SetActive(false);
    }
    public void DisableMenuCanvas()
    {
        MenuGroup.SetActive(false);
        GameUIGroup.SetActive(true);
    }
    public void GetCameraObject(GameObject cam)
    {
        mainCamera = cam;
    }
    public Slider GetPlayer1UI()
    {
        return player1UI;
    }
    public Slider GetPlayer2UI()
    {
        return player2UI;
    }
    public void TurnOnMenuObj()
    {
        MainMenu.SetActive(true);
    }
    public void TurnOffCharacterSelectObj()
    {
        CharacterSelect.SetActive(false);
    }
    public void ConnectToGameManager(int CameraType)
    {
        Invoke("MoveGameManagerOutOfDontDestroy", 1);
        ConnectToCanvas(CameraType);
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(0).buildIndex)
        {
            if (titleScreen.haveBeenThroughTitle)
            {
                titleScreen.SetInputSystemModule();
                titleScreen.TurnOnInputSystem();
            }

        }
        loadLevelScript.SetMusicFade();
        MainMenuAssetsGRP = GameObject.FindGameObjectWithTag("MenuAsset");
        Invoke("SetThisToDontDestroy", 1);
    }
    private void ConnectToCanvas(int CameraType)
    {
        GameObject camObj = mainCamera;
        Camera cam = camObj.gameObject.GetComponent<Camera>();
        Canvas canvas = mainCanvas.GetComponent<Canvas>();
        if (CameraType == 0)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
        else if (CameraType == 1)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        canvas.worldCamera = cam;
    }
    private void SetThisToDontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void MoveGameManagerOutOfDontDestroy()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    public void ResetBindToPlayer()
    {
        bindToPlayer.ResetBindToPlayer();
    }
    public void ResetMenu()
    {
        EnabledJoining();
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        CharacterSelect.SetActive(false);
        MainMenu.SetActive(true);
        eventSystem = GameObject.FindGameObjectWithTag("Event");
        inputManager = eventSystem.gameObject.GetComponent<PlayerInputManager>();
        eventSystem.gameObject.GetComponent<EventSystem>().firstSelectedGameObject = PlayButton.gameObject;
        PlayButton.Select();
    }

    public void SwitchOnCharacterSelect()
    {
        Invoke("CharacterSelectTransition", 0.2f);
    }
    private void CharacterSelectTransition()
    {
        MainMenu.SetActive(false);
        CharacterSelect.SetActive(true);
        Debug.Log("set main asset group false");
        MainMenuAssetsGRP.SetActive(false);
    }
    public void SetPlayerSpawns(Transform _player1Spawn, Transform _player2Spawn)
    {
        player1Spawn = _player1Spawn;
        player2Spawn = _player2Spawn;
    }
    //public void ReadyPlayer()
    //{
    //    if (player1Ready == true)
    //    {
    //        Debug.Log("Player 2 set ready");
    //        player2Ready = true;
    //        player1SolAltAnimated.SetActive(true);
    //        player2CharacterPuck.SetActive(true);
    //        player1Character2PortraitPuck.SetActive(false);
    //    }
    //    else
    //    {
    //        player1Ready = true;
    //        player1SolAnimated.SetActive(true);
    //        player1CharacterPuck.SetActive(true);
    //        player1Character1PortraitPuck.SetActive(false);
    //    }
    //}

    public bool GetPlayer1Ready()
    {
        return player1Ready;
    }
    public bool GetPlayer2Ready()
    {
        return player2Ready;
    }
    public void ResetPlayersReady()
    {
        Debug.Log("Reset");
        player1Ready = false;
        player2Ready = false;
        player1SolAnimated.SetActive(false);
        player1SolAltAnimated.SetActive(false);
        player1GoblinAnimated.SetActive(false);
        player1GoblinAltAnimated.SetActive(false);
        player2SolAnimated.SetActive(false);
        player2SolAltAnimated.SetActive(false);
        player2GoblinAnimated.SetActive(false);
        player2GoblinAltAnimated.SetActive(false);
        LevelDisplay1Obj.SetActive(false);
        LevelDisplay2Obj.SetActive(false);
    }
    public void AllowPlayersToJoin()
    {
        inputManager.EnableJoining();
    }
    public void StopPlayersFromJoining()
    {
        inputManager.DisableJoining();
    }
    public void ChangeSceneIndex(int index)
    {
        sceneIndex = index;
    }
    private void EnableJoiningManager()
    {
        switch (sceneIndex)
        {
            case 0:
                foreach (Player ob in FindObjectsOfType(typeof(Player)))
                {
                    Destroy(ob);
                }
                inputManager.DisableJoining();
                break;
            case 1:
                inputManager.DisableJoining();
                break;
            case 2:
                //inputManager.EnableJoining();
                break;
            case 3:
                inputManager.DisableJoining();
                break;
            case 4:
                inputManager.DisableJoining();
                break;
            case 5:
                inputManager.DisableJoining();
                break;
            case 6:
                inputManager.DisableJoining();
                break;
            case 7:
                belowBounds = 0;
                inputManager.DisableJoining();
                break;
            case 8:
                inputManager.DisableJoining();
                break;
            case 9:
                inputManager.DisableJoining();
                break;
        }
    }
    public void EnabledJoining()
    {
        StartCoroutine(DelayedEnabled());
    }
    public void DisableJoining()
    {
        inputManager.DisableJoining();
    }
    IEnumerator DelayedEnabled()
    {
        yield return new WaitForSeconds(0.2f);
        canJoin = true;
        inputManager.EnableJoining();
    }

    public int draws;
    public void TimerRunOut()
    {
        draws++;
        ResetPlayers();
    }
    [SerializeField] private float maxTimer;
    public void SetMaxTimer(float max)
    {
        maxTimer = max;
    }
    private void TrackPlayersArmour()
    {
        TrackPlayer1Armour();
        TrackPlayer2Armour();
    }
    private void TrackPlayer1Armour()
    {
        if(players.Count >= 1)
        {
            ArmourCheck playerArmour = players[0].GetComponent<ArmourCheck>();
            if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.armour)
            {
                player1Head.SetActive(true);
            }
            else if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                player1Head.SetActive(false);
            }

            if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player1Chest.SetActive(true);
            }
            else if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player1Chest.SetActive(false);
            }

            if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player1Legs.SetActive(true);
            }
            else if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player1Legs.SetActive(false);
            }
        }
    }

    private void TrackPlayer2Armour()
    {
        if(players.Count > 1)
        {
            ArmourCheck playerArmour = players[1].GetComponent<ArmourCheck>();
            if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.armour)
            {
                player2Head.SetActive(true);
            }
            else if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                player2Head.SetActive(false);
            }

            if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player2Chest.SetActive(true);
            }
            else if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player2Chest.SetActive(false);
            }

            if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player2Legs.SetActive(true);
            }
            else if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player2Legs.SetActive(false);
            }
        }
    }
    private void TrackPlayers()
    {
        if (players != null)
        {
            if (players.Count > 0)
            {

                TrackPlayer1();
                if (players.Count > 1)
                {

                    TrackPlayer2();
                }
                if (player1Rounds == 3 || player2Rounds == 3)
                {
                    if (players.Count > 1)
                    {
                        if (player1Rounds > player2Rounds)
                        {
                            player1Wins.SetActive(true);

                            Destroy(players[1].gameObject);
                        }
                        else if (player2Rounds > player1Rounds)
                        {
                            player2Wins.SetActive(true);

                            Destroy(players[0].gameObject);
                        }
                    }
                    else
                    {
                        player2Wins.SetActive(true);
                        Destroy(players[0].gameObject);
                    }
                    Invoke("ExitBackToMenu", 2);
                }
            }
        }
    }
    public void ExitBackToMenu()
    {

        player1Rounds = 0;
        player2Rounds = 0;
        //Debug.Log("loading");
        loadLevelScript.TransitionBackToMainMenu();
    }
    //public bool InRoundStarter;
    public void AddPlayerToList(GameObject player)
    {
        players.Add(player);
    }
    public void TurnOnMenuINteraction()
    {
        titleScreen.TurnOnInputSystem();
    }
    public void ClearBindToPlayer()
    {
        if(bindToPlayer.players.Count> 1)
        {
            //SceneManager.MoveGameObjectToScene(bindToPlayer.players[1].gameObject, SceneManager.GetActiveScene());
            Destroy(bindToPlayer.players[1].gameObject);
        }
        //SceneManager.MoveGameObjectToScene(bindToPlayer.players[0].gameObject, SceneManager.GetActiveScene());
        Destroy(bindToPlayer.players[0].gameObject);
        bindToPlayer.ResetBindToPlayer();

        ExitBackToMenu();
    }
    public void RoundStartCountDown()
    {
        roundStartCounter = 0;
        if (players.Count > 1)
        {
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().InRoundStarter = true;
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().CanControlCharacters = false;
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().primed = false;
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().SetAllInputsToZero();
            players[1].GetComponent<Player>().SetVelocityToZero();
        }
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().InRoundStarter = true;
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().CanControlCharacters = false;
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().primed = false;
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().SetAllInputsToZero();
        players[0].GetComponent<Player>().SetVelocityToZero();
        Invoke("DelayRoundStartLooper", 0.3f);
    }
    private void DelayRoundStartLooper()
    {
        StartCoroutine(RoundStartLooper());
    }
    public void SetRoundStart(bool val)
    {
        startRound = val;
    }
    private bool startRound = false;
    public bool StartRound {get { return startRound; } }
    private IEnumerator RoundStartLooper()
    {
        yield return new WaitForSeconds(0.01f);
        while (_RoundStart)
        {

            bool ReadyActive = false;
            bool FightActive = false;

            roundStartCounter += 1 * Time.deltaTime;


            TrackPlayer1Rounds();
            TrackPlayer2Rounds();

            if ((int)roundStartCounter >= 0.5f && ReadyActive == false)
            {
                ReadyActive = true;
                ReadyUI.SetActive(true);
            }
            if ((int)roundStartCounter >= 1.5f && FightActive == false)
            {
                FightActive = true;
                ReadyUI.SetActive(false);
                FightUI.SetActive(true);
            }
            if ((int)roundStartCounter >= 2.5f && FightActive == true && ReadyActive == true)
            {
                FightUI.SetActive(false);
                //InRoundStarter = false;
                ResetPlayersInputs();
                if (players.Count > 1)
                {
                    bindToPlayer.players[1].GetComponent<PlayerInputHandler>().InRoundStarter = false;
                    bindToPlayer.players[1].gameObject.SetActive(true);
                }
                bindToPlayer.players[0].GetComponent<PlayerInputHandler>().InRoundStarter = false;
                bindToPlayer.players[0].gameObject.SetActive(true);
                _RoundStart = false;
            }

            yield return null;
        }
    }

    public void ResetPlayersInputs()
    {
        if (players.Count > 1)
        {
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().CanControlCharacters = true;
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().primed = true;
            bindToPlayer.players[1].GetComponent<PlayerInputHandler>().SetAllInputsToZero();
        }
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().CanControlCharacters = true;
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().primed = true;
        bindToPlayer.players[0].GetComponent<PlayerInputHandler>().SetAllInputsToZero();
    }
    private void TrackPlayerRounds()
    {
        TrackPlayer1Rounds();
        TrackPlayer2Rounds();
    }
    public int LevelIDNumber;
    private void TrackPlayer1Rounds()
    {
        switch (player1Rounds)
        {
            case 1:

                RoundsAnim.Play("P1Round1");
                player1Round1.SetActive(true);
                break;
            case 2:

                RoundsAnim.Play("P1Round2");
                player1Round2.SetActive(true);
                break;
            case 3:

                RoundsAnim.Play("P1Round3");
                player1Round3.SetActive(true);
                break;
        }
    }
    private void TrackPlayer2Rounds()
    {
        switch (player2Rounds)
        {
            case 1:

                RoundsAnim.Play("P2Round1");
                player2Round1.SetActive(true);
                break;
            case 2:

                RoundsAnim.Play("P2Round2");
                player2Round2.SetActive(true);
                break;
            case 3:

                RoundsAnim.Play("P2Round3");
                player2Round3.SetActive(true);
                break;
        }
    
    }
    private void TrackPlayer1()
    {
        if(players != null)
        {
            if (players[0].gameObject.transform.position.y <= belowBounds)
            {
                players[0].GetComponent<Player>().ResetGuage();
                player2Rounds++;
                //TrackPlayer2Rounds();
                ResetPlayers();
                _RoundStart = true;
                RoundStartCountDown();
            }
        }
    }
    private void TrackPlayer2()
    {
        if(players != null)
        {
            if (players[1].gameObject.transform.position.y <= belowBounds)
            {
                players[1].GetComponent<Player>().ResetGuage();
                player1Rounds++;
                //TrackPlayer1Rounds();
                ResetPlayers();
                _RoundStart = true;
                RoundStartCountDown();
            }
        }
    }
    private void ResetPlayers()
    {
        //timerScript.ResetTimer();
        players[0].gameObject.GetComponent<Player>().SetJumpIndexTo1();
        players[0].gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
        players[0].transform.position = player1Spawn.transform.position;
        players[0].transform.rotation = player1Spawn.transform.rotation;
        if (players.Count > 1)
        {
            players[1].gameObject.GetComponent<Player>().SetJumpIndexTo1();
            players[1].gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
            players[1].gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
            players[1].gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
            players[1].transform.position = player2Spawn.transform.position;
            players[1].transform.rotation = player2Spawn.transform.rotation;
        }
    }
    public void SetRoundsToZero()
    {
        player1Rounds = 0;
        player2Rounds = 0;
    }

}
