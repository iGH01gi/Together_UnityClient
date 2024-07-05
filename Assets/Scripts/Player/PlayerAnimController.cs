using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _anim;
    public static bool isRunning = false;
    public static bool isWalking = false;
    public static bool isDigging = false;
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        _anim.SetBool("isRunning", isRunning);
        _anim.SetBool("isWalking",isWalking);
        _anim.SetBool("isDigging",isDigging);
    }
    
    void PlayerAnimClear()
    {
        isRunning = false;
        isWalking = false;
        isDigging = false;
    }
}
