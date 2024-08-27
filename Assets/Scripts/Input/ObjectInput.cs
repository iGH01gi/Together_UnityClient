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
            //상자 트리거 처리
            if(other.CompareTag("Chest") && !other.transform.parent.GetComponent<Chest>()._isOpened)
            {
                ChangeHighlightChest(other.transform.parent.gameObject);
            }
        }
        else
        {
            //킬러일 때 생존자 공격 트리거 처리
            if (Managers.Player.IsMyDediPlayerKiller())
            {
                if (other.CompareTag("Player") && Managers.Player._myDediPlayer.transform.GetComponentInChildren<PlayerAnimController>().IsAttacking())
                {
                    Debug.Log("Attacked Player with ID: "+other.transform.parent.GetComponent<OtherDediPlayer>().PlayerId);
                }
            }
            else
            {
                //생존자일 때 클렌즈 처리
                if (other.CompareTag("Cleanse") && other.transform.parent.GetComponent<Cleanse>()._isAvailable)
                {
                    _currentCleanse = other.transform.parent.gameObject;
                }
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
            QuitCleansing();
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
            if (!Managers.Player.IsMyDediPlayerKiller())
            {
                if (_currentCleanse != null)
                {
                    if ((Managers.Object._cleanseController._myPlayerCurrentCleanse == null) && value.isPressed)
                    {
                        Managers.Object._cleanseController.TryCleanse(
                            _currentCleanse.GetComponent<Cleanse>()._cleanseId);
                    }
                    else if (value.isPressed)
                    {
                        QuitCleansing();
                    }
                }
            }
            else
            {
                Managers.Killer.BaseAttack(Managers.Player._myDediPlayerId);
            }
        }
    }

    public void QuitCleansing()
    {
        if ((Managers.Object._cleanseController._myPlayerCurrentCleanse != null))
        {
            Managers.Object._cleanseController.QuitCleansing(_currentCleanse.GetComponent<Cleanse>()._cleanseId);
        }
    }

    public void Clear()
    {
        _currentChest = null;
        _currentChest = null;
    }
}
