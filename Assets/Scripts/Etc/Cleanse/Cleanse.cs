using Google.Protobuf.Protocol;
using UnityEngine;

public class Cleanse : MonoBehaviour
{
    public int _cleanseId = 0; // 클린즈의 고유 ID (0부터 시작)
    public TransformInfo _transformInfo = new TransformInfo(); // 클린즈의 위치 정보
    public float _cleansePoint = 0; //클린즈로 올라갈 게이지 정도
    public float _cleanseDurationSeconds = 0; //정화하는데 걸리는 시간
    public float _cleanseCoolTimeSeconds = 0; //클린즈를 사용한 후 쿨타임
    private bool _isAvailable = true; // 클린즈 사용 가능 여부 (현재 쿨타임 중인지)

    /// <summary>
    /// 클린즈 정보 초기화
    /// </summary>
    /// <param name="cleanseId">클린즈id</param>
    /// <param name="transformInfo">위치,회전 정보</param>
    /// <param name="point">클린즈로 올라갈 게이지 정도</param>
    /// <param name="durationSeconds">정화하는데 걸리는 시간</param>
    /// <param name="coolTimeSeconds">클린즈를 사용한 후 쿨타임</param>
    public void InitCleanse(int cleanseId, TransformInfo transformInfo, float point, float durationSeconds, float coolTimeSeconds)
    {
        _cleanseId = cleanseId;
        _transformInfo = transformInfo;
        _cleansePoint = point;
        _cleanseDurationSeconds = durationSeconds;
        _cleanseCoolTimeSeconds = coolTimeSeconds;
        _isAvailable = true;
    }

}