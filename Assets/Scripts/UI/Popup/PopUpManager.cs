using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PopUpManager
{
    public static void LoadYesNoPopup(string description, Action yesFunc, Action noFunc)
    {
        GameObject go = Managers.UI.LoadPopupPanel(Define.Popup.YesNo.ToString());
        go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Managers.UI.CloseTopPopup);
        go.transform.GetChild(1).GetComponent<LocalizeStringEvent>().StringReference
            .SetReference("StringTable", description);
        go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { yesFunc(); });
        go.transform.GetChild(2).GetChild(0).GetComponent<LocalizeStringEvent>().StringReference
            .SetReference("StringTable", "Yes");
        go.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { noFunc(); });
        go.transform.GetChild(3).GetChild(0).GetComponent<LocalizeStringEvent>().StringReference
            .SetReference("StringTable", "No");
    }
}
