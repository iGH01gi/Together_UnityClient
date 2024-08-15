using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void Init()
    {
        foreach (KeyValuePair<int, IItem> entry in Managers.Item._items)
        {
            //instantiate slot
            //set item icon to slot
            //set item cost to slot
        }
    }
}
