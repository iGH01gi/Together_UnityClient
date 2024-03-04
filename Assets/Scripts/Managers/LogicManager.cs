using System;
using UnityEngine;


/// <summary>
/// 서버와 동기화 하는 로직을 담당하는 매니저.
/// </summary>
public class LogicManager
{
    private float _tick = 0.1f; //초당 10회(이 주기마다 동기화, 데디서버와 맞춰야 함)
    private float _timer = 0.0f;
    
    /// <summary>
    /// 플레이어 움직임 정보 보내는 이벤트
    /// </summary>
    public event Action SendMyPlayerMoveEvent;
    
    public void Update()
    {
        //TODO: 다른 플레이어 움직임 동기화 패킷 받아서 먼저 처리하기
        
        _timer += Time.deltaTime;
        if(_timer >= _tick)
        {
            SendMyPlayerMoveEvent?.Invoke(); //플레이어 움직임 정보 보냄
            _timer = 0;
        }
    }
    
}