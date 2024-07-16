using System.Collections.Generic;
using UnityEngine;

public class CleanseController : MonoBehaviour
{
    public string _cleanseObjectPath = "Map/Cleanses"; //클린즈들이 모여있는 부모 게임오브젝트 경로
    public List<GameObject> _cleansetList = new List<GameObject>(); //클린즈 리스트(인덱스는 클린즈 고유 ID)
    public float _cleansePoint = 0; //클린즈로 올라갈 게이지 정도
    public float _cleanseDurationSeconds = 0; //정화하는데 걸리는 시간(초 단위)
    public float _cleanseCoolTimeSeconds = 0; //클린즈를 사용한 후 쿨타임(초 단위)
    
    
}