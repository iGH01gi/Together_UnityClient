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
    
    /*//내가 접속했을때, 서버로부터 모든 플레이어들의 리스트를 받아서 처리
    public void Add(S_PlayerList packet)
    {
        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Managers.Resource.Instantiate("Player") as GameObject;

            if (p.isSelf) //내 플레이어
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.playerId = p.playerId;
                myPlayer.transform.position=new Vector3(p.posX,p.posY,p.posZ);
                _myPlayer = myPlayer;
            }
            else //다른 플레이어
            {
                Player player = go.AddComponent<Player>();
                player.playerId = p.playerId;
                player.transform.position=new Vector3(p.posX,p.posY,p.posZ);
                _players.Add(p.playerId,player);
            }
        }
        
    }
    
    //나를 포함한 누군가가 이동했을때
    //정책을 1.먼저 이동하고 받은 값을통해서 보정
    //      2.ok패킷을 받아야지만 그제서야 이동
    //이렇게 둘 중에 고를수 있는데 여기서는 2번 정책을 사용
    public void Move(S_BroadcastMove packet)
    {
        if(_myPlayer.playerId==packet.playerId) 
        {
            _myPlayer.transform.position=new Vector3(packet.posX,packet.posY,packet.posZ);
        }
        else 
        {
            Player player = null;
            if(_players.TryGetValue(packet.playerId,out player))
            {
                Debug.Log($"서버에서 받은 좌표는 {packet.posX},{packet.posY},{packet.posZ}");
                player.transform.position=new Vector3(packet.posX,packet.posY,packet.posZ);
            }
        }
    }
    
    //이미 내가 입장해있는 상태일때, 새로운 애가 입장
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.playerId) //내가 입장한거라면
            return;
        
        GameObject go = Managers.Resource.Instantiate("Player") as GameObject;
        
        Player player = go.AddComponent<Player>();
        player.transform.position=new Vector3(packet.posX,packet.posY,packet.posZ);
        _players.Add(packet.playerId,player);
    }
    
    //나를 포함해서 누군가가 게임을 떠났을 때
    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if(_myPlayer.playerId==packet.playerId) //내가 떠난거라면
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer= null;
        }
        else //다른 애가 떠난거라면
        {
            Player player = null;
            if(_players.TryGetValue(packet.playerId,out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }*/
}
