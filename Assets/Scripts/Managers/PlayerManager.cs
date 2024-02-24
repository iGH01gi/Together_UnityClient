using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 룸서버와 데디서버의 플레이어 정보를 들고있는 매니저
/// </summary>
public class PlayerManager
{
    public MyRoomPlayer _myRoomPlayer; //내 룸서버 플레이어
    public Dictionary<int, Player> _otherRoomPlayers = new Dictionary<int, Player>(); //다른 룸서버 플레이어들 (key: playerId, value: 플레이어 정보)
    
    
    public GameObject _myDediPlayer; //내 데디서버 플레이어
    public Dictionary<int,GameObject> _otherDediPlayers = new Dictionary<int, GameObject>(); //다른 데디서버 플레이어들 (key: 데디playerId, value: 플레이어 정보)
    
    public void Clear()
    {
        _myRoomPlayer = null;
        _otherRoomPlayers.Clear();
        
        ClearDedi();
    }

    public void ClearDedi()
    {
        if (_myDediPlayer != null)
        {
            GameObject.Destroy(_myDediPlayer);
            _myDediPlayer = null;
        }
        
        foreach (GameObject dediPlayer in _otherDediPlayers.Values)
        {
            if (dediPlayer != null)
            {
                GameObject.Destroy(dediPlayer);
            }
        }
        _otherDediPlayers.Clear();
    }

}
