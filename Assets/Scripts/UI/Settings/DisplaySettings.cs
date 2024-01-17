using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySettings : MonoBehaviour
{
    private List<string> resolutions;
    void Start()
    {
        Screen.resolutions.ToList().ForEach(x=>
        {
            resolutions.Add(ResolutionToString(x));
        });
        
        UIUtils.BindFieldToUIToggle(Managers.Data.Player,"isFullScreen",OnFullScreenChanged,transform);
        UIUtils.BindFieldToUIDropdown(Managers.Data.Player,"QualityIndex",OnQualityIndexChanged,transform,Util.EnumToString<Define.DisplayQuality>());
        UIUtils.BindFieldToUIDropdown(Managers.Data.Player,"MyResolution",OnResolutionChanged,transform,resolutions);
    }

    void OnFullScreenChanged(GameObject go)
    {
        bool value = go.transform.GetChild(1).GetComponent<Toggle>().isOn;
        Managers.Data.Player.isFullScreen = value;
        Screen.fullScreen = value;
    }

    void OnQualityIndexChanged(GameObject go)
    {
        int value = go.transform.GetChild(1).GetComponent<TMP_Dropdown>().value;
        go.transform.GetChild(1).GetComponent<TMP_Dropdown>().RefreshShownValue();
        SetQualityLevel(Managers.Data.Player.DisplayQuality);
    }

    public static void SetQualityLevel(Define.DisplayQuality value)
    {
        switch (value)
        {
            case Define.DisplayQuality.Low:
                QualitySettings.SetQualityLevel(0, true);
                break;
            case Define.DisplayQuality.Medium:
                QualitySettings.SetQualityLevel(3, true);
                break;
            case Define.DisplayQuality.High:
                QualitySettings.SetQualityLevel(5, true);
                break;
        }
    }

    void OnResolutionChanged(GameObject go)
    {
        int value = go.transform.GetChild(1).GetComponent<TMP_Dropdown>().value;
        Resolution myRes = Screen.resolutions[value];
        go.transform.GetChild(1).GetComponent<TMP_Dropdown>().RefreshShownValue();
        Managers.Data.Player.MyResolution = myRes;
        Debug.Log(Managers.Data.Player.MyResolution);
        Screen.SetResolution(myRes.width,myRes.height,Managers.Data.Player.isFullScreen);
    }

    public static string ResolutionToString(Resolution resolution)
    {
        return resolution.width +"x"+resolution.height;
    }

    public void SavePlayerData()
    {
        Managers.Data.SavePlayerData();
    }
}
