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
    [SerializeField] private GameObject loadingScreenGroup, loadingText, pressAnyButtonText, loadScreenLight, characterSelectLight, dot1, dot2, dot3;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;

    public GameObject events = null;
    
    Scene currentScene;
    Scene menuScene;

    public int playerIndex;

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
            players.Clear();
        }

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
        if (SceneManager.GetActiveScene() == menuScene)
        {
            if (GameManager.instance.GetPlayer1Ready() == true && GameManager.instance.GetPlayer2Ready() == true)
            {
                GameManager.instance.TransitionToLevelSelect();
                //GameManager.instance.ChooseLevel = true;
                GameManager.instance.levelSelect = true;
                if (GameManager.instance.StartGame == true)
                {
                    StartGame();
                }
            }
        }
    }
    public void SetLevelSelectedNumber(int var)
    {

        LevelSelectNumber = var;
    }
    [Range(1,2)]
    [SerializeField] private int LevelSelectNumber = 1;
    private void StartGame()
    {

        //loadingScreenAnim.Play("Fade In and Out");
        GameManager.instance.StartGame = false;
        StartCoroutine(DelayStartGame());
    }
    IEnumerator DelayStartGame()
    {
        {
            loadingScreenGroup.SetActive(true);
            yield return StartCoroutine(FadeLoadingScreen(1, 1));

            AsyncOperation operation = SceneManager.LoadSceneAsync(LevelSelectNumber);
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    loadingText.SetActive(false);
                    dot1.SetActive(false);
                    dot2.SetActive(false);
                    dot3.SetActive(false);
                    pressAnyButtonText.SetActive(true);
                    if (Input.anyKey)
                    {
                        loadScreenLight.SetActive(false);
                        characterSelectLight.SetActive(false);
                        Debug.Log("button pressed"); 
                        operation.allowSceneActivation = true;
                    }
                }
                Debug.Log(operation.progress);
                yield return null;
            }
            

            yield return StartCoroutine(FadeLoadingScreen(0, 1));
            //yield return new WaitForSeconds(1f);
            GameManager.instance.DisableMenuCanvas();
            GameManager.instance.DisableJoining();
            GameManager.instance.ResetPlayersReady();
            //SceneManager.LoadScene(LevelSelectNumber);
            GameManager.instance.ConnectToGameManager(1);
            GameManager.instance.inGame = true;
            GameManager.instance.RoundStart = true;
            //loadingScreenAnim.Play("Fade In and Out");
            GameManager.instance.SetRoundStart(true);
            GameManager.instance.RoundStartCountDown();
            GameManager.instance.SetRoundStart(false);
            //StartCoroutine(delayRoundStart());
            loadingScreenGroup.SetActive(false);
        }

        IEnumerator FadeLoadingScreen(float targetValue, float duration)
        {
            float startValue = loadingScreenCanvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                loadingScreenCanvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            loadingScreenCanvasGroup.alpha = targetValue;
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
