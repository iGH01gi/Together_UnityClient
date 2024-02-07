using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRoomListPacket : UIPacketHandler
{
    public override void WaitForPacket()
    {
        base.WaitForPacket();
        C_RoomList packet = new C_RoomList();
        Managers.Network._session.Send(packet);
    }
    
    public override void OnReceivePacket()
    {
        base.OnReceivePacket(transform);
        foreach (var VARIABLE in Managers.Room._rooms)
        {
            
        }
    }
}
