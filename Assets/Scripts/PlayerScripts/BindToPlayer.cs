using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BindToPlayer : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    [SerializeField] private PlayerJoinHandler join = null;

    [SerializeField] private Animator loadingScreenAnim;
    [SerializeField] private GameObject loadingScreenGroup, loadingText, pressAnyButtonText, loadScreenLight, characterSelectLight, dot1, dot2, dot3, keyboardImgGroup, controllerImgGrp, runningSol;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;

    [SerializeField] private LoadLevel loadLevelScript;

    public GameObject events = null;
    
    Scene currentScene;
    Scene menuScene;

    public int playerIndex;
    public bool Solo = false;
    private void OnEnable()
    {
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        currentScene = SceneManager.GetActiveScene();

        if (currentScene != SceneManager.GetActiveScene())
        {
            currentScene = SceneManager.GetActiveScene();
        }

        if (currentScene == menuScene)
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
        GameManager.instance.levelSelect = false;
        
    }
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
        if (SceneManager.GetActiveScene() == menuScene)
        {
            if (!Solo)
            {
                if (GameManager.instance.GetPlayer1Ready() == true && GameManager.instance.GetPlayer2Ready() == true)
                {
                    GameManager.instance.TransitionToLevelSelect();
                    //GameManager.instance.ChooseLevel = true;
                    GameManager.instance.levelSelect = true;
                    if (GameManager.instance.StartGame == true)
                    {
                        loadLevelScript.StartGame();
                    }
                }
            }
            else if (Solo)
            {
                if(GameManager.instance.GetPlayer1Ready() == true)
                {
                    GameManager.instance.StopPlayersFromJoining();
                    GameManager.instance.TransitionToLevelSelect();
                    //GameManager.instance.ChooseLevel = true;
                    GameManager.instance.levelSelect = true;
                    if (GameManager.instance.StartGame == true)
                    {
                        loadLevelScript.StartGame();
                    }
                }
            }

        }
    }
    
    
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
                GameManager.instance.player1pressAToJoinUI.SetActive(false);
                GameManager.instance.player1Character1PortraitPuck.SetActive(true);
                GameManager.instance.player1Character1Background.SetActive(true);
            }
            else if (input.playerIndex == 2 - 1)
            {
                GameManager.instance.player2pressAToJoinUI.SetActive(false);
                GameManager.instance.player2Character1PortraitPuck.SetActive(true);
                GameManager.instance.player2Character1Background.SetActive(true);
            }
            players.Add(input.gameObject);
            input.gameObject.GetComponent<PlayerInputHandler>().SetInput(input);
            playerIndex = players.Count;
            input.gameObject.GetComponent<PlayerInputHandler>().SetPlayerNumber(GameManager.instance.inputManager);
            input.gameObject.GetComponent<PlayerInputHandler>().PlayerIndex = playerIndex;
            //input.gameObject.GetComponent<PlayerInputHandler>().canAct = true;

            DontDestroyOnLoad(input.gameObject);
        }
    }
    //public void ReadyPlayer()
    //{
    //    GameManager.instance.ReadyPlayer();
    //}
}
