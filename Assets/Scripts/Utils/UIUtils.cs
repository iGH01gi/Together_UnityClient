using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIUtils
{
    public static void BindUISlider<T>(T classToBind, Action<GameObject> OnSliderValueChanged,Transform transform)
    {
        foreach (var current in classToBind.GetType().GetFields().ToList())
        {
            GameObject go = Managers.Resource.Instantiate("UI/UISlider", transform);
            go.name = current.Name;
            go.transform.GetChild(0).GetComponent<LocalizeStringEvent>().StringReference
                .SetReference("StringTable", current.Name);
            string val = current.GetValue(classToBind).ToString();
            go.transform.GetChild(1).GetComponent<Slider>().value = float.Parse(val);
            go.transform.GetChild(1).GetComponent<Slider>().onValueChanged.AddListener(delegate
            {
                OnSliderValueChanged(go);
            });
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = val;
        }
    }
}
