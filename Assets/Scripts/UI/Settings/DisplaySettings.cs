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
        resolutions = new List<string>();
        Screen.resolutions.ToList().ForEach(x=>
        {
            resolutions.Add(ResolutionToString(x));
        });
        
        UIUtils.BindFieldToUIToggle(Managers.Data.Player,"isFullScreen",OnFullScreenChanged,transform);
        UIUtils.BindFieldToUIDropdown(Managers.Data.Player,"DisplayQuality",OnQualityIndexChanged,transform,Util.EnumToString<Define.DisplayQuality>());
        UIUtils.BindFieldToUIDropdown(Managers.Data.Player.MyResolution.ToDisplayString(),"MyResolution",OnResolutionChanged,transform,resolutions);
    }

    void OnFullScreenChanged(GameObject go)
    {
        bool value = go.transform.GetChild(1).GetComponent<Toggle>().isOn;
        Managers.Data.Player.isFullScreen = value;
        Screen.fullScreen = value;
    }

    void OnQualityIndexChanged(TMP_Dropdown dropdown)
    {
        int value = dropdown.value;
        dropdown.RefreshShownValue();
        Managers.Data.Player.DisplayQuality = Util.GetEnumByIndex(Managers.Data.Player.DisplayQuality, value);
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

    void OnResolutionChanged(TMP_Dropdown dropdown)
    {
        dropdown.RefreshShownValue();
        int value = dropdown.value;
        Resolution myRes = Screen.resolutions[value];
        Managers.Data.Player.MyResolution.width = myRes.width;
        Managers.Data.Player.MyResolution.height = myRes.height;
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
