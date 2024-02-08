using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputField : UI_subitem
{
    public void SetPlaceHolder(string reference)
    {
        transform.GetChild(0).GetChild(0).GetComponent<UI_Text>().SetString(reference);
    }

    public string GetInputText()
    {
        return gameObject.GetComponent<InputField>().text;
    }

    public void SetInteractable(bool interactable)
    {
        gameObject.GetComponent<InputField>().interactable = interactable;
    }
}
