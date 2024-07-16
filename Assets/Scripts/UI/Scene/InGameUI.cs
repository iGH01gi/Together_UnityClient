using System;
using System.Collections;
using System.Collections.Generic;
using RainbowArt.CleanFlatUI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUI : UI_scene
{
    public GameObject _timer;
    public GameObject _gauge;
    private void Start()
    {
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true,false); //3초 카운트 다운
        _timer = transform.Find("Timer").gameObject;
        _gauge = transform.Find("Gauge").gameObject;
        _gauge.SetActive(false);
    }
    
    Color _colorTimerDay = new Color(68f,68f,68f);
    Color _colorTimerNight = new Color(210f,4f,45f);

    public void ChangeTimerPrefab()
    {
        if (Managers.Game._isDay)
        {
            _timer.GetComponent<ProgressBar>().ChangeForeground(_colorTimerDay);
        }
        else
        {
            _timer.GetComponent<ProgressBar>().ChangeForeground(_colorTimerNight);
        }
    }

    public void ChangeCurrentTimerValue(float value)
    {
        _timer.GetComponent<ProgressBar>().CurrentValue = Mathf.CeilToInt(value);
    }
    
    public void SetMaxTimerValue(float value)
    {
        _timer.GetComponent<ProgressBar>().MaxValue = Mathf.CeilToInt(value);
    }
    
    public void SetMaxGauge(float max)
    {
        _gauge.GetComponent<ProgressBar>().MaxValue =max;
        SetCurrentGauge();
    }
    
    public void SetCurrentGauge()
    {
        _gauge.GetComponent<ProgressBar>().CurrentValue = Managers.Game._clientGauge.GetMyGauge();
    }

    public void ToggleGaugePrefab(bool setActive)
    {
        _gauge.SetActive(setActive);
    }
}
