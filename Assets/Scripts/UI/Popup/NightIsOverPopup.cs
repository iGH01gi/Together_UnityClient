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
        _mySacrificeText = transform.Find("MySacrificeText").GetComponent<TMP_Text>();
        _otherSacrificeText = transform.Find("OtherSacrificeText").GetComponent<TMP_Text>();
        _playerName = transform.Find("PlayerName").GetComponent<TMP_Text>();
        
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
        _camera.enabled = true;
        Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceCamera); //캔버스 렌더모드 변경
        _playerID = playerId;
        //죽을 플레이어를 찾아서 이동시키기
        GameObject currentGO = GameObject.Find(String.Concat(_deadPlayerPrefabPath, "/PlayerPrefab"));
        Util.DestroyAllChildren(currentGO.transform);
        GameObject newGO;
        if (playerId == Managers.Player._myDediPlayerId)
        {
            if (!Managers.Player.IsMyDediPlayerKiller())
            {
                newGO = Managers.Resource.Instantiate("Player/OtherPlayer(Model)", currentGO.transform);
            }
            else
            {
                newGO = Instantiate(
                    Managers.Killer._otherPlayerKillerPrefabs[
                        Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>()._killerType]);
            }
        }
        else
        {
            newGO = Managers.Player._otherDediPlayers[playerId];
            newGO.transform.GetComponent<CharacterController>().enabled = false;
        }
        
        newGO.transform.SetParent(currentGO.transform);
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localRotation = Quaternion.identity;
        
        if (playerId == Managers.Player._myDediPlayerId)
        {
            newGO.GetComponentInChildren<PlayerAnimController>().SetTriggerByString("Die");
            _mySacrificeText.text = _myText;
        }
        else
        {
            Managers.Player.GetAnimator(playerId).SetTriggerByString("Die");
            _otherSacrificeText.text = _otherText;
            _playerName.text = Managers.Player._otherDediPlayers[playerId].GetComponent<OtherDediPlayer>().Name;
        }
        Managers.Sound.Play("SurvivorBoom");
    }

    public void StartDay()
    {
        _camera.enabled = false;
        Managers.UI.ChangeCanvasRenderMode(RenderMode.ScreenSpaceOverlay);
        if (Managers.Player._myDediPlayer == null)
        {
            Managers.UI.LoadScenePanel(Define.SceneUIType.PlayerDeadUI);
        }
        Managers.Player.DeletePlayerObject(_playerID);
        ClosePopup();
    }
}