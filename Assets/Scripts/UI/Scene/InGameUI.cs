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
    public GameObject _gauge;
    private void Start()
    {
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true,false); //3초 카운트 다운
        _timerDay = transform.Find("TimerDay").gameObject;
        _timerNight = transform.Find("TimerNight").gameObject;
        _gauge = transform.Find("Gauge").gameObject;
        _timerNight.SetActive(false);
        _gauge.SetActive(false);
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
    
    public void SetMaxTimerValue(float value)
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
    
    public void SetMaxGauge(float max)
    {
        _gauge.GetComponent<ProgressBarPattern>().MaxValue =max;
        SetCurrentGauge(max);
    }
    
    public void SetCurrentGauge(float cur)
    {
        _gauge.GetComponent<ProgressBarPattern>().CurrentValue = (cur);
    }

    public void ToggleGaugePrefab(bool setActive)
    {
        _gauge.SetActive(setActive);
    }
    
}
