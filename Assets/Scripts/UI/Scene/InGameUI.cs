using System;
using System.Collections;
using System.Collections.Generic;
using RainbowArt.CleanFlatUI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUI : UI_scene
{
    public GameObject _timerDay;
    public GameObject _timerNight;
    private void Start()
    {
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true,false); //3초 카운트 다운
        _timerDay = transform.Find("TimerDay").gameObject;
        _timerNight = transform.Find("TimerNight").gameObject;
        _timerNight.SetActive(false);
    }

    public void ChangeTimerPrefab()
    {
        if (Managers.Game._isDay)
        {
            _timerDay.SetActive(true);
            _timerNight.SetActive(false);
        }
        else
        {
            _timerDay.SetActive(false);
            _timerNight.SetActive(true);
        }
    }

    public void ChangeCurrentTimerValue(float value)
    {
        if (_timerDay.activeSelf)
        {
            _timerDay.GetComponent<ProgressBar>().CurrentValue = Mathf.CeilToInt(value);
        }
        else
        {
            _timerNight.GetComponent<ProgressBarPatternCircular>().CurrentValue = Mathf.CeilToInt(value);
        }
    }
    
    public void SetCurrentTimerValue(float value)
    {
        if (_timerDay.activeSelf)
        {
            _timerDay.GetComponent<ProgressBar>().MaxValue = Mathf.CeilToInt(value);
        }
        else
        {
            _timerNight.GetComponent<ProgressBarPatternCircular>().MaxValue = Mathf.CeilToInt(value);
        }
    }
}
