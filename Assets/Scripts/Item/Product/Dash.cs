using System;
using UnityEngine;

public class Dash : MonoBehaviour, IItem
{
    public int itemID;
    public float DashDistance { get; set; }
    
    public void Init(int itemID, float dashDistance = 0)
    {
        this.itemID = itemID;
        DashDistance = dashDistance;
    }


    public void Use()
    {
        Debug.Log("Item Dash Use");
    }
    
    public void OnHold()
    {
        //예상 대시 경로, 또는 도착지점을 보여준다던지 하는 기능을 실행
    }
}