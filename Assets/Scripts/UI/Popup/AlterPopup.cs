using System;
using System.Collections;
using System.Collections.Generic;
using RainbowArt.CleanFlatUI;
using UnityEngine;

public class AlterPopup : UI_popup
{
    ProgressBar _progressBar;
    public Alter _currentAlter = null;
    void Start()
    {
        _progressBar = transform.Find("Gauge").GetComponent<ProgressBar>();
        _progressBar.MaxValue = Managers.Object._alterController._timeToCleanse;
        _progressBar.CurrentValue = 0f;
    }

    void Init(Alter alter)
    {
        _currentAlter = alter;
    }

    private void Update()
    {
        _progressBar.CurrentValue += Time.deltaTime;
        if (_currentAlter != null)
        {
            _currentAlter.CurrentlyCleansing(_progressBar.CurrentValue);
        }
    }
}
