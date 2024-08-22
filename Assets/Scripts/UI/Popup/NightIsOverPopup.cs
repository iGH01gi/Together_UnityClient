using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightIsOverPopup : UI_popup
{
    Animator _backgroundAnim;
    void Start()
    {
        _backgroundAnim = transform.GetComponent<Animator>();
    }

    public void StartDay()
    {
        ClosePopup();
    }
}
