using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInput : MonoBehaviour
{
    private GameObject currentChest;
    
    private void Start()
    {
        GetComponent<PlayerInput>().DeactivateInput();
    }
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Chest"))
        {
            if(!other.transform.parent.GetComponent<Chest>()._isOpened)
            {
                ChangeHighlightChest(other.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Chest" && currentChest.Equals(other.transform.parent.gameObject))
        {
            currentChest.GetComponent<Chest>().UnHighlightChest();
            currentChest = null;
            Managers.UI.ClosePopup();
        }
    }
    
    private void ChangeHighlightChest(GameObject newChest)
    {
        if (currentChest != null)
        {
            if(
            Vector3.Angle(transform.forward, currentChest.transform.position - transform.position)<
            Vector3.Angle(transform.forward, newChest.transform.position - transform.position))
            {
                return;
            }
            currentChest.GetComponent<Chest>().UnHighlightChest();
        }
        currentChest = newChest;
        currentChest.GetComponent<Chest>().HighlightChest();
    }
    
    
    
    void OnInteract(InputValue value)
    {
        if(currentChest != null)
        {
            Managers.Object.TryOpenChest(currentChest.GetComponent<Chest>()._chestId);
        }
    }
}
