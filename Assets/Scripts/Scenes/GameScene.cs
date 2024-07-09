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
        Managers.Player._myDediPlayer.GetComponent<PlayerInput>().DeactivateInput();

        //Managers.Sound.Play("Bgm/test_bgm",Define.Sound.Bgm);
    }
    
    

    public override void Clear()
    {
        
    }
}
