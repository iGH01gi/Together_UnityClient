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
        Managers.UI.LoadPopupPanel<WairForSecondsPopup>(true); //3초 카운트 다운
        StartCoroutine(InitTimer()); // 3초 후에 타이머 시작
    }

    IEnumerator InitTimer(int time = 180)
    {
        yield return new WaitForSeconds(3);
        transform.Find("TimerText").GetComponent<ClientTimer>().Init(time);
        FindObjectOfType<PlayerInput>().ActivateInput();
    }
}
