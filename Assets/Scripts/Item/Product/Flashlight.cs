using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour, IItem
{
    public int itemID { get; set; }
    public float BlindDuration {get; set;}
    public float FlashlightDistance {get; set;}
    public float FlashlightAngle {get; set;}
    public float FlashlightAvailableTime {get; set;}
    public float FlashlightTimeRequired {get; set;}
    
    public void Init(int itemId)
    {
        this.itemID = itemId;
    }
    
    public void Init(int itemId, float blindDuration, float flashlightDistance, float flashlightAngle, float flashlightAvailableTime, float flashlightTimeRequired)
    {
        Init(itemId);
        BlindDuration = blindDuration;
        FlashlightDistance = flashlightDistance;
        FlashlightAngle = flashlightAngle;
        FlashlightAvailableTime = flashlightAvailableTime;
        FlashlightTimeRequired = flashlightTimeRequired;
    }
    
    public void Use()
    {
        Debug.Log("Item Trap Use");
    }

    public void OnHit()
    {
        
    }
}
