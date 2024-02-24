using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayAnim(Vector2 moveInput, bool isRunning)
    {
        if (moveInput.magnitude > 0)
        {
            if (isRunning)
            {
                PlayerAnimClear();
                _anim.SetBool("isRunning", true);
            }
            else
            {
                PlayerAnimClear();
                _anim.SetBool("isWalking", true);
            }
        }
        else
        {
            PlayerAnimClear();
        }
    }
    
    void PlayerAnimClear()
    {
        _anim.SetBool("isRunning", false);
        _anim.SetBool("isWalking",false);
    }
}
