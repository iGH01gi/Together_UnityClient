using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour
{
    public string _alterID;
    public bool _isAvailable = true;
    
    private CapsuleCollider _trigger;

    void Start()
    {
        _trigger = transform.Find("TriggerCapsule").GetComponent<CapsuleCollider>();
    }

    public void InitAlter(int alterID)
    {
        _alterID = alterID.ToString();
        _isAvailable = true;
        if(_trigger == null)
            _trigger = transform.Find("TriggerCapsule").GetComponent<CapsuleCollider>();
        _trigger.enabled = true;
    }

    public void AlterNowAvailable()
    {
        //TODO: ALTER AVAILABLE
    }

    public void AlterNowUnavailable()
    {
        //TODO: ALTER UNAVAILABLE
    }
}
