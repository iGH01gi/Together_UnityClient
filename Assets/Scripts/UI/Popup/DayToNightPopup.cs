using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayToNightPopup : UI_popup
{
    Animator _backgroundAnim;
    TMP_Text _text;
    String _survivorText;

    private void Start()
    {
        _backgroundAnim = transform.GetComponent<Animator>();
        _text = transform.Find("SurvivorText").GetComponent<TMP_Text>();
        _survivorText = _text.text;
        _text.text = "";
        if (Managers.Player.IsMyDediPlayerKiller())
        {
            //킬러일 경우
            
        }
        else
        {
            StartCoroutine(ShowText());
        }
    }

    /// <summary>
    /// OpenEyes 애니메이션 실행. 애니메이션 실행 시 자동으로 DayToNightPopup이 닫힘.
    /// (BlinkOpen 애니메이션 이벤트로 Closepopup() 호출)
    /// </summary>
    public void OpenEyes()
    {
        _backgroundAnim.SetTrigger("OpenEyes");
    }

    public IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i< _survivorText.Length; i++)
        {
            _text.text += _survivorText[i];
            Managers.Sound.Play("Typewriter");
            yield return new WaitForSeconds(0.15f);
        }
        Managers.Sound.Play("SurvivorBoom");
    }
}
