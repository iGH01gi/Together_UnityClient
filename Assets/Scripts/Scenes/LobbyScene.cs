using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        if (SceneType == Define.Scene.Unknown)
        {
            Managers.UI.LoadScenePanel("Panel");
            Managers.UI.LoadScenePanel("MainMenuButtons",GetComponent<MainMenuNavigator>());
            
        }

        SceneType = Define.Scene.Lobby;
    }

    public override void Clear()
    {
        
    }
}