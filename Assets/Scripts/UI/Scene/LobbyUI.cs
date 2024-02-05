using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : UI_scene
{
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
