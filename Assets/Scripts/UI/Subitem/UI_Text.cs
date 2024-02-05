using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Text : UI_subitem
{
    public void SetString(string description)
    {
        BindLocalizedString(gameObject,description);
    }
}
