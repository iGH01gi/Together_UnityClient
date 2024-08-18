using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int itemID;
    Image _icon;
    TMP_Text _amount;
    private Sprite _baseSprite;

    private void Start()
    {
        itemID = -1;
        _icon = transform.Find($"Icon").GetComponent<Image>();
        _baseSprite = _icon.sprite;
        _amount = transform.Find($"Amount").GetComponent<TMP_Text>();
    }

    public void Init(int itemId)
    {
        itemID = itemId;
        var info = Managers.Item._items[itemID];
        //아이템 아이콘 설정
        _icon.sprite = Managers.Resource.GetIcon(itemID.ToString());
        //아이템 개수 설정
        _amount.text = Managers.Inventory._ownedItems[itemID].ToString();
    }
    
    public void ClearSlot()
    {
        _icon.sprite = _baseSprite;
        _amount.text = string.Empty;
        itemID = -1;
    }

    public void UpdateAmount()
    {
        _amount.text = Managers.Inventory._ownedItems[itemID].ToString();
    }
}
