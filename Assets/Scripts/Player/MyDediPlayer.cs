using UnityEngine;
using UnityEngine.Serialization;

public class MyDediPlayer : MonoBehaviour
{
    //현재 데디서버의 playerId는 독자적인 값(=데디서버의 sessionID)으로 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }
    
    public bool _isKiller = false; //킬러 여부
    
    
    public void Init(int playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
    }

}