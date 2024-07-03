using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_popup : UI_base
{
    public enum PopupType
    {
        YesNoPopup,
        Settings,
        CreateRoom,
        Alert,
        ProgressPopup,
        SettingsPopup,
        WairForSecondsPopup
    }
    
    protected void ClosePopup()
    {
        Managers.UI.ClosePopup(gameObject);
    }
}
