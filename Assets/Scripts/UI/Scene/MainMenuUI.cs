using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : UI_scene
{
    private enum Buttons
    {
        StartGame,
        Shop,
        Settings,
        EndGame
    }
    
    private void Start()
    {
        InitButtons<Buttons>(true);
    }

    private void StartGame()
    {
        Managers.UI.LoadScenePanel(SceneUIType.LobbyUI.ToString());
    }
    
    private void Shop()
    {
        //implement shop
    }
    
    private void Settings()
    {
        //fix up settings
    }
    
    private void EndGame()
    {
        Managers.UI.LoadPopupPanel<QuitGameYesNoPopup>();
    }
}
