using System;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInput : MonoBehaviour
{
    int _runBit = (1 << 4);
    int _upBit = (1 << 3);
    int _leftBit = (1 << 2);
    int _downBit = (1 << 1);
    int _rightBit = 1;
    
    
    public static Vector2 _moveInput;
    static int sensitivityAdjuster = 3;
    static float _walkSpeed = 5f;
    static float _runSpeed = 7.5f;
    public static float _minViewDistance = 15f;
    static float _mouseSensitivity;
    private float _rotationX = 0f;
    public Vector3 _velocity;

    CharacterController _controller;
    private Transform _camera;
    private Transform _player;
    private Transform _prefab;

    public static bool _isRunning = false;
    private void ChangeAnim()
    {
        _player.GetComponent<PlayerAnimController>().PlayAnim(_moveInput,_isRunning);
    }
    
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        ChangeAnim();
    }

    void OnRun(InputValue value)
    {
        _isRunning = value.isPressed;
        ChangeAnim();
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mouseSensitivity = Managers.Data.Player.MouseSensitivity *sensitivityAdjuster;
        _prefab = gameObject.transform;
        _camera = _prefab.transform.GetChild(0);
        _player = _prefab.transform.GetChild(1);
        _velocity = new Vector3(0f,0f,0f);
        
        Managers.Logic.SendMyPlayerMoveEvent -= SendMove;
        Managers.Logic.SendMyPlayerMoveEvent += SendMove;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        
        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -70f, _minViewDistance);
        
        _camera.localRotation = Quaternion.Euler(_rotationX,0f,0f);
        _prefab.Rotate(3f * mouseX * Vector3.up);
        if (_moveInput.magnitude<=0)
        {
            _player.transform.Rotate(3f * -mouseX * Vector3.up);
        }
        if (_moveInput.magnitude > 0)
        {
            _player.transform.localRotation =
                Quaternion.AngleAxis(Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg, Vector3.up);
        }
        _velocity= CalculateVelocity(_moveInput, _prefab.localRotation);
        _controller.Move(_velocity * Time.deltaTime);
    }


    void SendMove()
    {
        //서버로 데디플레이어id, 현재 transform정보(고스트에서 사용), 누른 키, 플레이어 회전정보를 보냄
        CDS_Move packet = new CDS_Move();
        
        packet.MyDediplayerId = Managers.Player._myDediPlayerId;
        
        TransformInfo ghostTransformInfo = new TransformInfo();
        PositionInfo ghostPositionInfo = new PositionInfo();
        RotationInfo ghostRotationInfo = new RotationInfo();
        
        Vector3 position = _prefab.position;
        Quaternion rotation = _prefab.rotation;
        ghostPositionInfo.PosX = position.x;
        ghostPositionInfo.PosY = position.y;
        ghostPositionInfo.PosZ = position.z;
        ghostRotationInfo.RotX = rotation.x;
        ghostRotationInfo.RotY = rotation.y;
        ghostRotationInfo.RotZ = rotation.z;
        ghostRotationInfo.RotW = rotation.w;
        
        ghostTransformInfo.Position = ghostPositionInfo;
        ghostTransformInfo.Rotation = ghostRotationInfo;
        
        packet.GhostTransform = ghostTransformInfo;
        
        int moveBit = 0;
        if (_isRunning)
        {
            moveBit |= _runBit;
        }
        if (_moveInput.y > 0.5f) //윗키눌림
        {
            moveBit |= _upBit;
        }
        if(_moveInput.y < -0.5f) //아래키눌림
        {
            moveBit |= _downBit;
        }
        if(_moveInput.x < -0.5f) //왼쪽키눌림
        {
            moveBit |= _leftBit;
        }
        if(_moveInput.x > 0.5f) //오른쪽키눌림
        {
            moveBit |= _rightBit;
        }
        packet.KeyboardInput = moveBit;
 
        //플레이어 회전정보 (고스트가 아닌 플레이어의 회전정보로 사용)
        RotationInfo playerRotation = new RotationInfo();
        playerRotation.RotX = _player.rotation.x;
        playerRotation.RotY = _player.rotation.y;
        playerRotation.RotZ = _player.rotation.z;
        playerRotation.RotW = _player.rotation.w;
        packet.PlayerRotation = playerRotation;
        
        Managers.Network._dedicatedServerSession.Send(packet);
    }

    private Vector3 CalculateVelocity(Vector2 moveInputVector, Quaternion prefabRotation)
    {
        Vector3 velocity;
        if (_isRunning)
        {
            velocity = prefabRotation.normalized * new Vector3( moveInputVector.x, 0,  moveInputVector.y) * _runSpeed;
        }
        else
        {
            velocity = prefabRotation.normalized * new Vector3( moveInputVector.x, 0,  moveInputVector.y) * _walkSpeed;
        }

        if (!_controller.isGrounded)
        {
            velocity.y = -10f;
        }

        return velocity;
    }

}
