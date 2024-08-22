using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NightIsOverPopup : UI_popup
{
    private string _deadPlayerPrefabPath = "DeadPlayerGO";
    Animator _backgroundAnim;
    private Camera _camera;
    private TMP_Text _mySacrificeText;
    private TMP_Text _otherSacrificeText;
    private TMP_Text _playerName;
    void Start()
    {
        _mySacrificeText = transform.Find("MySacrificeText").GetComponent<TMP_Text>();
        _otherSacrificeText = transform.Find("OtherSacrificeText").GetComponent<TMP_Text>();
        _playerName = transform.Find("PlayerName").GetComponent<TMP_Text>();

        _mySacrificeText.enabled = false;
        _otherSacrificeText.enabled = false;
        _playerName.text = string.Empty;
        
        _backgroundAnim = transform.GetComponent<Animator>();
        _camera = GameObject.Find(String.Concat(_deadPlayerPrefabPath,"/RenderCamera")).transform.GetComponent<Camera>();
        _camera.enabled = false;
    }

    public void Init(int playerID)
    {
        //죽을 플레이어를 찾아서 이동시키기
        GameObject currentGO = GameObject.Find(String.Concat(_deadPlayerPrefabPath, "/PlayerPrefab"));
        GameObject newGO = (playerID == Managers.Player.GetKillerId())?Managers.Player._myDediPlayer: Managers.Player._otherDediPlayers[playerID];
        Util.DestroyAllChildren(currentGO.transform);
        newGO.transform.parent = currentGO.transform;
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localRotation = Quaternion.identity;
        
        if (playerID == Managers.Player.GetKillerId())
        {
            _mySacrificeText.enabled = true;
        }
        else
        {
            _otherSacrificeText.enabled = true;
            _playerName.text = Managers.Player._otherDediPlayers[playerID].GetComponent<OtherDediPlayer>().Name;
        }

        Managers.Player.GetAnimator(playerID).SetTriggerByString("Die");
        Managers.Sound.Play("SurvivorBoom");
    }

    public void StartDay()
    {
        ClosePopup();
        Managers.Player.ActivateInput();
    }
}