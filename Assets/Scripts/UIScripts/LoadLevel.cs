using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Animator loadingScreenAnim;
    [SerializeField] private GameObject loadingScreenGroup, loadingText, pressAnyButtonText, loadScreenLight, characterSelectLight, dot1, dot2, dot3, keyboardImgGroup, controllerImgGrp, runningSol;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;

    [Range(1, 2)]
    [SerializeField] private int LevelSelectNumber = 1;

    public void StartGame()
    {


        //loadingScreenAnim.Play("Fade In and Out");
        GameManager.instance.StartGame = false;
        StartCoroutine(DelayStartGame());
    }

    public void SetLevelSelectedNumber(int var)
    {

        LevelSelectNumber = var;
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
}
