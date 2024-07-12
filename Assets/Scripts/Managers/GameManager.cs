using UnityEngine;
using UnityEngine.Audio;

public class GameManager
{
    public static GameObject root;
    
    public ClientTimer _clientTimer;
    private PlayBombSound _playBombSound;

    public static int _dokidokiStart = 20;
    public static int _dokidokiExtreme = 10;
    public  bool gamestart = false;
    
    
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
        _playBombSound = Util.GetOrAddComponent<PlayBombSound>(root);
    }

    #region 근처 킬러 소리 처리

    public void PlayBombSound()
    {
        //if(Managers.Player._otherDediPlayers.Count == 0) return;
        
        if(!gamestart) return;

        float distance = Vector3.Distance(Managers.Player._myDediPlayer.transform.position,
            Managers.Object._chestList[0].transform.position);
        
        _playBombSound.CheckPlayBombSound(distance< _dokidokiStart, distance < _dokidokiExtreme);

        //if bomb player is within _dokidokiDistance, play sound
        /*_playBombSound.CheckPlayBombSound((Vector3.Distance(Managers.Player._myDediPlayer.transform.position,
        Managers.Player._otherDediPlayers..transform.position) < _dokidokiDistance));*/


    }

    #endregion
}