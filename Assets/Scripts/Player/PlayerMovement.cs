using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static bool _playerIsMoving;
    //probably should be stored elsewhere later
    public float _walkSpeed = 0.02f;
    public float _runSpeed = 0.03f;
    private Animator _anim;
    private float timeCount = 0;
    private float slerpFactor = 0.1f;

    private Vector2 moveInput;

    private void Start()
    {
        _anim = transform.GetChild(1).GetComponent<Animator>();
        _playerIsMoving = false;
    }

    private void Update()
    {
        if (moveInput!= Vector2.zero)
        {
            _playerIsMoving = true;   
            

                _anim.SetBool("isWalking", true);
                Move(_walkSpeed);
        }
        else
        {
            _playerIsMoving = false;
            _anim.SetBool("isRunning", false);
            _anim.SetBool("isWalking",false);
        }
    }

    private void Move(float moveSpeed)
    {
        Vector3 newPos = transform.rotation.normalized * new Vector3(moveSpeed * moveInput.x, 0f, moveSpeed * moveInput.y);
        LookDirection(Mathf.Atan2(moveInput.x,moveInput.y)* Mathf.Rad2Deg);
        transform.position += newPos;
    }

    private void LookDirection(float angle)
    {
        timeCount += (Time.deltaTime * slerpFactor);
        transform.GetChild(1).transform.localRotation = Quaternion.Slerp(transform.GetChild(1).transform.localRotation,Quaternion.AngleAxis(angle, Vector3.up),timeCount);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
}
