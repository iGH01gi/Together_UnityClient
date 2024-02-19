using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public MyRoomPlayer _myRoomPlayer; //내 룸서버 플레이어
    public Dictionary<int, Player> _otherRoomPlayers = new Dictionary<int, Player>(); //다른 룸서버 플레이어들 (key: playerId, value: 플레이어 정보)
    
    public DediPlayer _myDediPlayer; //내 데디서버 플레이어
    public Dictionary<int,DediPlayer> _otherDediPlayers = new Dictionary<int, DediPlayer>(); //다른 데디서버 플레이어들 (key: 데디playerId, value: 플레이어 정보)
    
    public void Clear()
    {
        _myRoomPlayer = null;
        _otherRoomPlayers.Clear();
        
        _myDediPlayer = null;
        _otherDediPlayers.Clear();
    }

    public void ClearDedi()
    {
        _myDediPlayer = null;
        _otherDediPlayers.Clear();
    }

}
