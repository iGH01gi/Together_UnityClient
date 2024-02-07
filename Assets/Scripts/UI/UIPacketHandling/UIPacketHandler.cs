using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPacketHandler
{
    public virtual void WaitForPacket()
    {
        Managers.Input.DisableInput();
        Managers.UI.LoadPopupPanel<ProgressPanel>();
    }

    public virtual void OnReceivePacket()
    {
        Managers.UI.CloseTopPopup();
        Managers.Input.EnableInput();
    }
}
