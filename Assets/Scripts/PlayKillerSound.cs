using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayKillerSound : MonoBehaviour
{
    private float _dokidokiStart;
    private float _dokidokiClose;
    private float _dokidokiExtreme;
    private bool isDoki;
    
    public void Init(float _dokidokiStart, float _dokidokiClose, float _dokidokiExtreme)
    {
        isDoki = (Vector3.Distance(Managers.Player._myDediPlayer.transform.position,
            Managers.Player.GetKillerGameObject().transform.position)<=_dokidokiStart);
        this._dokidokiStart = _dokidokiStart;
        this._dokidokiClose = _dokidokiClose;
        this._dokidokiExtreme = _dokidokiExtreme;
    }
    
    public void CheckPlayKillerSound(float currentDistance)
    {
        if (!isDoki && currentDistance<= _dokidokiStart)
        {
            isDoki = true;
            StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Heartbeat, "Heartbeat"));
            StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Bgm));
        }
        else if (isDoki && (currentDistance >_dokidokiStart))
        {
            StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Bgm,"tense-horror-background"));
            StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Heartbeat));
            isDoki = false;
        }
        
        if (currentDistance <= _dokidokiExtreme)
        {
            Managers.Sound.ChangePitch(Define.Sound.Heartbeat,2.0f);
        }
        else if (currentDistance <= _dokidokiClose)
        {
            Managers.Sound.ChangePitch(Define.Sound.Heartbeat, 1.5f);
        }
        else if (currentDistance <= _dokidokiStart)
        {
            Managers.Sound.ChangePitch(Define.Sound.Heartbeat, 1.0f);
        }
    }
}
