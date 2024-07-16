using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInput : MonoBehaviour
{
    private GameObject _currentChest;
    private GameObject _currentAlter;

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
            if (other.CompareTag("Alter") && other.GetComponent<Alter>().isAvailable)
            {
                //TODO: SHOW ALTER AVAILABLE POPUP
                _currentAlter = other.gameObject;
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
        else if (other.tag == "Alter")
        {
            _currentAlter = null;
            Managers.UI.ClosePopup();
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
        if (Managers.Game._isDay)
        {
            if (_currentChest != null)
            {
                Managers.Object._chestController.TryOpenChest(_currentChest.GetComponent<Chest>()._chestId);
            }
        }
        else
        {
            if (_currentAlter != null)
            {
                Managers.Object._alterController.TryCleanse(_currentAlter.GetComponent<Alter>().GetInstanceID());
            }
        }
    }
}
