using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager
{
    public static GameObject root;
    public bool _isDay;
    
    public ClientTimer _clientTimer;
    public ClientGauge _clientGauge;

    public static int _dokidokiStart = 20;
    public static int _dokidokiExtreme = 10;

    //Managers Init과 함께 불리는 Init
    public void Init()
    {
        root = GameObject.Find("@Game");
        if (root == null)
        {
            root = new GameObject { name = "@Game" };
            Object.DontDestroyOnLoad(root);
        }
    }

    //게임 씬으로 넘어갈 시에 호출되는 '사실상 init'인 함수
    public void GameScene()
    {
        _clientTimer = Util.GetOrAddComponent<ClientTimer>(root);
        _clientGauge = Util.GetOrAddComponent<ClientGauge>(root);
        _playKillerSound = Util.GetOrAddComponent<PlayKillerSound>(root);
    }

    private void WhenChangeDayNight(float timeToSet)
    {
        _clientTimer.Init(timeToSet);
        Managers.UI.GetComponentInSceneUI<InGameUI>().ChangeTimerPrefab();
        Managers.UI.GetComponentInSceneUI<InGameUI>().ToggleGaugePrefab(!_isDay);
    }

    public void ChangeToDay(float timeToSet)
    {
        _isDay = true;
        WhenChangeDayNight(timeToSet);
    }
    
    public void ChangeToNight(float timeToSet)
    {
        _isDay = false;
        WhenChangeDayNight(timeToSet);
        _clientGauge.Init();
    }
    

    #region 근처 킬러 소리 처리
    private PlayKillerSound _playKillerSound;

    public void PlayKillerSound()
    {
        if(Managers.Player.GetKillerId() == -1)
            return;
        
        float distance = Vector3.Distance(Managers.Player._myDediPlayer.transform.position,
            Managers.Player.GetKillerGameObject().transform.position);
        
        _playKillerSound.CheckPlayKillerSound(distance< _dokidokiStart, distance < _dokidokiExtreme);

    }

    #endregion

    #region 게이지 관련
    
    
    
    #endregion

}