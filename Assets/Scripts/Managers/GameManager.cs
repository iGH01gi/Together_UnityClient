using UnityEngine;
public class GameManager
{
    public static GameObject root;
    public ClientTimer _clientTimer;
    public void Init()
    {
        root = GameObject.Find("@Game");
        if (root == null)
        {
            root = new GameObject { name = "@Game" };
            Object.DontDestroyOnLoad(root);
            
            _clientTimer = root.AddComponent<ClientTimer>();
        }
    }

    #region 근처 폭탄마 소리 처리

    

    #endregion
}