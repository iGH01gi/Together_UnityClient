using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Toggle : UI_subitem
{
    public void SetToggleState(bool state)
    {
        gameObject.GetComponent<Toggle>().isOn = state;
    }

    public bool GetToggleState()
    {
        return gameObject.GetComponent<Toggle>().isOn;
    }
    
    public void SetOnClick(Action func)
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { func(); });
    }
}
