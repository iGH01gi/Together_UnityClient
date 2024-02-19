using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public Define.Scene SceneType = Define.Scene.Lobby;
    
	public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneType = type;
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        // Implement Scene clear if necessary
    }
}
