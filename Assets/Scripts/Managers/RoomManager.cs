using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Protocol;

//방에 누군가가 들어오거나 나가면 PlayerManager와 GameRoom 둘 다 처리해줘야함

public class RoomManager
{
    public Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
    
    /// <summary>
    /// 룸 딕셔너리에 수동으로 방 목록 추가
    /// </summary>
    /// <param name="roomId">룸id/param>
    /// <param name="gameRoom">게임룸 정보</param>
    public void AddRoom(int roomId, GameRoom gameRoom)
    {
        _rooms.Add(roomId, gameRoom);
    }

    /// <summary>
    /// 방 목록 새로 받아서 갱신
    /// </summary>
    /// <param name="roomInfos">방 정보 리스트</param>
    /// <param name="callback">방이 갱신된 뒤에 처리할 함수(client)</param>
    public void RefreshRoomList(List<RoomInfo> roomInfos, Action callback)
    {
        //기존 방 목록인 _rooms 초기화
        _rooms.Clear();

        //roomInfos에 있는 방들을 _rooms에 갱신
        foreach (RoomInfo roomInfo in roomInfos)
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.Info = roomInfo;
            _rooms.Add(roomInfo.RoomId, gameRoom);
        }

        if (callback != null)
            callback.Invoke();
    }

    /// <summary>
    /// 해당 방 입장을 요청
    /// </summary>
    /// <param name="roomId">방 id</param>
    /// <param name="password">방 비번</param>
    /// <param name="name">플레이어 이름</param>
    public void RequestEnterRoom(int roomId, string password, string name)
    {
        CS_EnterRoom sendPacket = new CS_EnterRoom();
        sendPacket.RoomId = roomId;
        sendPacket.Password = password;
        sendPacket.Name = name;

        Managers.Network._roomSession.Send(sendPacket);
    }

    /// <summary>
    /// '내'가 방에 입장했을때 처리
    /// </summary>
    /// <param name="packet">서버로부터 받은 패킷</param>
    /// <param name="callback">입장했을때의 ui를 띄워주는 콜백함수</param>
    public void ProcessEnterRoom(SC_AllowEnterRoom packet, Action callback)
    {
        if (packet.CanEnter == false)
            return;

        /*if (!_rooms.ContainsKey(packet.Room.RoomId))
            return;*/

        //입장한 방에 서버로부터 받은 정보 넣기
        GameRoom gameRoom = _rooms[packet.Room.RoomId];
        gameRoom.Info = packet.Room;
        foreach (PlayerInfo playerInfo in packet.Players)
        {
            RoomPlayer roomPlayer = new RoomPlayer() { PlayerId = playerInfo.PlayerId, Name = playerInfo.Name };

            if (roomPlayer.PlayerId == packet.MyPlayerId) //'나'라면 플레이어 매니저 정보에 본인 입력 + 게임룸에 '나' 추가
            {
                Managers.Player._myRoomPlayer = new MyRoomPlayer()
                    { PlayerId = playerInfo.PlayerId, Name = playerInfo.Name, Room = gameRoom };
                gameRoom._players.Add(Managers.Player._myRoomPlayer);
            }
            else //다른사람이면 플레이어 매니저에 등록 + 게임룸에 추가
            {
                Managers.Player._otherRoomPlayers.Add(key: roomPlayer.PlayerId, value: roomPlayer);
                gameRoom._players.Add(roomPlayer);
            }
        }

        //입장한 방에 필요한 정보 등록과 플레이어매니저 정보 등록이 모두 완료되었기 때문에, 
        //callback함수를 실행하여서 입장한 방 상태를 UI에 반영
        if (callback != null)
            callback.Invoke();
    }

    /// <summary>
    /// 내가 있는 방에 다른 유저가 들어왔을때 처리
    /// </summary>
    /// <param name="packet">서버로부터 받은 패킷</param>
    /// <param name="callback">새로운 들어온 유저를 반영한 ui를 띄워주는 콜백함수</param>
    public void ProcessNewFaceInRoom(SC_InformNewFaceInRoom packet, Action callback)
    {
        if (!_rooms.ContainsKey(packet.RoomId))
            return;

        GameRoom gameRoom = _rooms[packet.RoomId];
        gameRoom.Info.CurrentCount = packet.CurrentCount;

        RoomPlayer roomPlayer = new RoomPlayer() { PlayerId = packet.NewPlayer.PlayerId, Name = packet.NewPlayer.Name };

        //플레이어 매니저에 등록
        Managers.Player._otherRoomPlayers.Add(key: roomPlayer.PlayerId, value: roomPlayer);
        //게임룸에 추가
        gameRoom._players.Add(roomPlayer);

        //새로 들어온 플레이어의 정보 처리가 완료되었기 때문에,
        //callback함수를 실행하여서 새로 들어온 플레이어의 정보를 방 UI에 반영
        if (callback != null)
            callback.Invoke();
    }

    /// <summary>
    /// 방을 본인 포함 누군가가 나갔을때 처리
    /// </summary>
    /// <param name="packet">서버로부터 받은 패킷</param>
    /// <param name="callback">방을 나갔을때의 ui 처리</param>
    public void ProcessLeaveRoom(SC_LeaveRoom packet, Action callback)
    {
        GameRoom gameRoom = Managers.Player._myRoomPlayer.Room;

        if (gameRoom == null)
            return;

        if (packet.PlayerId == Managers.Player._myRoomPlayer.PlayerId) //내가 방을 나간것일때
        {
            //나가기 전에 방 정보 건들여주기
            gameRoom._players.Remove(Managers.Player._myRoomPlayer);
            gameRoom.Info.CurrentCount = gameRoom._players.Count;
            gameRoom._players.Clear();

            //내 플레이어 정보 초기화
            Managers.Player.Clear();
        }
        else //다른 유저가 방을 나간것일때
        {
            //방정보 최신화
            gameRoom._players.RemoveAll(x => x.PlayerId == packet.PlayerId);

            //내 플레이어 정보 초기화
            if (Managers.Player._otherRoomPlayers.ContainsKey(packet.PlayerId))
                Managers.Player._otherRoomPlayers.Remove(packet.PlayerId);
        }
        
        if(callback!=null)
            callback.Invoke();
    }

}