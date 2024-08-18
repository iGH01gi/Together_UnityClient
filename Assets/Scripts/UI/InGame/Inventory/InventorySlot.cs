using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int itemID;
    Image _sprite;
    TMP_Text _amount;

    private void Start()
    {
        itemID = -1;
        _sprite = transform.Find($"Item/Sprite").GetComponent<Image>();
        _amount = transform.Find($"Item/Amount").GetComponent<TMP_Text>();
    }

    public void Init(int itemId)
    {
        itemID = itemId;
        var info = Managers.Item._items[itemID];
        //아이템 아이콘 설정
        _sprite.sprite = Managers.Resource.GetIcon(itemID.ToString());
        //아이템 개수 설정
        _amount.text = Managers.Inventory._ownedItems[itemID].ToString();
    }
    
    public void ClearSlot()
    {
        _sprite.sprite = null;
        _amount.text = string.Empty;
        itemID = -1;
    }

    public void UpdateAmount()
    {
        _amount.text = Managers.Inventory._ownedItems[itemID].ToString();
    }
    
    //<summary>
    //아이템 슬롯 교체 (Item GameObject가 먼저!!! 바뀌고 InventorySlot이 바뀌어야 함)
    //</summary>
    public void SwapSlots(int id)
    {
        itemID = id;
        _sprite = transform.Find($"Item/Sprite").GetComponent<Image>();
        _amount = transform.Find($"Item/Amount").GetComponent<TMP_Text>();
    }
}
