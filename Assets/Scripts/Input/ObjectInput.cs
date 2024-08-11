using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInput : MonoBehaviour
{
    private GameObject _currentChest;
    private GameObject _currentCleanse;

    private void Start()
    {
        GetComponent<PlayerInput>().DeactivateInput();
    }
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (Managers.Game._isDay)
        {
            if(other.CompareTag("Chest") && !other.transform.parent.GetComponent<Chest>()._isOpened)
            {
                ChangeHighlightChest(other.transform.parent.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Cleanse") && other.transform.parent.GetComponent<Cleanse>()._isAvailable)
            {
                _currentCleanse = other.transform.parent.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Chest" && _currentChest.Equals(other.transform.parent.gameObject))
        {
            _currentChest.GetComponent<Chest>().UnHighlightChest();
            _currentChest = null;
            Managers.UI.ClosePopup();
        }
        else if (other.tag == "Cleanse")
        {
            if (Managers.Object._cleanseController._myPlayerCurrentCleanse != null)
            {
                Managers.Object._cleanseController.QuitCleansing(Managers.Object._cleanseController._myPlayerCurrentCleanse._cleanseId);
            }
            _currentCleanse = null;
        }
    }
    
    private void ChangeHighlightChest(GameObject newChest)
    {
        if (_currentChest != null)
        {
            if(
            Vector3.Angle(transform.forward, _currentChest.transform.position - transform.position)<
            Vector3.Angle(transform.forward, newChest.transform.position - transform.position))
            {
                return;
            }
            _currentChest.GetComponent<Chest>().UnHighlightChest();
        }
        _currentChest = newChest;
        _currentChest.GetComponent<Chest>().HighlightChest();
    }

    void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract");
        if (Managers.Game._isDay)
        {
            if (_currentChest != null)
            {
                Managers.Object._chestController.TryOpenChest(_currentChest.GetComponent<Chest>()._chestId);
            }
        }
        else
        {
            if (_currentCleanse != null)
            {
                if ((Managers.Object._cleanseController._myPlayerCurrentCleanse == null) && value.isPressed)
                {
                    Managers.Object._cleanseController.TryCleanse(_currentCleanse.GetComponent<Cleanse>()._cleanseId);
                }
                else if((Managers.Object._cleanseController._myPlayerCurrentCleanse != null) && value.isPressed)
                {
                    Managers.Object._cleanseController.QuitCleansing(_currentCleanse.GetComponent<Cleanse>()._cleanseId);
                }
            }
        }
    }
    
    void OnSkill(InputValue value)
    {
        Debug.Log("Skill Try");
        if (Managers.Player.IsMyDediPlayerKiller())
        {
            Managers.Game._myKillerSkill.TryUseSkill();
        }
    }
}
