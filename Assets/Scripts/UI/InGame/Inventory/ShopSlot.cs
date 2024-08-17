using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private int itemID;
    public void Init(int itemId)
    {
        itemID = itemId;
        //아이템 아이콘 설정
        transform.Find($"Icon/Sprite").GetComponent<Image>().sprite = Managers.Resource.GetIcon(itemID.ToString());
        
    }
}
