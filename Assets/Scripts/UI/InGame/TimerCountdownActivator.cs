using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCountdownActivator : ClientTimer
{
    //client 자체 카운트 다운
    void Update()
    {
        float old = clientTimerValue;
        clientTimerValue = Mathf.Max(0f, clientTimerValue - Time.deltaTime);
        if (Mathf.CeilToInt(old)>Mathf.CeilToInt(clientTimerValue))
        {
            UpdateTimerUI();
        }
    }
}
