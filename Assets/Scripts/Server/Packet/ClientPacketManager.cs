using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();

	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }
		
	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.ScRoomList, MakePacket<SC_RoomList>);
		_handler.Add((ushort)MsgId.ScRoomList, PacketHandler.SC_RoomListHandler);		
		_onRecv.Add((ushort)MsgId.ScMakeRoom, MakePacket<SC_MakeRoom>);
		_handler.Add((ushort)MsgId.ScMakeRoom, PacketHandler.SC_MakeRoomHandler);		
		_onRecv.Add((ushort)MsgId.ScAllowEnterRoom, MakePacket<SC_AllowEnterRoom>);
		_handler.Add((ushort)MsgId.ScAllowEnterRoom, PacketHandler.SC_AllowEnterRoomHandler);		
		_onRecv.Add((ushort)MsgId.ScInformNewFaceInRoom, MakePacket<SC_InformNewFaceInRoom>);
		_handler.Add((ushort)MsgId.ScInformNewFaceInRoom, PacketHandler.SC_InformNewFaceInRoomHandler);		
		_onRecv.Add((ushort)MsgId.ScLeaveRoom, MakePacket<SC_LeaveRoom>);
		_handler.Add((ushort)MsgId.ScLeaveRoom, PacketHandler.SC_LeaveRoomHandler);		
		_onRecv.Add((ushort)MsgId.ScPingPong, MakePacket<SC_PingPong>);
		_handler.Add((ushort)MsgId.ScPingPong, PacketHandler.SC_PingPongHandler);		
		//_onRecv.Add((ushort)MsgId.DscPingPong, MakePacket<DSC_PingPong>);
		//_handler.Add((ushort)MsgId.DscPingPong, PacketHandler.DSC_PingPongHandler);		
		_onRecv.Add((ushort)MsgId.ScConnectDedicatedServer, MakePacket<SC_ConnectDedicatedServer>);
		_handler.Add((ushort)MsgId.ScConnectDedicatedServer, PacketHandler.SC_ConnectDedicatedServerHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		//유니티 메인쓰레드 실행용(OnConnected에서 구현)
		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}