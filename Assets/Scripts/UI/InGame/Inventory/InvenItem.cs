using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenItem : MonoBehaviour
{
    int itemID;
    
    public void Init(int itemId)
    {
        itemID = itemId;
    }
    
    public void SetIcon()
    {
        transform.Find("Sprite").GetComponent<Image>().sprite = Managers.Resource.GetIcon(itemID.ToString());
    }
}
