using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadUI : UI_scene
{
    public GameObject _lobbyButton;
    public GameObject _observeButton;
    
    private enum Buttons
    {
        LobbyButton,
        ObserveButton
    }
    void Start()
    {
        InitButtons<Buttons>(gameObject);
    }
    
    private void LobbyButton()
    {
        Managers.Scene.LoadScene(Define.Scene.Lobby);
        //Managers.UI.LoadScenePanel(Define.SceneUIType.LobbyUI);
    }
    
    private void ObserveButton()
    {
        Managers.UI.LoadScenePanel(Define.SceneUIType.ObserveUI);
    }
}
