using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    protected void InitButtons<T>(GameObject go,bool initiate = false) where T : Enum
    {
        foreach (string buttonName in Enum.GetNames(typeof(T)))
        {
            GameObject buttonGO;
            if (initiate)
            {
                buttonGO = Managers.Resource.Instantiate("UI/Subitem/UI_Button", go.transform);
                buttonGO.name = buttonName;
            }
            else
            {
                buttonGO = go.transform.Find(buttonName).gameObject;
            }

            GameObject localText = Util.FindChild(buttonGO, "LocalizationText", true);

            if (localText != null)
            {
                localText.GetComponent<UI_Text>().SetString(buttonName);
            }

            buttonGO.GetComponent<UI_Button>().SetOnClick(funcToRun(buttonName));
        }
    }

    protected Action funcToRun(string func)
    {
        return delegate { Invoke(func, 0f); };
    }
}