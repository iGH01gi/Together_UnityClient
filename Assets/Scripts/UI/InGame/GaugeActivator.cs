using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeActivator : MonoBehaviour
{
    //client 자체 카운트 다운

    // Update is called once per frame
    void Update()
    {
        Managers.Game._clientGauge.DecreaseAllGaugeAuto();
    }
}
