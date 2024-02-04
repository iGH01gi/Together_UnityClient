using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuNavigator : MonoBehaviour
{
    private void Start()
    {
        UIUtils.BindUIButtonWithText("StartGame",ToLobby,transform);
        UIUtils.BindUIButtonWithText("Shop",ToLobby,transform);
        UIUtils.BindUIButtonWithText("Settings",Settings,transform);
        UIUtils.BindUIButtonWithText("EndGame",ExitGame,transform);
    }

    public void ToMainMenu()
    {
        
    }

    public void ToLobby()
    {
        
    }

    public void Settings()
    {
        
    }
    
    public void ExitGame()
    {
        PopUpManager.LoadYesNoPopup("QuitGamePopupDescription",Application.Quit,Managers.UI.CloseTopPopup);
    }
}