using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_scene : UI_base
{
    public enum SceneUIType
    {
        MainMenuUI,
        LobbyUI,
        RoomUI,
    }

    public static void InstantiateSceneUI(SceneUIType sceneUIType)
    {
        Managers.UI.LoadScenePanel(sceneUIType.ToString());
    }

    protected void InitButtons<T>(bool initiate = false) where T : Enum
    {
        foreach (string button in Enum.GetNames(typeof(T)))
        {
            GameObject go;
            if (initiate)
            {
                go = Managers.Resource.Instantiate("UI/Subitem/UI_Button", transform);
                go.name = button;
            }
            else
            {
                go = gameObject.transform.Find(button).gameObject;
            }

            if (go.transform.childCount == 1 && go.transform.GetChild(0).name == "LocalizationText")
            {
                go.transform.GetChild(0).GetComponent<UI_Text>().SetString(button);
            }

            go.GetComponent<UI_Button>().SetOnClick(funcToRun(button));
        }
    }

    protected Action funcToRun(string func)
    {
        return delegate { Invoke(func, 0f); };
    }
}