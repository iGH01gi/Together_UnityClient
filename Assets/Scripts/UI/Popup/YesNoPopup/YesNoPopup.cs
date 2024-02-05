using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class YesNoPopup : UI_popup
{
    protected abstract void YesFunc();
    protected abstract void NoFunc();

    protected GameObject popup;
    
    protected void Init<T>() where T: YesNoPopup
    {
        transform.GetChild(0).GetComponent<UI_Button>().SetOnClick(ClosePopup);
        transform.GetChild(1).GetComponent<UI_Text>().SetString(typeof(T).Name);
        transform.GetChild(2).GetComponent<UI_Button>().SetOnClick(YesFunc);
        transform.GetChild(2).GetChild(0).GetComponent<UI_Text>().SetString("Yes");
        transform.GetChild(3).GetComponent<UI_Button>().SetOnClick(NoFunc);
        transform.GetChild(3).GetChild(0).GetComponent<UI_Text>().SetString("No");
    }
}
