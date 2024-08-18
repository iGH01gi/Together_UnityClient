using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    //Hotbar의 child는 5개의 슬롯만 있다는 가정 하에 구현
    Transform _currentSlot;
    private Color _selectedColor = Color.white;
    private Color _unselectColor= Color.black;
    
    private void Start()
    {
        _currentSlot = GetSlot(0);
        ChangeSelected(0);
    }

    public void AddToSlot(int slot, int itemID)
    {
        if (BadIndexCheck(slot))
        {
            Debug.Log("Hotbar Slot Index Out of Range");
            return;
        }
        GetSlot(slot).GetComponentInChildren<InventorySlot>().Init(slot);
    }

    public void RemoveFromSlot(int slot)
    {
        if (BadIndexCheck(slot))
        {
            Debug.Log("Hotbar Slot Index Out of Range");
            return;
        }
        GetSlot(slot).GetComponentInChildren<InventorySlot>().ClearSlot();
    }

    public void ChangeSelected(int index)
    {
        //인덱스 체크
        if (BadIndexCheck(index))
        {
            Debug.Log("Hotbar Slot Index Out of Range");
            return;
        }
        //이전 슬롯과 현재 슬롯 색상 변경
        _currentSlot.Find("Paint").GetComponent<Image>().color = _unselectColor;
        _currentSlot = GetSlot(index);
        _currentSlot.Find("Paint").GetComponent<Image>().color = _selectedColor;
        Debug.Log("Current Slot : " + index);
    }

    public int CurrentSelectedItemID()
    {
        return _currentSlot.GetComponentInChildren<InventorySlot>().itemID;
    }
    
    public void ClearSlot()
    {
        //TODO:만약 들고 있는 아이템이 있다면 아이템을 해제
        for (int i = 0; i < transform.childCount; i++)
        {
            GetSlot(i).GetComponentInChildren<InventorySlot>().ClearSlot();
        }
    }
    
    private bool BadIndexCheck(int index)
    {
        return index >= transform.childCount || index < 0;
    }
    
    private Transform GetSlot(int index)
    {
        return transform.GetChild(index);
    }
}
