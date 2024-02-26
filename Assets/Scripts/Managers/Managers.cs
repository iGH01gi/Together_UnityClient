using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; 
    static Managers Instance {get { Init(); return _instance; } } 
    
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();
    NetworkManager _network = new NetworkManager();
    PlayerManager _player = new PlayerManager();
    WebManager _web = new WebManager();
    InputManager _input = new InputManager();
    RoomManager _room = new RoomManager();
    DedicatedManager _dedicated = new DedicatedManager();
    ObjectManager _object = new ObjectManager();
    LogicManager _logic = new LogicManager();
    
    
    public static  ResourceManager Resource { get { return Instance._resource;} }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }
    public static NetworkManager Network { get { return Instance._network; } }
    public static PlayerManager Player { get { return Instance._player; } }
    public static InputManager Input { get { return Instance._input; } }
    public static RoomManager Room { get { return Instance._room; } }
    public static DedicatedManager Dedicated { get { return Instance._dedicated; } }
    public static ObjectManager Object { get { return Instance._object; } }
    public static LogicManager Logic { get { return Instance._logic; } }


    void Start()
    {
        Init();
        WebManager.Init();
        LogicManager.Init();
    }

    
    void Update()
    {
        _network.Update(); //받은 패킷 처리
        _logic.Update(); //게임 로직 업데이트(매 프레임마다 실행되어야하는)
    }

    private void FixedUpdate()
    {
        _logic.FixedUpdate(); //게임 로직 업데이트(게임로직 주기마다 실행되어야하는)
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
            _instance._sound.Init();
            _instance._data.Init();
            _instance._pool.Init();
            _instance._network.Init();
            _instance._ui.Init();
            _instance._input.Init();
        }
    }
    
    
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }

    private void OnApplicationQuit()
    {
        _network.OnQuitUnity();
    }
}
