using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : UI_scene
{
    private void Start()
    {
        ClientTimer.Init(180);
    }
}
