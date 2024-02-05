using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class UI_subitem : MonoBehaviour
{
    protected void BindLocalizedString(GameObject go, string description)
    {
        go.GetComponent<LocalizeStringEvent>().StringReference
            .SetReference("StringTable", description);
    }
}
