using System.Net;
using Google.Protobuf.Protocol;
using UnityEngine;

public class DedicatedServerSession : ServerSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"OnConnected DediServer : {endPoint}");

        if (PacketManager.Instance.CustomHandler == null)
        {
            PacketManager.Instance.CustomHandler = (s, m, i) =>
            {
                PacketQueue.Instance.Push(i, m);
            };
        }
        
        //데디서버에 입장 요청 패킷 보냄
        if(Managers.Player._myRoomPlayer != null) //내 플레이어가 방에 있는 상태라면
        {
            CDS_AllowEnterGame sendPacket = new CDS_AllowEnterGame();
            sendPacket.RoomId = Managers.Player._myRoomPlayer.Room.Info.RoomId;
            sendPacket.Name = Managers.Player._myRoomPlayer.Name;
            Send(sendPacket);
        }
    }
    
    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"OnDisconnected DediServer : {endPoint}");
        
        //TODO: 데디서버 세션 연결이 끊겼을 때 처리
    }
}