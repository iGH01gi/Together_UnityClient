using UnityEngine;

public class Firework : MonoBehaviour,IItem
{
    public int itemID;

    //이 아이템만의 속성
    public float FlightHeight { get; set; }

    public void Init(int itemID, float flightHeight = 0)
    {
        this.itemID = itemID;
        FlightHeight = flightHeight;
    }

    public void Use()
    {
        Debug.Log("Item Firework Use");
    }
    
    public void OnHold()
    {
        
    }
}