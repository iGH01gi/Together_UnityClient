using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    
    protected override void Init()
    {
        base.Init();
        Managers.UI.LoadScenePanel("InGameUI");
        //Managers.Sound.Play("Bgm/test_bgm",Define.Sound.Bgm);
    }
    
    

    public override void Clear()
    {
        
    }
}
