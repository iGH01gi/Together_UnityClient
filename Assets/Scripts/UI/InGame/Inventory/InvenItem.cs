using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenItem : MonoBehaviour
{
    int itemID;
    
    public void Init(int itemId)
    {
        itemID = itemId;
    }
    
    public void SetSlot()
    {
        transform.Find($"Sprite").GetComponent<Image>().sprite = Managers.Resource.GetIcon(itemID.ToString());
        transform.Find($"Amount").GetComponent<TMP_Text>().text = Managers.Inventory._ownedItems[itemID].ToString();
    }

    public void ResetSlot()
    {
        transform.Find($"Sprite").GetComponent<Image>().sprite = null;
        transform.Find($"Amount").GetComponent<TMP_Text>().text = string.Empty;
    }
}
