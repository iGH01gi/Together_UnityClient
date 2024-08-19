using UnityEngine;

public sealed class Firework : IItem
{
    //이 아이템만의 속성
    public float FlightHeight { get; set; }

    public void Init(int id, int price, string englishName, string koreanName, string englishDescription,
        string koreanDescription, float flightHeight)
    {
        base.Init(id, price, englishName, koreanName, englishDescription, koreanDescription);
        FlightHeight = flightHeight;
    }

    public override void Use()
    {
        Debug.Log("Item Firework Use");
    }
    
    public override void OnHold()
    {
        
    }
}