using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    string _viewContentPath = "Scroll View/Viewport/Content";
    string _slotPath = "UI/Inventory/InventoryParts/ShopSlot";
    private Dictionary<int, ShopSlot> _shopSlots;
    public void Init()
    {
        _shopSlots = new Dictionary<int, ShopSlot>();
        foreach (KeyValuePair<int, IItem> entry in Managers.Item._items)
        {
            ShopSlot temp = Managers.Resource.Instantiate(_slotPath,transform.Find(_viewContentPath)).GetComponent<ShopSlot>();
            _shopSlots.Add(entry.Key, temp);
            temp.Init(entry.Key);
        }
    }
}
