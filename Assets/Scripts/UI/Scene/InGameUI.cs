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
    public GameObject _coin;
    public GameObject _coinCollect;
    public GameObject _killerSkill;
    private void Start()
    {
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true,false); //3초 카운트 다운
        _timer = transform.Find("Timer").gameObject;
        _gauge = transform.Find("Gauge").gameObject;
        _coin = transform.Find("Coin").gameObject;
        _coinCollect = transform.Find("CoinCollect").gameObject;
        _killerSkill = transform.Find("KillerSkill").gameObject;
        _gauge.SetActive(false);
        _coinCollect.SetActive(false);
        _killerSkill.SetActive(false);
    }
    
    Color _colorTimerDay = new Color(68f/ 255f,68f/ 255f,68f/ 255f);
    Color _colorTimerNight = new Color(210f/ 255f,4f/ 255f,45f/ 255f);

    public void ChangeDayNightUI()
    {
        bool isDay = Managers.Game._isDay;
        if (Managers.Game._isDay)
        {
            IsNotKiller();
            _timer.GetComponent<ProgressBar>().ChangeForeground(_colorTimerDay);
        }
        else
        {
            _timer.GetComponent<ProgressBar>().ChangeForeground(_colorTimerNight);
            SetCurrentCoin(0);
        }
        _gauge.SetActive(!isDay);
        _coin.SetActive(isDay);
    }

    public void ChangeCurrentTimerValue(float value)
    {
        _timer.GetComponent<ProgressBar>().CurrentValue = value;
    }
    
    public void SetMaxTimerValue(float value)
    {
        _timer.GetComponent<ProgressBar>().MaxValue = value;
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
    
    public void SetCurrentCoin(int coinTotal)
    {
        _coin.transform.Find("CoinText").GetComponent<TMP_Text>().text = coinTotal.ToString();
    }

    public void AddGetCoin(int coinAdded)
    {
        _coinCollect.SetActive(true);
        _coinCollect.AddComponent<AddCoinEffect>().Init(coinAdded);
    }

    public void SetSkillCooltime(float skillCooltime)
    {
        _killerSkill.SetActive(true);
        _killerSkill.GetComponent<ProgressBar>().MaxValue = skillCooltime;
    }

    public void IsNotKiller()
    {
        _killerSkill.SetActive(false);
    }

    public void SetKillerSkillValue(float value)
    {
        _killerSkill.GetComponent<ProgressBar>().CurrentValue = value;
    }
}
