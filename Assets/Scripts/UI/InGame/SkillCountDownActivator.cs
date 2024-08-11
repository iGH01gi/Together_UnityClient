using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCountDownActivator : MonoBehaviour
{
    void Update()
    {
        float old = Managers.Game._myKillerSkill._currentCoolTime;
        float max = Managers.Game._myKillerSkill._skillCoolTime;
        float cur = old + Time.deltaTime;
        if (cur >= max)
        {
            Managers.Game._myKillerSkill._currentCoolTime = max;
            Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(max);
            Managers.Game._myKillerSkill._canUseSkill = true;
            Destroy(this);
        }
        else
        {
            Managers.Game._myKillerSkill._currentCoolTime = cur;
            Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(cur);
        }
    }
}
