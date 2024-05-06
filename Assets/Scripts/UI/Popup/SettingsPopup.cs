using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RainbowArt.CleanFlatUI;
using UnityEngine;

public class SettingsPopup : UI_popup
{
    public int currentTab = 0;
    private Transform TabGroup;
    private Transform ViewGroup;
    private int childCount;
    private List<Transform> Tabs;
    private List<Transform> Views;
    

    private void Start()
    {
        TabGroup = transform.GetChild(0);
        ViewGroup = transform.GetChild(1);
        childCount = TabGroup.childCount;
        if (childCount != ViewGroup.childCount)
        {
            Debug.Log("tab and view count not equal");
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            Tabs.Add(TabGroup.GetChild(i));
            Views.Add(ViewGroup.GetChild(i));
            if (i != currentTab)
            {
                Tabs[i].GetComponent<ButtonWave>().Init(i,false);
            }
            else
            {
                Tabs[i].GetComponent<ButtonWave>().Init(i,true);
            }
        }
    }

    public void ChangeTab(int target)
    {
        
    }

    void DisableAll()
    {
        foreach (Transform view in ViewGroup)
        {
            view.gameObject.SetActive(false);
        }
    }
}
