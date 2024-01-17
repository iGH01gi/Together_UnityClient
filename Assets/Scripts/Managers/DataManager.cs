using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using SimpleJSON;
using UnityEngine.InputSystem;
using File = System.IO.File;


[System.Serializable]
public class PlayerData
{
    public float MouseSensitivity;
    public bool isFullScreen;
    
    public PlayerData()
    {
        MouseSensitivity = 100f;
        isFullScreen = true;
    }
}

public class DataManager
{
    Dictionary<Define.SaveFiles, string> fileNames;
    public static PlayerData _playerData;
    
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
        return Application.persistentDataPath + fileNames[fileType];
    }
    
    public PlayerData Player { get { return _playerData; } }
}