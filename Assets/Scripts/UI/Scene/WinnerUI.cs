using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerUI : UI_scene
{
    private string _winPlayerPrefabPath = "WinnerPlayerGO";
    private TMP_Text _winnerPlayerNameText;
    private TMP_Text _anyKeyText;
    private GameObject _currentGO;
    Camera _camera;
    bool _canMoveOn = false;

    void Awake()
    {
        Managers.Sound.Play("Win");
        _winnerPlayerNameText = transform.Find("WinnerPlayerName").GetComponent<TMP_Text>();
        _anyKeyText = transform.Find("AnyKeyText").GetComponent<TMP_Text>();
        _currentGO = GameObject.Find(string.Concat(_winPlayerPrefabPath, "/PlayerPrefab"));
        _camera = GameObject.Find(string.Concat(_winPlayerPrefabPath,"/RenderCamera")).transform.GetComponent<Camera>();
        _camera.enabled = true;
        _anyKeyText.enabled = false;
        
        // Set this camera to be the main camera
        Camera.main.gameObject.SetActive(false); // Disable the current main camera
        _camera.tag = "MainCamera"; // Set this camera's tag to "MainCamera"
        StartCoroutine(WaitForTwoSeconds());
    }
    
    public void SetWinner(int playerId, string playerName)
    {
        GameObject newGO;
        if(playerId == Managers.Player._myDediPlayerId)
        {
            newGO = Managers.Resource.Instantiate("Player/OtherPlayer(Model)", _currentGO.transform);
            newGO.GetComponentInChildren<MovementInput>().enabled = false;
        }
        else
        {
            newGO = Managers.Player._otherDediPlayers[playerId];
            newGO.transform.GetComponent<CharacterController>().enabled = false;
        }
        newGO.transform.SetParent(_currentGO.transform);
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localRotation = Quaternion.identity;
        _winnerPlayerNameText.text = playerName;
    }

    private void Update()
    {
        if (_canMoveOn)
        {
            if (Input.anyKeyDown)
            {
                Managers.Network._dedicatedServerSession.Disconnect();
                Managers.Scene.LoadScene(Define.Scene.Lobby);
                Managers.UI.LoadScenePanel(Define.SceneUIType.LobbyUI);
            }
        }
    }
    
    IEnumerator WaitForTwoSeconds()
    {
        yield return new WaitForSeconds(2f);
        _canMoveOn = true;
        _anyKeyText.enabled = true;
    }
}
