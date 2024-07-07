using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUI : UI_scene
{
    private void Start()
    {
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true,false); //3초 카운트 다운
    }
}
