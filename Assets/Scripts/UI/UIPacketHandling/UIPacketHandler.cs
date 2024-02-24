using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class UIPacketHandler
{
    private GameObject go;
    public static void WaitForPacket()
    {
        Managers.Input.DisableInput();
        Managers.UI.LoadPopupPanel<ProgressPopup>(true);
    }

    public static void OnReceivePacket()
    {
        Managers.UI.ClosePopup();
        Managers.Input.EnableInput();
    }
    
    public static void RoomListSendPacket()
    {
        WaitForPacket();
        CS_RoomList packet = new CS_RoomList();
        Managers.Network._roomSession.Send(packet);
    }
    
    public static void RoomListOnReceivePacket()
    {
        OnReceivePacket(); 
        Managers.UI.SceneUI.GetComponent<LobbyUI>().ReceiveNewRoomList();
    }

    public static void MakeNewRoomSendPacket(string roomName, bool isPassword, string password)
    {
        CS_MakeRoom cMakeRoom = new CS_MakeRoom();
        cMakeRoom.Title = roomName;
        cMakeRoom.IsPrivate = isPassword;
        cMakeRoom.Password = password;
        Managers.Network._roomSession.Send(cMakeRoom);
    }

    public static void EnterRoomReceivePacket()
    {
        OnReceivePacket();
        UI_scene.InstantiateSceneUI(UI_scene.SceneUIType.RoomUI);
    }

    public static void LeaveRoomSendPacket(int roomID)
    {
        CS_LeaveRoom csLeaveRoom = new CS_LeaveRoom();
        csLeaveRoom.RoomId = roomID;
        Managers.Network._roomSession.Send(csLeaveRoom);
    }

    public static void RequestLeaveRoomReceivePacket()
    {
        UI_scene.InstantiateSceneUI(UI_scene.SceneUIType.LobbyUI);
    }
    
    public static void OthersLeftRoomReceivePacket()
    {
        Managers.UI.SceneUI.GetComponent<RoomUI>().RefreshButton();
    }

    public static void NewFaceEnterReceivePacket()
    {
        Managers.UI.SceneUI.GetComponent<RoomUI>().RefreshButton();
    }
}
