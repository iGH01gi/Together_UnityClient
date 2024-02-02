using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public void Init()
    {
        GameObject root = GameObject.Find("@Input");
        if (root == null)
        {
            root = new GameObject { name = "@Input" };
            Object.DontDestroyOnLoad(root);
            root.AddComponent<PlayerInput>(); 
        }
    }
}
