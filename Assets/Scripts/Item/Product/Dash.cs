using System;
using UnityEngine;

public class Dash : MonoBehaviour, IItem
{
    //IItem 인터페이스 구현
    public int itemID { get; set; }

    //이 아이템만의 속성
    public float DashDistance { get; set; }
    
    public void Init(int itemId)
    {
        this.itemID = itemId;
    }
    
    public void Init(int itemId, float dashDistance)
    {
        Init(itemId);
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
    
    public void OnHit()
    {
    }
}