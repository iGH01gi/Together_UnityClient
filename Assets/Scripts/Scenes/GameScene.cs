using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameScene : BaseScene
{
    
    protected override void Init()
    {
        base.Init();
        Managers.UI.LoadPopupPanel<ProgressPopup>(true,false);
        Managers.Game.GameScene();
        Managers.Sound.Play("tense-horror-background",Define.Sound.Bgm);
    }
    
    

    public override void Clear()
    {
        
    }
}
