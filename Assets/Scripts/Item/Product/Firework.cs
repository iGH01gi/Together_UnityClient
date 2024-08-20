using UnityEngine;

public class Firework : MonoBehaviour,IItem
{
    //IItem 인터페이스 구현
    public int itemID { get; set; }

    //이 아이템만의 속성
    public float FlightHeight { get; set; }

    public void Init(int itemId)
    {
        this.itemID = itemId;
    }
    public void Init(int itemId, float flightHeight)
    {
        Init(itemId);
        FlightHeight = flightHeight;
    }

    public void Use()
    {
        Debug.Log("Item Firework Use");
    }
    
    public void OnHold()
    {
        
    }
    
    public void OnHit()
    {
        
    }
}