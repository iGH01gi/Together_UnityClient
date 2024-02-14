using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIInputHandler : MonoBehaviour
{
    void OnCancel()
    {
        if (Managers.UI.PopupActive())
        {
            Managers.UI.ClosePopup();
        }
    }
}