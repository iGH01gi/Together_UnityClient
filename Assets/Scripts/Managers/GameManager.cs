using UnityEngine;
public class GameManager
{
    public ClientTimer _clientTimer;
    public static GameObject root;
    
    public void Init()
    {
        root = GameObject.Find("@Game");
        if (root == null)
        {
            root = new GameObject { name = "@Game" };
            Object.DontDestroyOnLoad(root);
        }
    }

    public void GameScene()
    {
        _clientTimer = Util.GetOrAddComponent<ClientTimer>(root);
    }

    #region 근처 폭탄마 소리 처리

    

    #endregion
}