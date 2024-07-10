using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBombSound : MonoBehaviour
{
    private bool _isWithinRange = false;
    
    public void CheckPlayBombSound(bool currentState)
    {
        if (_isWithinRange ^ currentState)
        {
            if (currentState)
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
