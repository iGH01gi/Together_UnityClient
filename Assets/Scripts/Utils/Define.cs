using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SaveFiles
    {
        KeyBinding
    }
    public enum Scene
    {
        //Scene Types that can occur
        Unknown,
        Lobby,
        InGame,
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
