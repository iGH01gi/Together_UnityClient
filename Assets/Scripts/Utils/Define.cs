using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        //Scene Types that can occur
        Unknown,
        Login,
        Lobby,
        InGame,
        PostGame
    }
    public enum UIEvent
    {
        //UI Events that can occur
        Click,
        Drag
    }
    
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum PlayerAction
    {
        Idle,
        Run,
        Walk,
        Jump,
    }
}
