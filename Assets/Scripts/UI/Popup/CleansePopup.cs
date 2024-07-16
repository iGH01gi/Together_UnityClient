using System;
using System.Collections;
using System.Collections.Generic;
using RainbowArt.CleanFlatUI;
using UnityEngine;

public class CleansePopup : UI_popup
{
    ProgressBar _progressBar;
    public Cleanse _currentCleanse = null;
    void Start()
    {
        _progressBar = transform.Find("Gauge").GetComponent<ProgressBar>();
        _progressBar.MaxValue = Managers.Object._cleanseController._cleanseDurationSeconds;
        _progressBar.CurrentValue = 0f;
    }

    void Init(Cleanse cleanse)
    {
        _currentCleanse = cleanse;
    }

    private void Update()
    {
        _progressBar.CurrentValue += Time.deltaTime;
        if (_currentCleanse != null)
        {
            _currentCleanse.CurrentlyCleansing(_progressBar.CurrentValue);
        }
    }
}
