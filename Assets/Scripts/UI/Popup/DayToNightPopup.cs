using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using TMPro;
using UnityEngine;

public class DayToNightPopup : UI_popup
{
    private string _killerPrefabPath = "KillerChangeGO";
    Animator _backgroundAnim;
    TMP_Text _text;
    String _survivorText;
    private Camera _camera;
    private bool _canOpenEyes;
    private TMP_Text _killerText;
    private TMP_Text _killerDescriptionText;

    private void Start()
    {
        _canOpenEyes = false;
        _backgroundAnim = transform.GetComponent<Animator>();
        _text = transform.Find("SurvivorText").GetComponent<TMP_Text>();
        _survivorText = _text.text;
        _text.text = "";
        _camera = GameObject.Find(String.Concat(_killerPrefabPath,"/RenderCamera")).transform.GetComponent<Camera>();
        _camera.enabled = false;
        _killerText = transform.Find("KillerText").GetComponent<TMP_Text>();
        _killerDescriptionText = transform.Find("KillerDescription").GetComponent<TMP_Text>();
        if (Managers.Player.IsMyDediPlayerKiller())
        {
            StartCoroutine(ShowKiller());
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

    public void StartNight()
    {
        if (!Managers.Player.IsMyDediPlayerKiller())
        {
            OpenEyes();
        }
        else
        {
            _canOpenEyes = true;
        }
    }
    
    public void OpenEyes()
    {
        _survivorText = String.Empty;
        Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceOverlay); 
        _killerText.text = String.Empty;
        _killerDescriptionText.text = String.Empty;
        _camera.enabled = false;
        _backgroundAnim.SetTrigger("OpenEyes");
        
        Managers.Player.ActivateInput();
    }

    public IEnumerator ShowText()
    {
        yield return new WaitForSeconds(0.9f);
        for (int i = 0; i< _survivorText.Length; i++)
        {
            _text.text += _survivorText[i];
            Managers.Sound.Play("Typewriter");
            yield return new WaitForSeconds(0.15f);
        }
        Managers.Sound.Play("SurvivorBoom");
    }

    public IEnumerator ShowKiller()
    {
        yield return new WaitForSeconds(0.9f); 
        _camera.enabled = true;
        //Managers.Resource.Destroy(transform.Find(String.Concat(_killerPrefabPath,"/KillerPrefab")).gameObject);
       // Managers.Resource.Instantiate(String.Concat(_killerPrefabPath, Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerEngName),transform.Find(_killerPrefabPath));
       Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceCamera); 
       
       Transform positions = _camera.transform.parent.Find("Positions");
       for (int i = 0; i < (positions.childCount) ; i++)
        {
            _camera.GetComponent<Transform>().position = positions.GetChild(i).GetComponent<Transform>().position;
            _camera.GetComponent<Transform>().rotation = positions.GetChild(i).GetComponent<Transform>().rotation;
            Managers.Sound.Play("BoomTransition");
            yield return new WaitForSeconds(0.9f);
        }
        if (CultureInfo.CurrentCulture.Name == "ko-KR")
        {
            _killerText.text = Managers.Killer._killers[Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerType].KoreanName;
            _killerDescriptionText.text = Managers.Killer._killers[Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerType].KoreanDescription;
        }
        else
        {
            _killerText.text = Managers.Killer._killers[Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerType].EnglishName;
            _killerDescriptionText.text = Managers.Killer._killers[Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerType].EnglishDescription;
        }
        Managers.Sound.Play("SurvivorBoom");
        yield return new WaitForSeconds(3f);
        if (_canOpenEyes)
        {
            OpenEyes();
        }
        else
        {
            while (!_canOpenEyes)
            {
                yield return new WaitForSeconds(0.1f);
            }
            OpenEyes();
        }
    }
}
