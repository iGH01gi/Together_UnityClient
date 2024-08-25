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
        Managers.Network._dedicatedServerSession.Disconnect();
        Managers.Scene.LoadScene(Define.Scene.Lobby);
        Managers.UI.LoadScenePanel(Define.SceneUIType.LobbyUI);
    }
    
    private void ObserveButton()
    {
        float currentTime = Managers.Game._clientTimer._clientTimerValue;
        Managers.UI.LoadScenePanel(Define.SceneUIType.ObserveUI);
        Managers.UI.GetComponentInSceneUI<ObserveUI>().InitObserveTimer(currentTime);
    }
}
