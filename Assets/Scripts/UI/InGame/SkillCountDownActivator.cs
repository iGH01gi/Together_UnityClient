using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCountDownActivator : MonoBehaviour
{
    void Update()
    {
        float old = Managers.Game._myKillerSkill._currentCoolTime;
        float max = Managers.Game._myKillerSkill._skillCoolTime;
        float cur = Mathf.Max(old + Time.deltaTime,max);
        if (Managers.Killer.GetMyKillerInfo().CanUseSkill)
        {
            Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(max);
            Destroy(this);
        }
        else
        {
            Managers.Game._myKillerSkill._currentCoolTime = cur;
            Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(cur);
        }
    }
}
