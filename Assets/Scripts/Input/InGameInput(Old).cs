using System;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInputOld : MonoBehaviour
{
    int _runBit = (1 << 4);
    int _upBit = (1 << 3);
    int _leftBit = (1 << 2);
    int _downBit = (1 << 1);
    int _rightBit = 1;
    
    
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
        _destination = prefab.position;
        rb = prefab.GetComponent<Rigidbody>();
        _velocity = new Vector3(0f,rb.velocity.y,0f);
        
        Managers.Logic.SendMyPlayerMoveEvent -= SendMove;
        Managers.Logic.SendMyPlayerMoveEvent += SendMove;
    }
    
    void Update()
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
        _velocity= CalculateVelocity(moveInput, prefab.localRotation);
    }

    private void FixedUpdate()
    {
        rb.velocity = _velocity;
    }

    public Vector3 _destination;
    public Vector3 _velocity;
    void SendMove()
    {
        //서버로 현재위치,쿼터니언의 4개의 부동소수점 값, 누른 키, utc타임 정보를 보냄
        CDS_Move packet = new CDS_Move();
        
        packet.MyDediplayerId = Managers.Player._myDediPlayerId;
        
        TransformInfo transformInfo = new TransformInfo();
        Vector3 position = prefab.position;
        Quaternion rotation = prefab.rotation;
        transformInfo.PosX = position.x;
        transformInfo.PosY = position.y;
        transformInfo.PosZ = position.z;
        transformInfo.RotX = rotation.x;
        transformInfo.RotY = rotation.y;
        transformInfo.RotZ = rotation.z;
        transformInfo.RotW = rotation.w;
        packet.Transform = transformInfo;
        
        int moveBit = 0;
        if (isRunning)
        {
            moveBit |= _runBit;
        }
        if (moveInput.y > 0.5f) //윗키눌림
        {
            moveBit |= _upBit;
        }
        if(moveInput.y < -0.5f) //아래키눌림
        {
            moveBit |= _downBit;
        }
        if(moveInput.x < -0.5f) //왼쪽키눌림
        {
            moveBit |= _leftBit;
        }
        if(moveInput.x > 0.5f) //오른쪽키눌림
        {
            moveBit |= _rightBit;
        }
        packet.KeyboardInput = moveBit;
        
        packet.UtcTimeStamp = DateTime.UtcNow.ToBinary();
        
        Managers.Network._dedicatedServerSession.Send(packet);
    }

    private Vector3 CalculateVelocity(Vector2 moveInputVector, Quaternion prefabRotation)
    {
        Vector3 velocity;
        if (isRunning)
        {
            velocity = prefabRotation.normalized * new Vector3(_runSpeed * moveInputVector.x, 0, _runSpeed * moveInputVector.y);
        }
        else
        {
            velocity = prefabRotation.normalized * new Vector3(_walkSpeed * moveInputVector.x, 0, _walkSpeed * moveInputVector.y);
        }
        
        return velocity;
    }
   
}
