using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCountdownActivator : MonoBehaviour
{
    //client 자체 카운트 다운
     void Update()
    {
        float old = Managers.Game._clientTimer._clientTimerValue;
        float cur = Mathf.Max(0f, old - Time.deltaTime);
        Managers.Game._clientTimer._clientTimerValue = cur;
        Managers.UI.GetComponentInSceneUI<InGameUI>().ChangeCurrentTimerValue(cur);
    }
}
