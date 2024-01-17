using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySettings : MonoBehaviour
{
    void Start()
    {
        UIUtils.BindFieldToUIToggle(Managers.Data.Player,"isFullScreen",OnFullScreenChanged,transform);
    }

    public void OnFullScreenChanged(GameObject go)
    {
        
    }
}
