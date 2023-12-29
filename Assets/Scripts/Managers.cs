using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; 
    static Managers Instance {get { Init(); return _instance; } } 
    
    ResourceManager _resource = new ResourceManager();
    
    /*
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();
    SoundManager _sound = new SoundManager();
    */
    
    public static  ResourceManager Resource { get { return Instance._resource;} }
    
    
    /*
     public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }
    
    public static SoundManager Sound { get { return Instance._sound; } }
    
    */
    
    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
            /*
            _instance._data.Init();
            _instance._pool.Init();
            _instance._sound.Init();
            */
        }
    }
    
    
    public static void Clear()
    {
        //Sound.Clear();
        //Scene.Clear();
        //UI.Clear();
        //Pool.Clear();
    }
    
}
