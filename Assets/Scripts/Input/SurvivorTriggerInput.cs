using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorTriggerInput : MonoBehaviour
{
    private ObjectInput _instance;
    private ObjectInput _objectInput { get { Init(); return _instance; } }

    private void Init()
    {
        if (_instance == null)
        {
            _instance = Managers.Player._myDediPlayer.GetComponent<ObjectInput>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Managers.Game._isDay) //撤
        {
            //担 飘府芭 贸府
            if (other.CompareTag("Trap") && !other.transform.GetComponent<Trap>()._isAlreadyTrapped)
            {   
                other.transform.GetComponent<Trap>()._isAlreadyTrapped = true;
                other.transform.GetComponent<SphereCollider>().enabled = false;
                StartCoroutine(Managers.Input._objectInput.Trapped(Managers.Player._myDediPlayerId));
            }
        }
        else
        {
            //担 飘府芭 贸府
        }
    }
}
