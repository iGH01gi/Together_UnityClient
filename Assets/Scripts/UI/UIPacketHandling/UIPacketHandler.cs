using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class UIPacketHandler
{
    public static void WaitForPacket()
    {
        Managers.Input.DisableInput();
        Managers.UI.LoadPopupPanel<ProgressPopup>(true);
    }

    public static void OnReceivePacket()
    {
        Managers.UI.CloseTopPopup();
        Managers.Input.EnableInput();
    }
    
    public static void RoomListSendPacket()
    {
        WaitForPacket();
        C_RoomList packet = new C_RoomList();
        Managers.Network._session.Send(packet);
    }
    
    public static void RoomListOnReceivePacket()
    {
        OnReceivePacket(); 
        Managers.UI.SceneUI.GetComponent<LobbyUI>().ReceiveNewRoomList();
    }
}
