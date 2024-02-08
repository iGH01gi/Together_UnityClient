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

    public void SetChildString(string str, int childindex = 0)
    {
        transform.GetChild(childindex).GetComponent<UI_Text>().SetString(str);
    }
}
