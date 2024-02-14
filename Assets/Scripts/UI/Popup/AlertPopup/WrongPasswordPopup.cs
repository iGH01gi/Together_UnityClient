using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongPasswordPopup : AlertPopup
{
    void Start()
    {
        UIPacketHandler.OnReceivePacket();
        Init<WrongPasswordPopup>();
    }
}
