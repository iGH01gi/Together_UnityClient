using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCountDownActivator : MonoBehaviour
{
    private InGameUI _inGameUI;
    private void Awake()
    {
        _inGameUI = Managers.UI.GetComponentInSceneUI<InGameUI>();
    }

    void Update()
    {
        if(_inGameUI == null)
        {
            Destroy(this);
        }
        float old = Managers.Game._myKillerSkill._currentCoolTime;
        float max = Managers.Game._myKillerSkill._skillCoolTime;
        float cur = Mathf.Max(old + Time.deltaTime,max);
        if (Managers.Killer.GetMyKillerInfo().CanUseSkill)
        {
            Debug.Log("Skill is ready");
            _inGameUI.SetKillerSkillValue(max);
            Destroy(this);
        }
        else
        {
            Managers.Game._myKillerSkill._currentCoolTime = cur;
            _inGameUI.SetKillerSkillValue(cur);
        }
    }
}
