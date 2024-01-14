using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using SimpleJSON;
using UnityEngine.InputSystem;
using File = System.IO.File;

public class DataManager
{
    Dictionary<Define.SaveFiles, string> fileNames;
    
    public void Init()
    {
        fileNames = new Dictionary<Define.SaveFiles, string>();
        //Define file names
        fileNames[Define.SaveFiles.Display] = "DisplaySettings.json";
        fileNames[Define.SaveFiles.Sound] = "SoundSettings.json";
        fileNames[Define.SaveFiles.Control] = "ControlSettings.json";
        fileNames[Define.SaveFiles.KeyBinding] = "OverrideBindings.json";
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
        Debug.Log(GetFilePath(fileType));
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
}
