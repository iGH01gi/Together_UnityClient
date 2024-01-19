using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using SimpleJSON;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using File = System.IO.File;

[System.Serializable]
public struct ResolutionStruct
{
    public int width;
    public int height;

    public string ToDisplayString()
    {
        return this.width +"x"+this.height;
    }
}

[System.Serializable]
public class PlayerData
{
    public Locale currentLocale;
    public float MouseSensitivity;
    public bool isFullScreen;
    public Define.DisplayQuality DisplayQuality;
    public ResolutionStruct MyResolution;

    public PlayerData()
    {
        MyResolution.width = Screen.currentResolution.width;
        MyResolution.height = Screen.currentResolution.height;
        currentLocale = LocalizationSettings.AvailableLocales.Locales[0];
        MouseSensitivity = 100f;
        isFullScreen = true;
        DisplayQuality = Define.DisplayQuality.High;
    }
}

public class DataManager
{
    Dictionary<Define.SaveFiles, string> fileNames;
    public static PlayerData _playerData;
    public PlayerData Player { get { return _playerData; } }

    public void Init()
    {
        fileNames = new Dictionary<Define.SaveFiles, string>();

        //Define file names
        fileNames[Define.SaveFiles.Player] = "PlayerData.json";
        fileNames[Define.SaveFiles.Display] = "DisplaySettings.json";
        fileNames[Define.SaveFiles.Sound] = "SoundSettings.json";
        fileNames[Define.SaveFiles.Control] = "ControlSettings.json";
        fileNames[Define.SaveFiles.KeyBinding] = "OverrideBindings.json";
        
        _playerData = new PlayerData();
        _playerData = Managers.Data.LoadFromJson<PlayerData>(Define.SaveFiles.Player, _playerData);
        Screen.fullScreen = _playerData.isFullScreen;
        Screen.SetResolution(_playerData.MyResolution.width,_playerData.MyResolution.height,_playerData.isFullScreen);
        DisplaySettings.SetQualityLevel(_playerData.DisplayQuality);
    }

    public void SaveToJson(Define.SaveFiles fileType, string data)
    {
        File.WriteAllText(GetFilePath(fileType), data);
    }

    public void SaveToJson<T>(Define.SaveFiles fileType, T data)
    {
        File.WriteAllText(GetFilePath(fileType),JsonUtility.ToJson(data));
    }

    public string LoadFromJson(Define.SaveFiles fileType)
    {
        if (File.Exists(GetFilePath(fileType)))
        {
            return File.ReadAllText(GetFilePath(fileType));
        }
        else
        {
            return null;
        }
    }
    
    public T LoadFromJson<T>(Define.SaveFiles fileType, T classToLoad)
    {
        if (File.Exists(GetFilePath(fileType)))
        {
            string str = File.ReadAllText(GetFilePath(fileType));
            return JsonUtility.FromJson<T>(str);
        }
        else
        {
            return classToLoad;
        }
    }

    string GetFilePath(Define.SaveFiles fileType)
    {
        return Application.persistentDataPath +"\\"+ fileNames[fileType];
    }
    
    public void SavePlayerData()
    {
        SaveToJson<PlayerData>(Define.SaveFiles.Player,_playerData);
    }
}