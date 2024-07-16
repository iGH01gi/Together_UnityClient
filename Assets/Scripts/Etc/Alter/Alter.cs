using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour
{
    public string _alterID;
    public bool _isAvailable = true;
    
    private float _currentCleanseTime = 0f;
    private float _timeToCleanse;
    
    private CapsuleCollider _trigger;

    void Start()
    {
        _trigger = transform.Find("TriggerCapsule").GetComponent<CapsuleCollider>();
        //_trigger.enabled = false;
        _trigger.enabled = true;
        _timeToCleanse = Managers.Object._alterController._timeToCleanse;
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

    public void CurrentlyCleansing(float cleanseTime)
    {
        _currentCleanseTime = cleanseTime;
        if(_currentCleanseTime>=_timeToCleanse)
        {
            Managers.Object._alterController.CleanseSuccess();
            Managers.UI.ClosePopup();
            _isAvailable = false;
            _trigger.enabled = false;
            //testcode
            StartCoroutine(Sdd());
        }
    }

    IEnumerator Sdd()
    {
        yield return new WaitForSeconds(3f);
        _isAvailable = true;
        _trigger.enabled = true;
    }

    public void QuitCleansing()
    {
        _currentCleanseTime = 0f;
        Managers.UI.ClosePopup();
        Managers.Object._alterController.CleanseQuit();
    }
}
