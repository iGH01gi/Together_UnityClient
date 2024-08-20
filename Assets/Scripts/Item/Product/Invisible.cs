using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour, IItem
{
    public int itemID { get; set; }
    public float InvisibleSeconds { get; set; }
    
    public void Init(int itemId)
    {
        this.itemID = itemId;
    }
    
    public void Init(int itemId, float invisibleSeconds)
    {
        Init(itemId);
        InvisibleSeconds = invisibleSeconds;
    }
    
    public void Use()
    {
        Debug.Log("Item Trap Use");
    }

    public void OnHit()
    {
        
    }
}
