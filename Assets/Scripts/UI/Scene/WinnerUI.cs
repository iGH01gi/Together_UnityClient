using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerUI : UI_scene
{
    private string _winPlayerPrefabPath = "WinnerPlayerGO";
    private TMP_Text _winnerPlayerNameText;
    private GameObject currentGO;
    Camera _camera;

    void Awake()
    {
        Managers.Sound.Play("Win");
        _winnerPlayerNameText = transform.Find("WinnerPlayerName").GetComponent<TMP_Text>();
        currentGO = GameObject.Find(string.Concat(_winPlayerPrefabPath, "/PlayerPrefab"));
        _camera = GameObject.Find(string.Concat(_winPlayerPrefabPath,"/RenderCamera")).transform.GetComponent<Camera>();
        _camera.enabled = true;
        
        // Set this camera to be the main camera
        Camera.main.gameObject.SetActive(false); // Disable the current main camera
        _camera.tag = "MainCamera"; // Set this camera's tag to "MainCamera"
    }
    
    public void SetWinner(int playerId, string playerName)
    {
        GameObject newGO;
        if(playerId == Managers.Player._myDediPlayerId)
        {
            newGO = Managers.Player._myDediPlayer;
        }
        else
        {
            newGO = Managers.Player._otherDediPlayers[playerId];
        }
        newGO.transform.SetParent(currentGO.transform);
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localRotation = Quaternion.identity;
        _winnerPlayerNameText.text = playerName;
    }
}
