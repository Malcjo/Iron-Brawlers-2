using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA.Input;

public class BindToPlayer : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    [SerializeField] private PlayerJoinHandler join = null;
    [SerializeField] private bool anyKeyToContinue;
    public bool AnyKeyToContinue { get{ return anyKeyToContinue; }set{ anyKeyToContinue = value; } }
    [SerializeField] private Animator loadingScreenAnim;
    [SerializeField] private GameObject loadingScreenGroup, loadingText, pressAnyButtonText, loadScreenLight, characterSelectLight, dot1, dot2, dot3, keyboardImgGroup, controllerImgGrp, runningSol;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;

    [SerializeField] private LoadLevel loadLevelScript;
    [SerializeField] PlayerInputManager inputManager;
    public GameObject events;

    public bool GoblinBeenPicked;
    public bool SolBeenPicked;

    Scene currentScene;
    Scene menuScene;
    Scene CharacterSelect;
    
    

    public int playerIndex;
    public bool Solo = false;
    private void OnEnable()
    {
        GameManager.instance.ConnectBindToPlayer(this);
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        currentScene = SceneManager.GetActiveScene();

        if (currentScene != SceneManager.GetActiveScene())
        {
            currentScene = SceneManager.GetActiveScene();
        }

        if (currentScene == CharacterSelect)
        {
            join.SetPlayerBind(this);
            foreach (GameObject obj in players)
            {
                Destroy(obj);
            }
            Debug.Log("clear");
            players.Clear();
        }
    }

    public void Start()
    {
        GoblinBeenPicked = false;
        SolBeenPicked = false;
        startGame = false;
        delayCounter = 0;
        inputManager.DisableJoining();

    }
    private float delayCounter;
    private void FixedUpdate()
    {
        DelayAbilityForPlayersToJoin();
    }
    private void DelayAbilityForPlayersToJoin()
    {
        if (delayCounter > 2)
        {
            inputManager.EnableJoining();
            return;
        }
        else if(delayCounter < 2)
        {
            delayCounter += 1 * Time.deltaTime;
        }

    }
    public void SetLoadLevelToContinue()
    {
        loadLevelScript._Continue = true;
    }
    public void ResetBindToPlayer()
    {
        loadLevelScript.TurnCharacterLightOn();
        GameManager.instance.ResetPlayersReady();
        playerIndex = 0;
        join = FindObjectOfType<PlayerJoinHandler>();
        players.Clear();
        //GameManager.instance.levelSelect = false;
        
    }
    [SerializeField] private bool player1Ready;
    [SerializeField] private bool player2Ready;
    public bool Player1Ready { get{ return player1Ready; } }
    public bool Player2Ready { get{ return player2Ready; } }
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
    [SerializeField] private bool startGame;
    public bool StartGame { get{ return startGame; } set{ startGame = value; } }
    private void Update()   
    {
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    LevelSelectNumber = 1;
        //    SetLevelSelectedNumber(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    LevelSelectNumber = 2;
        //    SetLevelSelectedNumber(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    GameManager.instance.StartGame = true;
        //}
        if(players.Count <= 1)
        {
            Solo = true;
        }
        else if (players.Count > 1)
        {
            Solo = false;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            if (!Solo)
            {
                if (player1Ready == true && player2Ready == true)
                {
                    TransitionToLevelSelect();
                    //GameManager.instance.ChooseLevel = true;
                    //GameManager.instance.levelSelect = true;
                    if (startGame == true)
                    {
                        startGame = false;
                        loadLevelScript.StartGame();
                    }
                }
            }
            else if (Solo)
            {
                if(player1Ready == true)
                {
                    inputManager.DisableJoining();
                    //GameManager.instance.StopPlayersFromJoining();
                    TransitionToLevelSelect();
                    //GameManager.instance.ChooseLevel = true;
                    //GameManager.instance.levelSelect = true;
                    if (startGame == true)
                    {
                        startGame = false;
                        loadLevelScript.StartGame();
                    }
                }
            }

        }
    }
    [SerializeField] private CharacterSelect characterSelect;
    
    IEnumerator delayRoundStart()
    {
        yield return new WaitForSeconds(0.1f);

    }
    public void JoinGame(PlayerInput input)
    {
        if(this.gameObject.tag == "Joining")
        {
            if(input.playerIndex == 1 - 1)
            {
                characterSelect.SetUpPlayer1();
                //GameManager.instance.player1pressAToJoinUI.SetActive(false);
                //GameManager.instance.player1Character1PortraitPuck.SetActive(true);
                //GameManager.instance.player1Character1Background.SetActive(true);
            }
            else if (input.playerIndex == 2 - 1)
            {
                characterSelect.SetUpPlayer2();
                //GameManager.instance.player2pressAToJoinUI.SetActive(false);
                //GameManager.instance.player2Character1PortraitPuck.SetActive(true);
                //GameManager.instance.player2Character1Background.SetActive(true);
            }
            players.Add(input.gameObject);
            input.gameObject.GetComponent<PlayerInputHandler>().SetInput(input);
            playerIndex = players.Count;
            input.gameObject.GetComponent<PlayerInputHandler>().SetPlayerNumber(characterSelect.playerInputManager);
            input.gameObject.GetComponent<PlayerInputHandler>().SetUpBind(this);
            input.gameObject.GetComponent<PlayerInputHandler>().SetUpCharacterSelect(characterSelect);
            input.gameObject.GetComponent<PlayerInputHandler>().PlayerIndex = playerIndex;
            //input.gameObject.GetComponent<PlayerInputHandler>().canAct = true;

            DontDestroyOnLoad(input.gameObject);
        }
    }

    public void ChangeCharacterModelIfSameIsChosen(int index, GameObject character, float currentCharacter)
    {
        if (index == 1)
        {
            players[0].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
        else if (index == 2)
        {
            players[1].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
    }

    public void TurnOffCanAct()
    {
        if(players.Count > 1)
        {
            players[1].gameObject.GetComponent<PlayerInputHandler>().canAct = false;
        }
        players[0].gameObject.GetComponent<PlayerInputHandler>().canAct = false;
    }
    //public void ReadyPlayer()
    //{
    //    GameManager.instance.ReadyPlayer();
    //}


    public void TransitionToLevelSelect()
    {
        _TransitionToLevelSelect();
    }
    private bool LevelTransitioned = false;
    private void _TransitionToLevelSelect()
    {
        Animator _display1 = characterSelect.Display1.GetComponent<Animator>();
        Animator _display2 = characterSelect.Display2.GetComponent<Animator>();
        Animator _display3 = characterSelect.Display3.GetComponent<Animator>();
        if (LevelTransitioned == false)
        {
            LevelTransitioned = true;
            StartCoroutine(PlayerDisplay1Animation(_display1));
            StartCoroutine(PlayerDisplay2Animation(_display2));
            StartCoroutine(PlayerDisplay3Animation(_display3));

            characterSelect.ChooseYourCharacterText.SetActive(false);
            characterSelect.ChooseYourStageText.SetActive(true);
            characterSelect.LevelDisplay1Obj.SetActive(true);
            characterSelect.LevelDisplay2Obj.SetActive(false);

            characterSelect.level1DisplayImage.SetActive(false);
            characterSelect.level1HighlightImage.SetActive(true);
            characterSelect.level2DisplayImage.SetActive(true);
            characterSelect.level2HighlightImage.SetActive(false);
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

    public void SetLevelNumber(int var)
    {
        loadLevelScript.SetLevelSelectedNumber(var);
    }
}
