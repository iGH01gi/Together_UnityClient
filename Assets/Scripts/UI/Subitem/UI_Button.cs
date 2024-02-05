using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : UI_subitem
{
    public void SetOnClick(Action func)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { func(); });
    }
}