using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public MyPlayer _myPlayer; //내 플레이어
    
    public Dictionary<int, Player> _otherPlayers = new Dictionary<int, Player>(); //다른 플레이어들

    public void Clear()
    {
        _myPlayer = null;
        _otherPlayers.Clear();
    }
}
