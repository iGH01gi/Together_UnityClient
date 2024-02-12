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

    public static void MakeNewRoomSendPacket(string roomName, bool isPassword, string password)
    {
        C_MakeRoom cMakeRoom = new C_MakeRoom();
        cMakeRoom.Title = roomName;
        cMakeRoom.IsPrivate = isPassword;
        cMakeRoom.Password = password;
        Managers.Network._session.Send(cMakeRoom);
    }

    public static void EnterRoomReceivePacket()
    {
        OnReceivePacket();
        UI_scene.InstantiateSceneUI(UI_scene.SceneUIType.RoomUI);
    }
}
