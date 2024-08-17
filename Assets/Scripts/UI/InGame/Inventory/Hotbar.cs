using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    //Hotbar의 child는 5개의 슬롯만 있다는 가정 하에 구현
    int _slotCount = 5;
    private int _currentSlot;

    private Color _unselectColor = Color.white;
    private Color _selectedColor = Color.black;

    private void Start()
    {
        _currentSlot = 0;
        ChangeSelected(0);
    }

    public void AddToSlot(int slot)
    {
        
    }

    public void RemoveFromSlot(int slot)
    {
        
    }

    public void ChangeSelected(int index)
    {
        //인덱스 체크
        if (index >= _slotCount || index < 0)
        {
            Debug.Log("Hotbar Slot Index Out of Range");
            return;
        }
        //이전 슬롯과 현재 슬롯 색상 변경
        transform.GetChild(_currentSlot).transform.Find("Paint").GetComponent<Image>().color = _unselectColor;
        _currentSlot = index;
        transform.GetChild(_currentSlot).transform.Find("Paint").GetComponent<Image>().color = _selectedColor;
    }
}
