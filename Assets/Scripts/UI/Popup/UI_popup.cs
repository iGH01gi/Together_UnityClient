using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_popup : UI_base
{
    public enum PopupType
    {
        YesNoPopup,
        Settings
    }
    
    protected void ClosePopup()
    {
        Managers.UI.CloseTopPopup();
    }
}
