using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WairForSecondsPopup : UI_popup
{
    IEnumerator Start()
    {
        transform.GetComponent<Image>().color= new Color(4F,4F,4F,255);
        for (int i = 3; i > 0; i--)
        {
            transform.Find("Seconds").GetComponent<TMP_Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        transform.GetComponent<Image>().color= new Color(0,0,0,0);
        transform.Find("Seconds").GetComponent<TMP_Text>().text = "START!";
        Managers.Sound.Play("Effects/Start!");
        ClosePopup();
    }
}
