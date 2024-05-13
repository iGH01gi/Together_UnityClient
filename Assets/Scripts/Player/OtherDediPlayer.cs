using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherDediPlayer : MonoBehaviour
{
    //현재 데디서버의 playerId는 독자적인 값(=데디서버의 sessionID)으로 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }
    
    CharacterController _controller;
    public GameObject _ghost;
    float rotationSpeed = 20f; // 회전 속도를 조절합니다.
    public Vector3 _velocity;
    bool _isRunning = false;
    Quaternion _ghostRotation;

    int _runBit = (1 << 4);
    int _upBit = (1 << 3);
    int _leftBit = (1 << 2);
    int _downBit = (1 << 1);
    int _rightBit = 1;

    static float _walkSpeed = 2f;
    static float _runSpeed = 3f;
    
    public void Init(int playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _velocity = new Vector3(0f, 0f, 0f);
        _isRunning = false;
    }

    private void Update()
    {
        if (_ghost == null)
        {
            _ghost = GameObject.Find("Ghost_" + PlayerId);
        }


        FollowGhost();
    }

    /// <summary>
    /// 자신의 ghost를 따라서 자연스럽게 움직이는 코드
    /// </summary>
    private void FollowGhost()
    {
        if (_ghost != null)
        {
            // 목표 방향을 계산합니다.
            Vector3 directionToGhost = _ghost.transform.position - transform.position;
            directionToGhost.y = 0;

            //목표 위치까지 거리가 0.05보다 작으면 도착한것으로 간주
            if (directionToGhost.magnitude < 0.05f)
            {
                _velocity = Vector3.zero;
                _controller.Move(_velocity);
                return;
            }

            // 목표 방향으로 이동합니다.
            _velocity = directionToGhost.normalized;
            if (_isRunning)
            {
                _velocity *= _runSpeed;
            }
            else
            {
                _velocity *= _walkSpeed;
            }
            
            
            if (!_controller.isGrounded)
            {
                _velocity.y = -10f;
            }
            
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    public void SetGhostLastState(int keyboardInput, Quaternion localRotation)
    {
        Vector2 moveInputVector = new Vector2();

        if ((keyboardInput & _runBit) != 1)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
        
        _ghostRotation = localRotation;
    }
}
