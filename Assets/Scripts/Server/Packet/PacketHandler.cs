using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public class PacketHandler
{
    //서버한테 방 리스트를 받고 갱신함
    public static void S_RoomListHandler(PacketSession session, IMessage packet)
    {
        S_RoomList roomListPacket = packet as S_RoomList;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log($"S_RoomListHandler, {roomListPacket.Rooms.Count}개의 방 존재");
        
        //방 목록 새로 받은정보로 갱신
        //TODO : callback함수는 받은 방 정보들로 ui 채워줘야 함. 방 정보들은 RoomManger의 _rooms들의 Info에 채워져있음
        Managers.Room.RefreshRoomList(roomListPacket.Rooms.ToList(), callback: UIPacketHandler.RoomListOnReceivePacket); 
    }
    
    //본인이 생성한 방 정보를 서버로부터 받음
    public static void S_MakeRoomHandler(PacketSession session, IMessage packet)
    {
        S_MakeRoom makeRoomPacket = packet as S_MakeRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_MakeRoomHandler");

        if (makeRoomPacket.Room != null)
        {
            //방 생성이 성공했다면, 해당 방 입장을 요청
            Managers.Room.RequestEnterRoom(makeRoomPacket.Room.RoomId, makeRoomPacket.Password, name: "TestName");
        }
    }
    
    //'나'의 방 입장을 허가or거부 받음
    public static void S_AllowEnterRoomHandler(PacketSession session, IMessage packet)
    {
        S_AllowEnterRoom allowEnterRoomPacket = packet as S_AllowEnterRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_AllowEnterRoomHandler");

        if (allowEnterRoomPacket.CanEnter == true)
        {
            //TODO : callback함수로 방 입장후 띄울 ui 처리. 필요한 정보는 RoomManger의 _rooms들의 Info와 _players에 채워져있음
            Managers.Room.ProcessEnterRoom(allowEnterRoomPacket, callback: UIPacketHandler.EnterRoomReceivePacket); 
        }
        else
        {
            //TODO : 방 입장 실패시 처리
        }
    }
    
    //'내'가 있는 방에 새로운 유저가 들어왔을때
    public static void S_InformNewFaceInRoomHandler(PacketSession session, IMessage packet)
    {
        S_InformNewFaceInRoom informNewFaceInRoomPacket = packet as S_InformNewFaceInRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_RefreshInRoomHandler");
        
        //TODO : callback함수로 새로 들어온 플레이어의 정보를 방 UI에 반영. 필요한 정보는 RoomManger의 _rooms들의 Info와 _players에 채워져있음
        Managers.Room.ProcessNewFaceInRoom(informNewFaceInRoomPacket, callback: null);
    }
    
    //방에서 누군가가 나갔을때 (본인포함)
    public static void S_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        S_LeaveRoom leaveRoomPacket = packet as S_LeaveRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_LeaveRoomHandler");
        
        if(leaveRoomPacket.PlayerId == Managers.Player._myPlayer.PlayerId)
        {
            //TODO : callback함수로 '내'가 방을 나갔을때의 ui 처리
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: null);
        }
        else
        {
            //TODO : callback함수로 방에 있는 다른 유저가 나갔을때 띄울 ui 처리
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: null);
        }
    }
    
    
    
    
    /****** 이 아래는 데디케이티드 서버로 구현하면서 변경될 예정 ***********/
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_EnterGameHandler");
        Debug.Log(enterGamePacket.Player);
    }
    
    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_LeaveGameHandler");
    }
    
    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_SpawnHandler");
        Debug.Log(spawnPacket.Players);
    }
    
    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_DespawnHandler");
        
    }
    
    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("S_MoveHandler");
    }
}