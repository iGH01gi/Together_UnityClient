using UnityEngine;

public class Firework : MonoBehaviour,IItem
{
    //IItem 인터페이스 구현
    public int ItemID { get; set; }
    public int PlayerID { get; set; }
    public string EnglishName { get; set; }


    //이 아이템만의 속성
    public float FlightHeight { get; set; }

    public void Init(int itemId, int playerId, string englishName)
    {
        this.ItemID = itemId;
        this.PlayerID = playerId;
        this.EnglishName = englishName;
    }

    public void Init(int itemId, int playerId, string englishName, float flightHeight)
    {
        Init(itemId,playerId, englishName);
        FlightHeight = flightHeight;
    }

    public void Use()
    {
        Managers.Player.GetAnimator(PlayerID).SetTriggerByString(EnglishName);
        Debug.Log("Item Firework Use");
    }
    
    public void OnHold()
    {
        
    }
    
    public void OnHit()
    {
        
    }
}