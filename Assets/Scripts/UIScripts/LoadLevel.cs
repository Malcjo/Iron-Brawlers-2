using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using JetBrains.Annotations;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Animator loadingScreenAnim;

    [SerializeField] private GameObject loadingScreenGroup, loadingText, pressAnyButtonText, loadScreenLight, characterSelectLight, dot1, dot2, dot3, keyboardImgGroup, controllerImgGrp, runningSol;
    [SerializeField] private GameObject player1Round1, player1Round2, player1Round3, player2Round1, player2Round2, player2Round3, player1Wins, player2Wins; 
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Animator musicFadeAnim;
    [SerializeField] private BindToPlayer bind;

    [Range(1, 2)]
    [SerializeField] private int LevelSelectNumber = 1;

    public void StartGame()
    {
        //loadingScreenAnim.Play("Fade In and Out");
        GameManager.instance.StartGame = false;
        StartCoroutine(DelayStartGame());
    }
    public void TurnCharacterLightOn()
    {
        characterSelectLight.SetActive(true);
    }
    public void ResetLoadingAssets()
    {
        loadingText.SetActive(true);
        dot1.SetActive(true);
        dot2.SetActive(true);
        dot3.SetActive(true);
        pressAnyButtonText.SetActive(false);

    }
    public void SetLevelSelectedNumber(int var)
    {
        LevelSelectNumber = var;
    }
    public bool _Continue;
    IEnumerator DelayStartGame()
    {
        loadingScreenGroup.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelSelectNumber);
        operation.allowSceneActivation = false;
        characterSelectLight.SetActive(false);
        loadScreenLight.SetActive(true);
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                loadingText.SetActive(false);
                dot1.SetActive(false);
                dot2.SetActive(false);
                dot3.SetActive(false);
                if (musicFadeAnim != null)
                {
                    musicFadeAnim.SetTrigger("FadeOut");
                }

                pressAnyButtonText.SetActive(true);
                GameManager.instance.AnyKeyToContinue = true;
                if (Input.anyKey)
                {
                    _Continue = false;
                    GameManager.instance.AnyKeyToContinue = false;
                    GameManager.instance.ResetPlayersInputs();
                    if(musicFadeAnim != null)
                    {
                        musicFadeAnim.SetTrigger("FadeOut");
                    }


                    GameManager.instance.DisableMenuCanvas();
                    keyboardImgGroup.SetActive(false);
                    controllerImgGrp.SetActive(false);
                    loadScreenLight.SetActive(false);
                    pressAnyButtonText.SetActive(false);
                    characterSelectLight.SetActive(false);
                    runningSol.SetActive(false);
                    Debug.Log("button pressed");
                    operation.allowSceneActivation = true;

                }
            }
            Debug.Log(operation.progress);

            yield return null;
        }
        
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        GameHasLoaded();

        IEnumerator FadeLoadingScreen(float targetValue, float duration)
        {
            runningSol.SetActive(true);
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
    private void MenuHasLoaded()
    {
        GameManager.instance.inGame = false;
        GameManager.instance.RoundStart = false;
        GameManager.instance.ConnectToGameManager(0);
    }
    private void GameHasLoaded()
    {
        GameManager.instance.DisableJoining();
        GameManager.instance.ResetPlayersReady();
        GameManager.instance.ConnectToGameManager(1);
        GameManager.instance.inGame = true;
        GameManager.instance.RoundStart = true;
        GameManager.instance.SetRoundStart(true);
        GameManager.instance.RoundStartCountDown();
        GameManager.instance.SetRoundStart(false);
        //StartCoroutine(delayRoundStart());
        loadingScreenGroup.SetActive(false);
    }

    public void TransitionBackToMainMenu()
    {
        StartCoroutine(LoadScreenToMainMenu());
    }
    public void SetMusicFade()
    {
        musicFadeAnim = GameObject.FindGameObjectWithTag("MusicSource").GetComponent<Animator>();
    }
    IEnumerator LoadScreenToMainMenu()
    {
        Debug.Log("Menu loading");
        loadingScreenGroup.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));

        AsyncOperation operation = SceneManager.LoadSceneAsync("Main Menu");
        while (!operation.isDone)
        {
            Debug.Log("progress: " + operation.progress);
            yield return null;
        }

        IEnumerator FadeLoadingScreen(float targetValue, float duration)
        {
            musicFadeAnim = GameObject.FindGameObjectWithTag("MusicSource").GetComponent<Animator>();
            //musicFadeAnim.SetTrigger("FadeOut");
            //runningSol.SetActive(true);
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
        ResetLoadingAssets();
        GameManager.instance.PausedGame(false);


        //player1
        player1Wins.SetActive(false);
        player1Round1.SetActive(false);
        player1Round2.SetActive(false);
        player1Round3.SetActive(false);

        GameManager.instance.player1pressAToJoinUI.SetActive(true);
        GameManager.instance.player1Character1PortraitPuck.SetActive(false);
        GameManager.instance.player1Character1Background.SetActive(false);
        GameManager.instance.player1Character2Background.SetActive(false);

        GameManager.instance.player1SolPortrait.SetActive(false);
        GameManager.instance.player1SolAltPortrait.SetActive(false);
        GameManager.instance.player1GoblinPortrait.SetActive(false);
        GameManager.instance.player1GoblinAltPortrait.SetActive(false);

        //player 2
        player2Wins.SetActive(false);
        player2Round1.SetActive(false);
        player2Round2.SetActive(false);
        player2Round3.SetActive(false);

        GameManager.instance.player2SolPortrait.SetActive(false);
        GameManager.instance.player2SolAltPortrait.SetActive(false);
        GameManager.instance.player2GoblinPortrait.SetActive(false);
        GameManager.instance.player2GoblinAltPortrait.SetActive(false);

        GameManager.instance.player2pressAToJoinUI.SetActive(true);
        GameManager.instance.player2Character1PortraitPuck.SetActive(false);
        GameManager.instance.player2Character1Background.SetActive(false);
        GameManager.instance.player2Character2Background.SetActive(false);

        GameManager.instance.TurnOffCanAct = false;

        runningSol.SetActive(false);
        GameManager.instance.Character1BeenPicked = false;
        GameManager.instance.Character2BeenPicked = false;
        //player1Loses.SetActive(false);
        //GameManager.instance.bothLose.SetActive(false);
        GameManager.instance.TurnOffCharacterSelectObj();
        GameManager.instance.TurnOnMenuObj();
        GameManager.instance.ResetLevelSelectDisplay();
        GameManager.instance.SetRoundsToZero();
        GameManager.instance.players.Clear();
        GameManager.instance.ChangeSceneIndex(1);
        GameManager.instance.EnableMenuCanvas();
        GameManager.instance.sceneIndex = 1;
        GameManager.instance.inGame = false;
        GameManager.instance.RoundStart = false;
        GameManager.instance.ResetMenu();
        GameManager.instance.ConnectToGameManager(0);
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        //MenuHasLoaded();



    }      
}
