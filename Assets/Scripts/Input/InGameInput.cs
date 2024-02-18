using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInput : MonoBehaviour
{
    private Vector2 moveInput;
    static int sensitivityAdjuster = 3;
    public float minViewDistance = 15f;
    static float mouseSensitivity;
    private float rotationX = 0f;
    
    //서버 통신 관련 변수들
    private float keyboardInputInterval = 0.1f; // 0.1초마다 키보드 입력 처리. 아마 이걸 예쌍 패킷 도착시간으로 생각하고 코딩해야할듯
    private double error=0; // 실제로 패킷을 보내고 올때까지의 시간과, 예상 시간과의 괴리. ms단
    private DateTime _packetSentTime;
    private float timeSinceLastInput=0;

    private GameObject camera;
    private GameObject player;

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    private void Start()
    {
        mouseSensitivity = Managers.Data.Player.MouseSensitivity *sensitivityAdjuster;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -70f, minViewDistance);
        
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(rotationX,0f,0f);
        transform.Rotate(3f * mouseX * Vector3.up);
        if (!PlayerMovement._playerIsMoving)
        {
            transform.GetChild(1).transform.Rotate(3f * -mouseX * Vector3.up);
        }
        
        timeSinceLastInput += Time.deltaTime;

        if (timeSinceLastInput >= keyboardInputInterval)
        {
            
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

    void MoveCharacter()
    {
        
    }
}
