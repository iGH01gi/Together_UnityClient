using DummyClient;
using ServerCore;
using UnityEngine;

public class PacketHandler
{
    /// <summary>
    /// 이미 내가 입장해있는 상태일때, 다른 애가 입장한것을 서버가 알려줬을때 처리하는 부분
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt=packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        Managers.Player.EnterGame(pkt);
    }
    
    /// <summary>
    /// 누군가가 나갔을때 서버에서 알려준것을 처리하는 부분
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt=packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;
        
        Managers.Player.LeaveGame(pkt);
    }
    
    /// <summary>
    /// 접속했을때 서버로부터 주변 플레이어들의 리스트를 받았을때 처리하는 부분
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt=packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;
        
        Managers.Player.Add(pkt);
    }
    
    /// <summary>
    /// 누군가가 이동했을때 서버에서 알려준것을 처리하는 부분
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt=packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;
        
        Managers.Player.Move(pkt);
    }
}