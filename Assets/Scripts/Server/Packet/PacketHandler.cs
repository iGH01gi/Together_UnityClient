using System;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public class PacketHandler
{
    //서버한테 방 리스트를 받고 갱신함
    public static void SC_RoomListHandler(PacketSession session, IMessage packet)
    {
        SC_RoomList roomListPacket = packet as SC_RoomList;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log($"SC_RoomListHandler, {roomListPacket.Rooms.Count}개의 방 존재");
        
        //방 목록 새로 받은정보로 갱신
        //TODO : callback함수는 받은 방 정보들로 ui 채워줘야 함. 방 정보들은 RoomManger의 _rooms들의 Info에 채워져있음
        Managers.Room.RefreshRoomList(roomListPacket.Rooms.ToList(), callback: UIPacketHandler.RoomListOnReceivePacket); 
    }
    
    //본인이 생성한 방 정보를 서버로부터 받음
    public static void SC_MakeRoomHandler(PacketSession session, IMessage packet)
    {
        SC_MakeRoom makeRoomPacket = packet as SC_MakeRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_MakeRoomHandler");

        if (makeRoomPacket.Room != null)
        {
            //방 매니저에 방 추가
            GameRoom gameRoom = new GameRoom();
            gameRoom.Info = makeRoomPacket.Room;
            Managers.Room.AddRoom(makeRoomPacket.Room.RoomId,gameRoom);
            
            //방 생성이 성공했다면, 해당 방 입장을 요청
            Managers.Room.RequestEnterRoom(makeRoomPacket.Room.RoomId, makeRoomPacket.Password, name: "TestName");
        }
    }
    
    //'나'의 방 입장을 허가or거부 받음
    public static void SC_AllowEnterRoomHandler(PacketSession session, IMessage packet)
    {
        SC_AllowEnterRoom allowEnterRoomPacket = packet as SC_AllowEnterRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_AllowEnterRoomHandler");

        if (allowEnterRoomPacket.CanEnter == true)
        {
            //TODO : callback함수로 방 입장후 띄울 ui 처리. 필요한 정보는 RoomManger의 _rooms들의 Info와 _players에 채워져있음
            Managers.Room.ProcessEnterRoom(allowEnterRoomPacket, callback: UIPacketHandler.EnterRoomReceivePacket); 
        }
        else
        {
            //TODO : 방 입장 실패시 처리
            UIPacketHandler.OnReceivePacket();
            Debug.Log(allowEnterRoomPacket.ReasonRejected.ToString());
            if(allowEnterRoomPacket.ReasonRejected==ReasonRejected.RoomIsFull)
            {
                Managers.UI.LoadPopupPanel<RoomIsFull>();
            }
            else if(allowEnterRoomPacket.ReasonRejected==ReasonRejected.CurrentlyPlaying)
            {
                Managers.UI.LoadPopupPanel<CurrentlyPlaying>();
            }
            else if(allowEnterRoomPacket.ReasonRejected==ReasonRejected.RoomNotExist)
            {
                Managers.UI.LoadPopupPanel<RoomNotExist>();
            }
            else if(allowEnterRoomPacket.ReasonRejected==ReasonRejected.WrongPassword)
            {
                Managers.UI.LoadPopupPanel<WrongPassword>();
            }
        }
    }
    
    //'내'가 있는 방에 새로운 유저가 들어왔을때
    public static void SC_InformNewFaceInRoomHandler(PacketSession session, IMessage packet)
    {
        SC_InformNewFaceInRoom informNewFaceInRoomPacket = packet as SC_InformNewFaceInRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_InformNewFaceInRoomHandler");
        
        //TODO : callback함수로 새로 들어온 플레이어의 정보를 방 UI에 반영. 필요한 정보는 RoomManger의 _rooms들의 Info와 _players에 채워져있음
        Managers.Room.ProcessNewFaceInRoom(informNewFaceInRoomPacket, callback: UIPacketHandler.NewFaceEnterReceivePacket);
    }
    
    //방에서 누군가가 나갔을때 (본인포함)
    public static void SC_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        SC_LeaveRoom leaveRoomPacket = packet as SC_LeaveRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_LeaveRoomHandler");
        
        if(leaveRoomPacket.PlayerId == Managers.Player._myRoomPlayer.PlayerId)
        {
            //TODO : callback함수로 '내'가 방을 나갔을때의 ui 처리
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: UIPacketHandler.RequestLeaveRoomReceivePacket);
        }
        else
        {
            //TODO : callback함수로 방에 있는 다른 유저가 나갔을때 띄울 ui 처리
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: UIPacketHandler.OthersLeftRoomReceivePacket);
        }
    }
    
    public static void SC_PingPongHandler(PacketSession session, IMessage packet)
    {
        SC_PingPong pingPongPacket = packet as SC_PingPong;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_PingPongHandler");
        
        //서버로부터 핑을 받았다는 의미로, 살아있다는 응답으로 퐁을 보냄
        CS_PingPong sendPacket = new CS_PingPong();
        serverSession.Send(sendPacket);
    }
    
    public static void DSC_PingPongHandler(PacketSession session, IMessage packet)
    {
        DSC_PingPong pingPongPacket = packet as DSC_PingPong;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_PingPongHandler");
        
        //데디케이티드 서버로부터 핑을 받았다는 의미로, 살아있다는 응답으로 퐁을 보냄
        CDS_PingPong sendPacket = new CDS_PingPong();
        dedicatedServerSession.Send(sendPacket);
    }
    
    
    
    /****** 이 아래는 데디케이티드 서버로 구현하면서 변경될 예정 ***********/
    public static void SC_ConnectDedicatedServerHandler(PacketSession session, IMessage packet)
    {
        SC_ConnectDedicatedServer connectDedicatedServerPacket = packet as SC_ConnectDedicatedServer;
        ServerSession serverSession = session as ServerSession;
        Debug.Log("SC_ConnectDedicatedServerHandler");
        
        //데디케이티드 서버가 정상적으로 생성됨
        if(connectDedicatedServerPacket.Ip != null && connectDedicatedServerPacket.Port != -1) 
        {
            string dediIP = connectDedicatedServerPacket.Ip;
            int dediPort = connectDedicatedServerPacket.Port;
            
            //해당 데디케이티드 서버와 연결, 전용 세션 생성
            //세션이 생성되었을때만 입장요청 패킷(CDS_AllowEnterGame) 보냄을 보장함.
            Managers.Network.ConnectToDedicatedServer(dediIP, dediPort);
        }
        else//데디케이티드 서버 연결 실패
        {
            Debug.Log("데디케이티드 서버 연결 실패");
        }
    }
}