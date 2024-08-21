using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObserveUI : UI_scene
{
    public GameObject _timerText;
    public GameObject _observingPlayerName;
    public GameObject _leftButton;
    public GameObject _rightButton;
    public GameObject _quitButton;

    private int _currentTime;
    private int _currentlyObservingPlayerID;
    private void Start()
    {
        //추후 접근이 필요한 UI들을 찾아서 저장
        _timerText = transform.Find("TimerText").gameObject;
        _observingPlayerName = transform.Find("ObservingPlayerName").gameObject;
        _leftButton = transform.Find("LeftButton").gameObject;
        _rightButton = transform.Find("RightButton").gameObject;
        _quitButton = transform.Find("QuitButton").gameObject;
        
        //OtherDediPlayer에 첫 플레이어부터 시작하도록 설정
        _currentlyObservingPlayerID = Managers.Player._otherDediPlayers.Keys.FirstOrDefault();
        ObserveChanged();
        
        //버튼 설정
        _quitButton.GetComponent<UI_Button>().SetOnClick(ReturnToLobby);
        _leftButton.GetComponent<UI_Button>().SetOnClick(LeftButtonClicked);
        _rightButton.GetComponent<UI_Button>().SetOnClick(RightButtonClicked);
    }
    
    public void SetTimerText(int time)
    {
        if(time != _currentTime)
        {
            _currentTime = time;
            _timerText.GetComponent<TMP_Text>().text = time.ToString();
        }
    }

    private void ObserveChanged()
    {
        SetObservingPlayerName();
        //TODO: Get the camera to follow the player
    }
    
    private void ReturnToLobby()
    {
        Managers.Scene.LoadScene(Define.Scene.Lobby);
        Managers.UI.LoadScenePanel(Define.SceneUIType.LobbyUI.ToString());
    }
    
    private void SetObservingPlayerName()
    {
        _observingPlayerName.GetComponent<TMP_Text>().text = Managers.Player._otherDediPlayers[_currentlyObservingPlayerID].GetComponent<OtherDediPlayer>().Name;
    }

    //TODO: 만약 관전하던 사람이 사라지면 어캄?
    private void RightButtonClicked()
    {
        var keys = Managers.Player._otherDediPlayers.Keys.ToList();
        int currentIndex = keys.IndexOf(_currentlyObservingPlayerID);
        int nextIndex = (currentIndex + 1) % keys.Count;
        _currentlyObservingPlayerID = keys[nextIndex];
        ObserveChanged();
    }
    
    private void LeftButtonClicked()
    {
        var keys = Managers.Player._otherDediPlayers.Keys.ToList();
        int currentIndex = keys.IndexOf(_currentlyObservingPlayerID);
        int previousIndex = (currentIndex - 1 + keys.Count) % keys.Count;
        _currentlyObservingPlayerID = keys[previousIndex];
        ObserveChanged();
    }
    
    public void CheckIfObservingThisPlayer(int playerID)
    {
        if (_currentlyObservingPlayerID == playerID)
        {
            RightButtonClicked();
        }
    }
}
