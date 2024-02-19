using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInput : MonoBehaviour
{
    public static Vector2 moveInput;
    static int sensitivityAdjuster = 3;
    static float _walkSpeed = 0.03f;
    static float _runSpeed = 0.045f;
    public static float minViewDistance = 15f;
    static float mouseSensitivity;
    private float rotationX = 0f;
    
    //서버 통신 관련 변수들
    private float keyboardInputInterval = 0.04f; // 0.1초마다 키보드 입력 처리. 아마 이걸 예쌍 패킷 도착시간으로 생각하고 코딩해야할듯
    private double error=0; // 실제로 패킷을 보내고 올때까지의 시간과, 예상 시간과의 괴리. ms단
    private DateTime _packetSentTime;
    private float timeSinceLastInput=0;

    private Transform camera;
    private Transform player;
    private Transform prefab;

    public static bool isRunning = false;
    public Define.PlayerAction playerState;
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void OnRun(InputValue value)
    {
        isRunning = value.isPressed;

    }

    void OnJump(InputValue value)
    {
        if (value.Get<float>()>0)
        {
            //jump
        }
    }
    
    private void Start()
    {
        mouseSensitivity = Managers.Data.Player.MouseSensitivity *sensitivityAdjuster;
        prefab = GameObject.Find("Player").transform;
        camera = prefab.transform.GetChild(0);
        player = prefab.transform.GetChild(1);
    }
    
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -70f, minViewDistance);
        
        camera.localRotation = Quaternion.Euler(rotationX,0f,0f);
        prefab.Rotate(3f * mouseX * Vector3.up);
        if (moveInput.magnitude<=0)
        {
            player.transform.Rotate(3f * -mouseX * Vector3.up);
        }
        if (moveInput.magnitude > 0)
        {
            player.transform.localRotation =
                Quaternion.AngleAxis(Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg, Vector3.up);
        }
        
        timeSinceLastInput += Time.fixedDeltaTime;

        if (timeSinceLastInput >= keyboardInputInterval)
        {
            //Send Packet(Vector2 moveInput, Vector3 player.rotation, bool isRunning)
            prefab.GetComponent<Rigidbody>().MovePosition(Move(moveInput,prefab.position,prefab.localRotation,isRunning));
            timeSinceLastInput = 0;
        }
    }
    
    private Vector3 Move(Vector2 moveInputVector, Vector3 prefabPosition,Quaternion prefabRotation, bool toRunOrNot)
    {
        Vector3 newPos;
        if (isRunning)
        {
            newPos = prefabRotation.normalized * new Vector3(_runSpeed * moveInputVector.x, 0f, _runSpeed * moveInputVector.y);
        }
        else
        {
            newPos = prefabRotation.normalized * new Vector3(_walkSpeed * moveInputVector.x, 0f, _walkSpeed * moveInputVector.y);
        }

        prefabPosition += newPos;
        return prefabPosition;
    }

    void MoveCharacter()
    {
        
    }
}
