using System;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;
using UnityEngine.InputSystem;

public class PacketHandler
{
    //서버한테 방 리스트를 받고 갱신함
    public static void SC_RoomListHandler(PacketSession session, IMessage packet)
    {
        SC_RoomList roomListPacket = packet as SC_RoomList;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log($"SC_RoomListHandler, {roomListPacket.Rooms.Count}개의 방 존재");
        
        //방 목록 새로 받은정보로 갱신
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
            Managers.Room.ProcessEnterRoom(allowEnterRoomPacket, callback: UIPacketHandler.EnterRoomReceivePacket); 
        }
        else
        {
            UIPacketHandler.OnReceivePacket();
            Debug.Log(allowEnterRoomPacket.ReasonRejected.ToString());
            //거부된 이유에 해당하는 팝업 띄우기
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
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: UIPacketHandler.RequestLeaveRoomReceivePacket);
        }
        else
        {
            Managers.Room.ProcessLeaveRoom(leaveRoomPacket, callback: UIPacketHandler.OthersLeftRoomReceivePacket);
        }
    }
    
    //방 안 플레이어들의 레디 관련 정보가 왔을때
    public static void SC_ReadyRoomHandler(PacketSession session, IMessage packet)
    {
        SC_ReadyRoom readyRoomPacket = packet as SC_ReadyRoom;
        ServerSession serverSession = session as ServerSession;
        
        Debug.Log("SC_ReadyRoomHandler");
        
        Managers.Room.ProcessReadyRoom(readyRoomPacket, callback: UIPacketHandler.UpdateRoomReadyStatus);
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
    
    //지금 session이 데디서버세션이아니라 서버세션으로 등록되는것이 문제임.
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
    
    //데디케이트서버와 연결을 시도하고, 연결되었다면 입장요청 패킷을 보냄
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
            Managers.Dedicated.ConnectToDedicatedServer(dediIP, dediPort);
            
            //게임씬 변경
            Managers.Scene.LoadScene(Define.Scene.InGame);
        }
        else//데디케이티드 서버 연결 실패
        {
            Debug.Log("데디케이티드 서버 연결 실패");
        }
    }
    
    //데디케이트서버로부터 게임에 입장을 허가받았을때의 처리
    public static void DSC_AllowEnterGameHandler(PacketSession session, IMessage packet)
    {
        DSC_AllowEnterGame allowEnterGamePacket = packet as DSC_AllowEnterGame;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_AllowEnterGameHandler");
        
        //TODO : 데디케이티드 서버로부터 게임에 입장을 허가받았을때의 처리
        Managers.Dedicated.AllowEnterGame(allowEnterGamePacket);
    }
    
    //데디케이트서버로부터 새로운 유저가 들어왔을때의 처리
    public static void DSC_InformNewFaceInDedicatedServerHandler(PacketSession session, IMessage packet)
    {
        DSC_InformNewFaceInDedicatedServer informNewFaceInDedicatedServerPacket = packet as DSC_InformNewFaceInDedicatedServer;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_InformNewFaceInDedicatedServerHandler");
        
        Managers.Dedicated.InformNewFaceInDedicatedServer(informNewFaceInDedicatedServerPacket, callback:()=>{});
    }

    //데디케이티드 서버로부터 유저가 나갔을때의 처리
    public static void DSC_InformLeaveDedicatedServerHandler(PacketSession session, IMessage packet)
    {
        DSC_InformLeaveDedicatedServer informLeaveDedicatedServerPacket = packet as DSC_InformLeaveDedicatedServer;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_InformLeaveDedicatedServerHandler");
        
        Managers.Dedicated.InformLeaveDedicatedServer(informLeaveDedicatedServerPacket, callback:()=>{});
    }
    
    //데디케이트서버로부터 게임 시작을 알려받았을때의 처리 (이 패킷을 받은 클라는  3,2,1 카운트 후 게임을 시작함)
    public static void DSC_StartGameHandler(PacketSession session, IMessage packet)
    {
        DSC_StartGame startGamePacket = packet as DSC_StartGame;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_StartGameHandler");
        Managers.UI.ClosePopup(); //현재 띄워져 있는 progressPopup을 닫는다
        Managers.UI.ClosePopup(); //현재 띄워져 있는 progressPopup을 닫는다
        Managers.UI.LoadScenePanel("InGameUI"); //Timer를 포함한 인게임UI 부르기.
        Managers.Player._myDediPlayer.GetComponent<PlayerInput>().DeactivateInput();
    }
    
    //데디케이트서버로부터 유저의 움직임을 받았을때의 처리
    public static void DSC_MoveHandler(PacketSession session, IMessage packet)
    {
        DSC_Move movePacket = packet as DSC_Move;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        //Debug.Log("DSC_MoveHandler");
        
        Managers.Player._syncMoveCtonroller.SyncOtherPlayerMove(movePacket);
        
        //killer가 존재하고 내가 아닐시에 심장소리
        if(Managers.Player.GetKillerId()!= -1 && !Managers.Player.IsMyDediPlayerKiller()){
            Managers.Game.PlayKillerSound(); //킬러가 근처에 있으면 심장소리 재생
        }
    }
    
    //데디케이트서버로부터 낮 타이머 시작을 받았을때의 처리
    public static void DSC_DayTimerStartHandler(PacketSession session, IMessage packet)
    {
        DSC_DayTimerStart dayTimerStartPacket = packet as DSC_DayTimerStart;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_DayTimerStartHandler");
        
        int daySeconds = dayTimerStartPacket.DaySeconds; //낮 시간(초)
        float estimatedCurrentServerTimer = daySeconds - Managers.Time.GetEstimatedLatency(); //현재 서버 타이머 시간(예측)

        Managers.Game.ChangeToDay(); //낮임을 설정
        
        UIPacketHandler.StartGameHandler(); //게임 시작 팝업
        Managers.Game._clientTimer.Init(daySeconds); //클라이언트 타이머 초기화
        Managers.Game._clientTimer.CompareTimerValue(estimatedCurrentServerTimer); //클라이언트 타이머 시간 동기화

        //낮->일몰 효과를 설정함 (낮 시간의 2/3초동안은 낮상태 유지. 남은 낮 시간의 1/3초동안 일몰로 천천히 전환됨)
        Managers.Scene.SimulateDayToSunset(daySeconds*2/3, daySeconds/3);
        
        //킬러정보 초기화
        Managers.Player.ClearKiller();
    }

    //데디케이트서버로부터 낮 타이머 싱크를 받았을때의 처리
    public static void DSC_DayTimerSyncHandler(PacketSession session, IMessage packet)
    {
        DSC_DayTimerSync dayTimerSyncPacket = packet as DSC_DayTimerSync;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;

        Debug.Log("DSC_DayTimerSyncHandler");

        float currentServerTimer = dayTimerSyncPacket.CurrentServerTimer; 
        float estimatedCurrentServerTimer = currentServerTimer - Managers.Time.GetEstimatedLatency(); //현재 서버 타이머 시간(예측)
        
        Managers.Game._clientTimer.CompareTimerValue(estimatedCurrentServerTimer); //클라이언트 타이머 시간 동기화
    }

    //데디케이트서버로부터 낮 타이머 종료를 받았을때의 처리
    public static void DSC_DayTimerEndHandler(PacketSession session, IMessage packet)
    {
        DSC_DayTimerEnd dayTimerEndPacket = packet as DSC_DayTimerEnd;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;

        Debug.Log("DSC_DayTimerEndHandler");
        
        int kiilerId = dayTimerEndPacket.KillerPlayerId;
        Managers.Player.SetKiller(kiilerId, callback:()=>{}); //킬러 설정 + 그 이후 실행될 callback함수
        
        Managers.Player._myDediPlayer.GetComponent<PlayerInput>().DeactivateInput();
        UIPacketHandler.TimerEndedInServer();
    }
    
    //데디케이트서버로부터 밤 타이머 시작을 받았을때의 처리
    public static void DSC_NightTimerStartHandler(PacketSession session, IMessage packet)
    {
        DSC_NightTimerStart nightTimerStartPacket = packet as DSC_NightTimerStart;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;

        Debug.Log("DSC_NightTimerStartHandler");

        int nightSeconds = nightTimerStartPacket.NightSeconds; //밤 시간(초)
        float estimatedCurrentServerTimer = nightSeconds - Managers.Time.GetEstimatedLatency(); //현재 서버 타이머 시간(예측)
        
        float gaugeMax = nightTimerStartPacket.GaugeMax; //게이지 최대값 및 초기값
        MapField<int,float> playerGaugeDecreasePerSecond = nightTimerStartPacket.PlayerGaugeDecreasePerSecond; //플레이어별 게이지 감소량
        
        Managers.Game._gaugeMax = gaugeMax; //게이지 최대값 설정
        Managers.Game.SetAllGaugeDecreasePerSecond(playerGaugeDecreasePerSecond); //플레이어별 게이지 감소량을 모든 플레이어에게 적용
        Managers.Game.SetAllGauge(gaugeMax); //모든 플레이어의 gauge값을 gaugeMax로 초기화

        //dediplayerId를 key로, value로 estimatedgauge로 해서 map형식으로 구함. 만약 value가 0보다 작으면 0으로 설정
        MapField<int,float> estimatedGauge = new MapField<int, float>();
        foreach (int dediPlayerId in playerGaugeDecreasePerSecond.Keys)
        {
            float estimatedValue = gaugeMax - playerGaugeDecreasePerSecond[dediPlayerId] * Managers.Time.GetEstimatedLatency();
            if (estimatedValue<=0)
                estimatedValue = 0;

            estimatedGauge.Add(dediPlayerId, estimatedValue);
        }
        
        Managers.Game._isDay = false; //밤임을 설정
        Managers.Game.ChangeToNight(); //밤임을 설정
        Managers.Player._myDediPlayer.GetComponent<PlayerInput>().ActivateInput();
        Managers.Game._clientTimer.Init(nightSeconds); //클라이언트 타이머 초기화
        Managers.Game._clientTimer.CompareTimerValue(estimatedCurrentServerTimer); //클라이언트 타이머 시간 동기화
    }
    
    //데디케이트서버로부터 밤 타이머 싱크를 받았을때의 처리
    public static void DSC_NightTimerSyncHandler(PacketSession session, IMessage packet)
    {
        DSC_NightTimerSync nightTimerSyncPacket = packet as DSC_NightTimerSync;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;

        Debug.Log("DSC_NightTimerSyncHandler");

        float currentServerTimer = nightTimerSyncPacket.CurrentServerTimer;
        float estimatedCurrentServerTimer = currentServerTimer - Managers.Time.GetEstimatedLatency(); //현재 서버 타이머 시간(예측)
        Debug.Log(estimatedCurrentServerTimer);
        Managers.Game._clientTimer.CompareTimerValue(estimatedCurrentServerTimer); //클라이언트 타이머 시간 동기화
    }
    
    //데디케이트서버로부터 밤 타이머 종료를 받았을때의 처리
    public static void DSC_NightTimerEndHandler(PacketSession session, IMessage packet)
    {
        DSC_NightTimerEnd nightTimerEndPacket = packet as DSC_NightTimerEnd;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;

        Debug.Log("DSC_NightTimerEndHandler");
        
        //킬러정보 초기화
        Managers.Player.ClearKiller();
        
        Managers.Player._myDediPlayer.GetComponent<PlayerInput>().DeactivateInput();
        UIPacketHandler.TimerEndedInServer();
    }

    //데디케이트서버로부터 새로운 상자 정보를 받았을때의 처리
    public static void DSC_NewChestsInfoHandler(PacketSession session, IMessage packet)
    {
        DSC_NewChestsInfo newChestsInfoPacket = packet as DSC_NewChestsInfo;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_NewChestsInfoHandler");
        
        Managers.Object.ChestSetAllInOne(newChestsInfoPacket);
    }

    //데디케이티드서버가 누군가가 상자를 여는데 성공했음을 알려줄때의 처리
    public static void DSC_ChestOpenSuccessHandler(PacketSession session, IMessage packet)
    {
        DSC_ChestOpenSuccess chestOpenSuccessPacket = packet as DSC_ChestOpenSuccess;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_ChestOpenSuccessHandler");
        
        int chestId = chestOpenSuccessPacket.ChestId;
        int playerId = chestOpenSuccessPacket.PlayerId;
        
        //여는데 성공한것이 나인가? 남인가?
        if(playerId == Managers.Player._myDediPlayerId)
        {
            //나의 상자 열기 성공 처리
            Managers.Object.OnMyPlayerOpenChestSuccess(chestId);
        }
        else
        {
            //다른 유저의 상자 열기 성공 처리
            Managers.Object.OnOtherPlayerOpenChestSuccess(chestId, playerId);
        }
    }
    
    //데디케이티드서버로부터 타임스탬프를 받았을때의 처리
    public static void DSC_ResponseTimestampHandler(PacketSession session, IMessage packet)
    {
        DSC_ResponseTimestamp responseTimestampPacket = packet as DSC_ResponseTimestamp;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        //Debug.Log("DSC_ResponseTimestampHandler");
        
        Managers.Time.OnRecvDediServerTimeStamp(responseTimestampPacket);
    }
    
    public static void DSC_GaugeSyncHandler(PacketSession session, IMessage packet)
    {
        DSC_GaugeSync gaugeSyncPacket = packet as DSC_GaugeSync;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
            
        MapField<int,float> playerGauges = gaugeSyncPacket.PlayerGauges;
        MapField<int, float> playerGaugeDecreasePerSecond = gaugeSyncPacket.PlayerGaugeDecreasePerSecond;
        //dediplayerId를 key로, value로 estimatedgauge로 해서 map형식으로 구함
        MapField<int,float> estimatedGauge = new MapField<int, float>();
        foreach (int dediPlayerId in playerGauges.Keys)
        {
            float estimatedValue = playerGauges[dediPlayerId] -
                                   playerGaugeDecreasePerSecond[dediPlayerId] * Managers.Time.GetEstimatedLatency();
            if (estimatedValue<=0)
                estimatedValue = 0;

            estimatedGauge.Add(dediPlayerId, estimatedValue);
        }
        
        Debug.Log("DSC_GaugeSyncHandler");
    }
    
    public static void DSC_PlayerDeathHandler(PacketSession session, IMessage packet)
    {
        DSC_PlayerDeath playerDeathPacket = packet as DSC_PlayerDeath;
        DedicatedServerSession dedicatedServerSession = session as DedicatedServerSession;
        
        Debug.Log("DSC_PlayerDeathHandler");
        
        int playerId = playerDeathPacket.PlayerId;
        DeathCause deathCause = playerDeathPacket.DeathCause;
       
        if(playerId == Managers.Player._myDediPlayerId) //'내'가 죽었을 경우
        {
            if (deathCause == DeathCause.TimeOver) //밤 시간이 끝나서 '킬러'인 '나' 사망
            {
                
            }
            else if (deathCause == DeathCause.GaugeOver) //게이지가 다 닳아서 '나' 사망
            {
                
            }
        }
        else //다른 플레이어가 죽었을 경우
        {
            if (deathCause == DeathCause.TimeOver) //밤 시간이 끝나서 '킬러'인 '다른 플레이어' 사망
            {
                
            }
            else if (deathCause == DeathCause.GaugeOver) //게이지가 다 닳아서 '다른 플레이어' 사망
            {
                
            }
        }
    }
}