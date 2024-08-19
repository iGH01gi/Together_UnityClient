using System;
using UnityEngine;

public class Dash : ItemProduct
{ 
    public float DashDistance { get; set; }
    
    public Dash(int id, int price, string englishName, string koreanName, string englishDescription,
        string koreanDescription, float dashDistance)
    {
        base.Init(id, price, englishName, koreanName, englishDescription, koreanDescription);
        DashDistance = dashDistance;
    }

    public override void Use()
    {
        Debug.Log("Item Dash Use");
    }
    
    public override void OnHold()
    {
        //예상 대시 경로, 또는 도착지점을 보여준다던지 하는 기능을 실행
    }
}