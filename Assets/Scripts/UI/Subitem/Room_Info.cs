using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room_Info : UI_subitem
{
    public GameRoom myroom { get; set; }

    public void Init(GameRoom gameRoom)
    {
        myroom = gameRoom;
        transform.GetChild(0).GetComponent<TMP_Text>().text = gameRoom.Info.Title;
        transform.GetChild(1).GetComponent<TMP_Text>().text =
            (gameRoom.Info.CurrentCount + "/" + gameRoom.Info.MaxCount);
        if (gameRoom.Info.IsPlaying)
        {
            transform.GetChild(2).GetComponent<UI_Text>().SetString("InGameStatus");
        }
        else
        {
            transform.GetChild(2).GetComponent<UI_Text>().SetString("WaitingStatus");
        }
        
        if (!gameRoom.Info.IsPrivate)
        {
            Destroy(transform.GetChild(3).gameObject);
        }
        transform.GetComponent<UI_Button>().SetOnClick(EnterRoomUI);
    }

    public void EnterRoomUI()
    {
        if (myroom.Info.IsPrivate)
        {
            EnterPasswordPopup popup = Managers.UI.LoadPopupPanel<EnterPasswordPopup>();
            popup.Init(myroom);
        }
        else
        {
            UIPacketHandler.WaitForPacket();
            Managers.Room.RequestEnterRoom(myroom.Info.RoomId,"",Managers.Player._myRoomPlayer.Name);
        }
    }
}
