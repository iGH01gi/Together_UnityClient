using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ClientTimer : MonoBehaviour
{
    
    private static float hardSnapMargin = 0.1f; //HardSanp 기준 시간 / Update 단위
    protected static float clientTimerValue = 0; //현재 타이머 값
    public static TMP_Text timerUI;
    public static TimerCountdownActivator timerCountdownActivator;

    private void Awake()
    {
        timerCountdownActivator = transform.GetComponent<TimerCountdownActivator>();
        timerCountdownActivator.enabled = false;
        timerUI = transform.GetComponent<TMP_Text>();
    }
    

    //타이머 UI 업데이트
    protected void UpdateTimerUI()
    {
        timerUI.text = Mathf.CeilToInt(clientTimerValue).ToString();
    }
    
    
    ////서버 관련 함수/////
    
    // 라운드 시작 패킷 받을 시, 라운드 시간을 변수로 콜
    public static void Init(int newTimeToSet)
    {
        clientTimerValue = newTimeToSet;
        timerCountdownActivator.enabled = true;
    }

    //서버 시간과 비교. hardsnap 마진보다 차이 날 시 hardsnap
    public void CompareTimerValue(float serverTimerValue)
    {
        if (Math.Abs(serverTimerValue - clientTimerValue) >= hardSnapMargin)
        {
            clientTimerValue = serverTimerValue;
            UpdateTimerUI();
        }
    }
    
    //서버에서 라운드 종료 패킷 받을 시 타이머 처리
    public void EndTimer()
    {
        timerCountdownActivator.enabled = false;
        clientTimerValue = 0;
    }
}
