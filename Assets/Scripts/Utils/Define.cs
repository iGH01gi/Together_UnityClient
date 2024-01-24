using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SaveFiles
    {
        Player,
        Display,
        Sound,
        Control,
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
        Effects,
    }

    public enum PlayerAction
    {
        Idle,
        Run,
        Walk,
        Jump,
    }
    
    public enum DisplayQuality
    {
        Low,
        Medium,
        High,
    }

    public enum MainMenuButtons
    {
        StartGame,
        Shop,
        Settings,
        EndGame
    }
}
