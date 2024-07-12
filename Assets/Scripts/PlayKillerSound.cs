using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayKillerSound : MonoBehaviour
{
    private bool _isWithinRange = false;
    private bool currentlyHighPitch = false;
    
    public void CheckPlayKillerSound(bool currentlyInDokiRange, bool currentlyInDokiExtremeRange)
    {
        if (currentlyInDokiExtremeRange ^ currentlyHighPitch)
        {
            currentlyHighPitch = currentlyInDokiExtremeRange;
            if (currentlyInDokiExtremeRange)
            {
                Managers.Sound.ChangePitch(Define.Sound.Heartbeat,1.5f);
            }
            else
            {
                Managers.Sound.ChangePitch(Define.Sound.Heartbeat,1.0f);
            }
        }
        if (_isWithinRange ^ currentlyInDokiRange)
        {
            _isWithinRange = currentlyInDokiRange;
            if (currentlyInDokiRange)
            {
                StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Heartbeat, "Heartbeat"));
                StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Bgm));
            }
            else
            {
                StartCoroutine(Managers.Sound.FadeOut(Define.Sound.Heartbeat));
                StartCoroutine(Managers.Sound.FadeIn(Define.Sound.Bgm,"tense-horror-background"));
            }
        }
        
    }
}
