using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GameAnalyticsSDK;
//using GameAnalyticsSDK.Setup;
//using GameAnalyticsSDK.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTimer;
    [SerializeField] private float minTimer;
    [SerializeField] private float currentTime;
    private TMP_Text timerUI;
    private float timer;
    public bool pause;
    [SerializeField]
    private float startTimerValue;
    //public Score score;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetMaxTimer(maxTimer);
        ResetTimer();
        pause = false;
        timerUI = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Convert.ToInt32(timer);
        timerUI.text = "" + currentTime;

        if(pause == false)
        {
            TimerCountDown();
        }
        if (timer < (int)minTimer)
        {
            ResetTimer();
            GameManager.instance.TimerRunOut();
        }
    }
    public void ResetTimer()
    {
        timer = maxTimer;
    }
    void TimerCountDown()
    {
        timer -= 1 * Time.deltaTime;
    }
}
