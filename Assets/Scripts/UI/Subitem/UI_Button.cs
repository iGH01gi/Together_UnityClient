using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : UI_subitem
{
    public void PlayButtonClick()
    {
        Managers.Sound.Play("Paper");
    }

    public void PlayButtonHover()
    {
        Managers.Sound.Play("ButtonClick");
    }
    
    public void SetOnClick(Action func)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { func(); });
    }

    public void Activation(bool activate)
    {
        gameObject.GetComponent<Button>().interactable = activate;
    }
    
    public void RemoveAllOnClick()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}