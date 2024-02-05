using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        if (SceneType == Define.Scene.Unknown)
        {
            Managers.UI.LoadScenePanel("MainMenuUI");
        }
        SceneType = Define.Scene.Lobby;
    }

    public override void Clear()
    {
        
    }
}