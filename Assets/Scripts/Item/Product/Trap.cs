using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IItem
{
    //IItem 인터페이스 구현
    public int itemID { get; set; }
    
    public float TrapDuration { get; set; }
    public float TrapRadius { get; set; }
    public float StunDuration { get; set; }
    
    public void Init(int itemId)
    {
        this.itemID = itemId;
    }
    
    public void Init(int itemId, float trapDuration, float trapRadius, float stunDuration)
    {
        Init(itemId);
        TrapDuration = trapDuration;
        TrapRadius = trapRadius;
        StunDuration = stunDuration;
    }
    
    public void Use()
    {
        Debug.Log("Item Trap Use");
    }

    public void OnHit()
    {
        
    }
}
