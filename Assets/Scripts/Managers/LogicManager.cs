using System;
using UnityEngine;


public class LogicManager
{
    private static float _gameLogicInterval = 0.03f; //초당 20회(데디서버와 맞춰야 함)
    
    /// <summary>
    /// 매 프레임마다 실행할 이벤트들
    /// </summary>
    public event Action FrameEvent;
    
    /// <summary>
    /// 플레이어 움직임 이벤트
    /// </summary>
    public event Action PlayerMoveEvent;

    public static void Init()
    {
        // FixedUpdate를 초당 20번 호출하도록 설정합니다.
        Time.fixedDeltaTime = _gameLogicInterval;
    }
    
    public void Update()
    {
        FrameEvent?.Invoke(); //프레임 이벤트 실행
    }
    
    public void FixedUpdate()
    {
        PlayerMoveEvent?.Invoke(); //플레이어 움직임 계산 이벤트 실행
    }
}