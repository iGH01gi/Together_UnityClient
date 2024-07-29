using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayKillerSound : MonoBehaviour
{
    private float _dokidokiStart;
    private float _dokidokiClose;
    private float _dokidokiExtreme;
    private bool isDoki;
    
    public void Init(float _dokidokiStart, float _dokidokiClose, float _dokidokiExtreme)
    {
        isDoki = false;
        this._dokidokiStart = _dokidokiStart;
        this._dokidokiClose = _dokidokiClose;
        this._dokidokiExtreme = _dokidokiExtreme;
    }
    
    public void CheckPlayKillerSound()
    {
        Transform myPlayer = Managers.Player._myDediPlayer.transform;
        Vector3 killerPos = Managers.Player.GetKillerGameObject().transform.position;
        Vector3 myPlayerPos = myPlayer.position;
        float currentDistance = Vector3.Distance(myPlayerPos,killerPos);
        
        //거리에 따라 두근두근 재생 여부 확인
        if (!isDoki && currentDistance<= _dokidokiStart)
        {
            isDoki = true;
            //StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Heartbeat, "Heartbeat"));
            StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Bgm));
        }
        else if (isDoki && (currentDistance >_dokidokiStart))
        {
            isDoki = false;
            StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Bgm,"tense-horror-background"));
            //StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Heartbeat));
        }
        
        if (isDoki)
        {
            //거리에 따라 두근두근 재생 pitch (속도) 조절
            if (currentDistance <= _dokidokiExtreme)
            {
                Managers.Sound.ChangePitch(Define.Sound.Heartbeat, 2.0f);
            }
            else if (currentDistance <= _dokidokiClose)
            {
                Managers.Sound.ChangePitch(Define.Sound.Heartbeat, 1.5f);
            }
            else if (currentDistance <= _dokidokiStart)
            {
                Managers.Sound.ChangePitch(Define.Sound.Heartbeat, 1.0f);
            }
            
            //방향에 따른 소리의 방향 계산
            float panStereoVal = Vector3.SignedAngle(killerPos - myPlayerPos, myPlayer.forward, Vector3.up)/180f;
            Managers.Sound.ChangePanStereo(Define.Sound.Heartbeat,panStereoVal);
        }
    }
}
