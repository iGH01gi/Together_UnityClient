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
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true);
        StartCoroutine(InitTimer());
    }

    IEnumerator InitTimer(int time = 180)
    {
        yield return new WaitForSeconds(3);
        transform.Find("TimerText").GetComponent<ClientTimer>().Init(time);
        FindObjectOfType<PlayerInput>().enabled = true;
    }
}
