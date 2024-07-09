using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCountdownActivator : ClientTimer
{
    //client 자체 카운트 다운

    private void Awake()
    {
        _timerCountdownActivator = this;
    }
    
    void Update()
    {
        float old = _clientTimerValue;
        _clientTimerValue = Mathf.Max(0f, _clientTimerValue - Time.deltaTime);
        if (Mathf.CeilToInt(old)>Mathf.CeilToInt(_clientTimerValue))
        {
            UpdateTimerUI();
        }
    }
}
