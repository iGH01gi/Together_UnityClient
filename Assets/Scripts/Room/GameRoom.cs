using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class GameRoom
{
    public RoomInfo Info { get; set; } = new RoomInfo();
    
    public List<Player> _players = new List<Player>(); //방에 있는 플레이어 리스트
}
