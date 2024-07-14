using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeActivator : ClientGauge
{
    //client 자체 카운트 다운

    private void Awake()
    {
        _gaugeActivator = this;
    }


    // Update is called once per frame
    void Update()
    {
        float old = GetMyGauge();
        DecreaseAllGaugeAuto();
        float cur = GetMyGauge();
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetCurrentGauge(cur);
    }
}
