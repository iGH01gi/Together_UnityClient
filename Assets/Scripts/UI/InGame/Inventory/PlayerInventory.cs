using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventory : MonoBehaviour
{
    static int _slotsInLine = 4;
    static int _initialLines = 3;
    string _viewContentPath = "Scroll View/Viewport/Content";
    string _linePath = "UI/Inventory/InventoryParts/InventoryLine";

    List<GameObject> _lines = new List<GameObject>(); //라인들의 게임 오브젝트
    private List<bool[]> _slotUsed = new List<bool[]>(); // 슬롯 사용여부
    Dictionary<int,int> _address = new Dictionary<int, int>(); //key: 아이템Id, value: 아이템 위치

    void Start()
    {
        for(int i =0;i<_initialLines;i++)
        {
            MakeNewLine();
        }
    }

    void MakeNewLine()
    {
        GameObject cur = Managers.Resource.Instantiate(_linePath, transform.Find(_viewContentPath));
        _lines.Add(cur);
        cur.GetComponent<HorizontalLayoutGroup>().spacing = Screen.width / 300;
        _slotUsed.Add(new bool[5]);
    }

    public void AddNewItem(int itemId)
    {
        //비어있는 슬롯이 있을 경우
        for(int i=0;i<_slotUsed.Count;i++)
        {
            for(int j=0;j<_slotUsed[i].Length;j++)
            {
                if (!_slotUsed[i][j])
                {
                    _address.Add(itemId, i * _slotsInLine + j);
                    _slotUsed[i][j] = true;
                    _lines[i].transform.GetChild(j).GetComponent<InventorySlot>().Init(itemId);
                    return;
                }
            }
        }
        //모든 슬롯이 차있을 경우
        _address.Add(itemId, _slotUsed.Count * _slotsInLine);
        MakeNewLine();
        _slotUsed[_slotUsed.Count - 1][0] = true;
        _lines[_lines.Count - 1].transform.GetChild(0).GetComponent<InventorySlot>().Init(itemId);
    }
    
    public void ChangeItemAmount(int itemId)
    {
        if (_address.ContainsKey(itemId))
        {
            int line = _address[itemId] / _slotsInLine;
            int slot = _address[itemId] % _slotsInLine;
            _lines[line].transform.GetChild(slot).GetComponent<InventorySlot>().UpdateAmount();
        }
    }
    
    public void RemoveItem(int itemId)
    {
        if (_address.ContainsKey(itemId))
        {
            int line = _address[itemId] / _slotsInLine;
            int slot = _address[itemId] % _slotsInLine;
            _lines[line].transform.GetChild(slot).GetComponent<InventorySlot>().ClearSlot();
            _slotUsed[line][slot] = true;
        }
    }
    
    public void ClearInventory()
    {
        foreach (var line in _lines)
        {
            for (int i = 0; i < _slotsInLine; i++)
            {
                line.transform.GetChild(i).GetComponent<InventorySlot>().ClearSlot();
            }
        }
    }
}
