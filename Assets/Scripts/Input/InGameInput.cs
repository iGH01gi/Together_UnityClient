using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInput : MonoBehaviour
{
    public static Vector2 moveInput;
    static int sensitivityAdjuster = 3;
    static float _walkSpeed = 0.05f*100;
    static float _runSpeed = 0.075f*100;
    public static float minViewDistance = 15f;
    static float mouseSensitivity;
    private float rotationX = 0f;
    
    //서버 통신 관련 변수들
    //private float keyboardInputInterval = 0.04f; // 0.1초마다 키보드 입력 처리. 아마 이걸 예쌍 패킷 도착시간으로 생각하고 코딩해야할듯
    private double error=0; // 실제로 패킷을 보내고 올때까지의 시간과, 예상 시간과의 괴리. ms단
    private DateTime _packetSentTime;
    //private float timeSinceLastInput=0;

    private Transform camera;
    private Transform player;
    private Transform prefab;
    private Rigidbody rb;

    public static bool isRunning = false;
    public Define.PlayerAction playerState;
    
    private void ChangeAnim()
    {
        player.GetComponent<PlayerAnimController>().PlayAnim(moveInput,isRunning);
    }
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        ChangeAnim();
    }

    void OnRun(InputValue value)
    {
        isRunning = value.isPressed;
        ChangeAnim();
    }

    private void Start()
    {
        mouseSensitivity = Managers.Data.Player.MouseSensitivity *sensitivityAdjuster;
        prefab = gameObject.transform;
        camera = prefab.transform.GetChild(0);
        player = prefab.transform.GetChild(1);
        _velocity = Vector3.zero;
        _destination = prefab.position;
        rb = prefab.GetComponent<Rigidbody>();
        
        Managers.Logic.FrameEvent -= _Update;
        Managers.Logic.FrameEvent += _Update;
        Managers.Logic.PlayerMoveEvent -= _Move;
        Managers.Logic.PlayerMoveEvent += _Move;
    }
    
    void _Update()
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
        
        
        
        //prefab.GetComponent<Rigidbody>().velocity = _velocity ;
        /*if (Vector3.Distance(prefab.position, _destination) > 0.1f)
        {
            prefab.GetComponent<Rigidbody>().MovePosition(_destination);
        }
        else//transform.position이 _destination과 오차범위 이내로 들어오면 움직이지 않음
        {
            //prefab.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }*/
        
    }

    public Vector3 _destination;
    public Vector3 _velocity;
    void _Move()
    {
        //Send Packet(Vector2 moveInput, Vector3 player.rotation, bool isRunning)
        _destination = CalculateDestination(moveInput, prefab.rotation);
        rb.MovePosition(_destination);
        //_velocity = CalculateVelocity(moveInput, prefab.localRotation) * 100;
    }
    
    /*private Vector3 CalculateVelocity(Vector2 moveInputVector, Quaternion prefabRotation)
    {
        Vector3 velocity;
        if (isRunning)
        {
            velocity = prefabRotation.normalized * new Vector3(_runSpeed * moveInputVector.x, 0f, _runSpeed * moveInputVector.y);
        }
        else
        {
            velocity = prefabRotation.normalized * new Vector3(_walkSpeed * moveInputVector.x, 0f, _walkSpeed * moveInputVector.y);
        }
        
        return velocity;
    }*/
    
    private Vector3 CalculateDestination(Vector2 moveInputVector, Quaternion prefabRotation)
    {
        Vector3 movement;
        if (isRunning)
        {
            movement = prefabRotation.normalized * new Vector3(moveInputVector.x * _runSpeed , 0f, moveInputVector.y * _runSpeed ) * Time.fixedDeltaTime ;
        }
        else
        {
            movement = prefabRotation.normalized * new Vector3( moveInputVector.x * _walkSpeed, 0f, moveInputVector.y * _walkSpeed)*Time.fixedDeltaTime;
        }

        return transform.position + movement;
    }

}
