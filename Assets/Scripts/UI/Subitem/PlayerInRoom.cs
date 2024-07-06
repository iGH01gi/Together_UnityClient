using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using TMPro;
using UnityEngine;

public class PlayerInRoom : UI_subitem
{
    private RoomPlayer thisPlayer;
    GameObject readyIcon;

    public void Init(RoomPlayer player)
    {
        thisPlayer = player;
        transform.Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;
        readyIcon = transform.Find("ReadyIcon").gameObject;
        readyIcon.SetActive(player.IsReady);
        Debug.Log(player.Name + " is ready: " + player.IsReady);
        transform.Find("MasterIcon").gameObject.SetActive(Managers.Room.IsMaster(Managers.Room.GetMyPlayerRoomId(),player.PlayerId));
    }
    
    public void ToggleReady()
    {
        CS_ReadyRoom readyRoomPacket = new CS_ReadyRoom();
        readyRoomPacket.RoomId = Managers.Room.GetMyPlayerRoomId();
        readyRoomPacket.PlayerId = Managers.Player._myRoomPlayer.PlayerId;
        readyRoomPacket.IsReady = !thisPlayer.IsReady;
        Managers.Network._roomSession.Send(readyRoomPacket);
    }
}
