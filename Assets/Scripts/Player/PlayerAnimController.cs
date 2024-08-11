using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _anim;
    public bool isRunning = false;
    public bool isWalking = false;
    public bool isDigging = false;
    public bool isPraying = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        _anim.SetBool("isRunning",isRunning);
        _anim.SetBool("isWalking",isWalking);
        _anim.SetBool("isDigging",isDigging);
        _anim.SetBool("isPraying",isPraying);
    }

    public void KillerBaseAttack()
    {
        _anim.SetTrigger("Attack");
    }

    public bool IsAttacking()
    {
        return _anim.GetCurrentAnimatorStateInfo(_anim.GetLayerIndex("Base Layer")).IsName("Attack");
    }

    void PlayerAnimClear()
    {
        isRunning = false;
        isWalking = false;
        isDigging = false;
        isPraying = false;
    }
}
