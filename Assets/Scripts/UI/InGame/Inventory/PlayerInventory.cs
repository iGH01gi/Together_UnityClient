using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventory : MonoBehaviour
{
    private Color _unselect = Color.white;
    private Color _selectedSlot = new Color(255, 218, 218);
    private Color _selectedBorder = new Color(87, 87, 87);
    
    int _currentSlot = 0;
    
    void Start()
    {
        transform.GetComponent<HorizontalLayoutGroup>().spacing = Screen.width / 100;
    }
    
    public void ChangeSelected(int index)
    {
        //이전 슬롯과 현재 슬롯 색상 변경
        transform.GetChild(_currentSlot).transform.Find($"PaperBackground").GetComponent<Image>().color = _unselect;
        transform.GetChild(_currentSlot).transform.Find($"Border").GetComponent<Image>().color = _unselect;
        _currentSlot = index;
        transform.GetChild(_currentSlot).transform.Find($"PaperBackground").GetComponent<Image>().color = _selectedSlot;
        transform.GetChild(_currentSlot).transform.Find($"Border").GetComponent<Image>().color = _selectedBorder;
    }
}
