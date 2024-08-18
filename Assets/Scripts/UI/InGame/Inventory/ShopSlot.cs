using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private int itemID;
    private int price;
    public void Init(int itemId)
    {
        transform.Find($"ItemName").GetComponent<TMP_Text>().fontSize = Screen.height/ 30;
        transform.Find($"ItemDescription").GetComponent<TMP_Text>().fontSize = Screen.height/ 46;
        itemID = itemId;
        var info = Managers.Item._items[itemID];
        price = info.Price;
        //아이템 아이콘 설정
        transform.Find($"Icon/Sprite").GetComponent<Image>().sprite = Managers.Resource.GetIcon(itemID.ToString());
        //아이템 가격 설정
        transform.Find($"Price/Amount").GetComponent<TMP_Text>().text = price.ToString();
        if (Util.CheckLocale(Define.SupportedLanguages.Korean))
        {
            //아이템 이름 설정
            transform.Find($"ItemName").GetComponent<TMP_Text>().text = info.KoreanName;
            //아이템 설명 설정
            transform.Find($"ItemDescription").GetComponent<TMP_Text>().text = info.KoreanDescription;
        }
        else
        {
            //아이템 이름 설정
            transform.Find($"ItemName").GetComponent<TMP_Text>().text = info.EnglishName;
            //아이템 설명 설정
            transform.Find($"ItemDescription").GetComponent<TMP_Text>().text = info.EnglishDescription;
        }
    }

    public void BuyButtonClicked()
    {
        //Managers.Inventory.TryBuyItem(itemID);
        //테스트 용 바로 아이템 구매로 넘어가기
        Managers.Inventory.TryBuyItem(itemID);
        Debug.Log("Buy " +itemID);
    }
}
