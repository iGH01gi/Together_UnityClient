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
    
    void Start()
    {
        transform.Find(_viewContentPath).GetComponent<VerticalLayoutGroup>().spacing = Screen.width / 300;
        for(int i =0;i<_initialLines;i++)
        {
            MakeNewLine();
        }
    }

    void MakeNewLine()
    {
        GameObject cur = Managers.Resource.Instantiate(_linePath, transform.Find(_viewContentPath));
        cur.GetComponent<HorizontalLayoutGroup>().spacing = Screen.width / 300;
    }

    public void AddNewItem(int itemId)
    {
        InventorySlot slot;
        //비어있는 슬롯이 있을 경우
        Transform _lineTransform = transform.Find(_viewContentPath);
        foreach(Transform child in _lineTransform)
        {
            for(int i=0;i<child.childCount;i++)
            {
                slot = child.GetChild(i).GetComponentInChildren<InventorySlot>();
                if (slot.itemID == -1)
                {
                    slot.Init(itemId);
                    Managers.Inventory._address.Add(itemId,slot);
                    return;
                }
            }
        }
        //모든 슬롯이 차있을 경우
        MakeNewLine();
        slot = _lineTransform.GetChild(_lineTransform.childCount-1).GetChild(0).GetComponentInChildren<InventorySlot>();
        slot.Init(itemId);
        Managers.Inventory._address.Add(itemId,slot);
    }
}
