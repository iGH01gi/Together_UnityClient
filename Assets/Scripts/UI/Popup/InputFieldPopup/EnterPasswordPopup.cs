using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPasswordPopup : InputFieldPopup
{
    void Start()
    {
        base.Init<EnterPasswordPopup>();
    }

    protected override void OnButtonClick()
    {
        UIPacketHandler.WaitForPacket();
        Managers.Room.RequestEnterRoom(gameRoom.Info.RoomId,transform.GetChild(2).GetComponent<UI_InputField>().GetInputText(),Managers.Player._myPlayer.PlayerId.ToString());
    }
}
