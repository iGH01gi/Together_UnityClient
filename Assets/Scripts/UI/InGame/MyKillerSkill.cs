using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyKillerSkill : MonoBehaviour
{
    public float _currentCoolTime; //현재 스킬쿨 값
    public float _skillCoolTime; //스킬 쿨타임 초

    public void Init()
    {
        _skillCoolTime = Managers.Killer.GetMyKillerInfo().SkillCoolTimeSeconds;
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetSkillCooltime(_skillCoolTime);
        _currentCoolTime = _skillCoolTime;
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(_skillCoolTime);
    }

    public void UsedSkill()
    {
        _currentCoolTime = 0;
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(0);
        transform.AddComponent<SkillCountDownActivator>();
    }
}
