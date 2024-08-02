using UnityEngine;
using UnityEngine.Serialization;

public class MyDediPlayer : MonoBehaviour
{
    //현재 데디서버의 playerId는 독자적인 값(=데디서버의 sessionID)으로 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }
    
    public bool _isKiller = false; //킬러 여부
    
    public float _gauge = 0; //생명력 게이지
    public float _gaugeDecreasePerSecond = 0; //생명력 게이지 감소량
    
    public int _totalPoint = 0; //상자로 얻은 총 포인트(낮마다 초기화)
    
    public Inventory _inventory; //인벤토리
    
    
    public void Init(int playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
        
        _inventory = new Inventory();
    }

}