using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        Managers.UI.LoadScenePanel("MainMenuUI");
        Managers.Sound.Play("bgm/MainMenuMusic",Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}