using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager
{
    public static GameObject root;
    public bool _isDay;
    
    public ClientTimer _clientTimer;
    public ClientGauge _clientGauge;

    private float _dokidokiStart = 20;
    private float _dokidokiClose = 10;
    private float _dokidokiExtreme = 4;

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
        _playKillerSound.Init(_dokidokiStart, _dokidokiClose, _dokidokiExtreme);
    }

    private void WhenChangeDayNight(float timeToSet)
    {
        _clientTimer.Init(timeToSet);
        Managers.UI.GetComponentInSceneUI<InGameUI>().ChangeDayNightUIPrefab();
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
        if(_playKillerSound == null)
            return;
        float distance = Vector3.Distance(Managers.Player._myDediPlayer.transform.position,
            Managers.Player.GetKillerGameObject().transform.position);
        _playKillerSound.CheckPlayKillerSound(distance);

    }

    #endregion

    #region 게이지 관련
    
    
    
    #endregion

}