using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : UI_scene
{
    private UIRoomListPacket _uiRoomListPacket;
    private enum Buttons
    {
        MainMenuButton,
        LeftPageButton,
        RightPageButton,
        CreateRoom,
        RefreshButton
    }

    private void Start()
    {
        InitButtons<Buttons>();
        _uiRoomListPacket = gameObject.AddComponent<UIRoomListPacket>();
        _uiRoomListPacket.WaitForPacket();
    }

    private void MainMenuButton()
    {
        Managers.UI.LoadScenePanel(SceneUIType.MainMenuUI.ToString());
    }
    
    private void LeftPageButton()
    {
        //Switch to Left Page
    }
    
    private void RightPageButton()
    {
        //Switch to Right Page
    }
    
    private void CreateRoom()
    {
        Managers.UI.LoadScenePanel(SceneUIType.MainMenuUI.ToString());
    }
    
    private void RefreshButton()
    {
        //Refresh Room List
    }
}
