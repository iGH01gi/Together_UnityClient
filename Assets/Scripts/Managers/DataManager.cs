using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using SimpleJSON;
using UnityEngine.InputSystem;
using File = System.IO.File;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}



public class DataManager
{
    Dictionary<Define.SaveFiles, string> fileNames;
    
    public void Init()
    {
        fileNames = new Dictionary<Define.SaveFiles, string>();
        //Define file names
        fileNames[Define.SaveFiles.KeyBinding] = "OverrideBindings.json";
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
	}

    public void SaveToJson(Define.SaveFiles fileType, string data)
    {
        File.WriteAllText(GetFilePath(fileType), data);
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

    string GetFilePath(Define.SaveFiles fileType)
    {
        return Application.persistentDataPath + fileNames[fileType];
    }
}
