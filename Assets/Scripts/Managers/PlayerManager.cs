using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private MyPlayer _myPlayer; //내 플레이어
    private Dictionary<int, Player> _players = new Dictionary<int, Player>(); //다른 플레이어들
    
    public void Add(S_PlayerList packet)
    {
        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Managers.Resource.Instantiate("Player") as GameObject;

            if (p.isSelf) //내 플레이어
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.transform.position=new Vector3(p.posX,p.posY,p.posZ);
                _myPlayer = myPlayer;
            }
            else //다른 플레이어
            {
                Player player = go.AddComponent<Player>();
                _players.
            }
        }
        
    }
}
