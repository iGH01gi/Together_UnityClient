using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NightIsOverPopup : UI_popup
{
    private int _playerID;
    private string _deadPlayerPrefabPath = "DeadPlayerGO";
    Animator _backgroundAnim;
    private Camera _camera;
    private TMP_Text _mySacrificeText;
    private TMP_Text _otherSacrificeText;
    private TMP_Text _playerName;

    private string _myText;
    private string _otherText;
    void Awake()
    {
        Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceCamera); //캔버스 렌더모드 변경
        _mySacrificeText = transform.Find("MySacrificeText").GetComponent<TMP_Text>();
        Debug.Log(_mySacrificeText == null);
        _otherSacrificeText = transform.Find("OtherSacrificeText").GetComponent<TMP_Text>();
        _playerName = transform.Find("PlayerName").GetComponent<TMP_Text>();

        Debug.Log("NightIsOverPopup Start");
        
        _myText = _mySacrificeText.text;
        _otherText = _otherSacrificeText.text;
        
        _playerName.text = "";
        _mySacrificeText.text = "";
        _otherSacrificeText.text = "";

        _backgroundAnim = transform.GetComponent<Animator>();
        _camera = GameObject.Find(String.Concat(_deadPlayerPrefabPath,"/RenderCamera")).transform.GetComponent<Camera>();
        _camera.enabled = false;
    }

    public void Init(int playerId)
    {
        _playerID = playerId;
        //죽을 플레이어를 찾아서 이동시키기
        GameObject currentGO = GameObject.Find(String.Concat(_deadPlayerPrefabPath, "/PlayerPrefab"));
        GameObject newGO = (playerId == Managers.Player._myDediPlayerId)?Managers.Player._myDediPlayer: Managers.Player._otherDediPlayers[playerId];
        newGO.GetComponent<CharacterController>().enabled = false;
        Util.DestroyAllChildren(currentGO.transform);
        newGO.transform.parent = currentGO.transform;
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localRotation = Quaternion.identity;
        
        if (playerId == Managers.Player.GetKillerId())
        {
            _mySacrificeText.text = _myText;
        }
        else
        {
            _otherSacrificeText.text = _otherText;
            _playerName.text = Managers.Player._otherDediPlayers[playerId].GetComponent<OtherDediPlayer>().Name;
        }

        Managers.Player.GetAnimator(playerId).SetTriggerByString("Die");
        Managers.Sound.Play("SurvivorBoom");
    }

    public void StartDay()
    {
        Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceOverlay);
        if (Managers.Player._myDediPlayer == null)
        {
            Managers.UI.LoadScenePanel(Define.SceneUIType.PlayerDeadUI);
        }
        Managers.Player.ActivateInput();
        Managers.Player.DeletePlayerObject(_playerID);
        ClosePopup();
    }
}