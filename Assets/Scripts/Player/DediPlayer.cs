using UnityEngine;

public class DediPlayer : MonoBehaviour
{
    //현재 데디서버의 playerId는 독자적인 값으로 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public bool IsMyPlayer { get; set; }
    
    
    public void CopyFrom(DediPlayer dediPlayer)
    {
        PlayerId = dediPlayer.PlayerId;
        Name = dediPlayer.Name;
        IsMyPlayer = dediPlayer.IsMyPlayer;
    }
}