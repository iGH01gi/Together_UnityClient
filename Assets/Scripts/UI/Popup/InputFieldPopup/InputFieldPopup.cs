using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputFieldPopup : UI_popup
{
    public GameRoom gameRoom { get; set; }
    protected abstract void OnButtonClick();
    protected void Init<T>() where T: InputFieldPopup 
    {
        transform.GetChild(0).GetComponent<UI_Button>().SetOnClick(ClosePopup);
        transform.GetChild(1).GetComponent<UI_Text>().SetString(typeof(T).Name);
        transform.GetChild(3).GetChild(0).GetComponent<UI_Button>().SetOnClick(OnButtonClick);
    }
}
