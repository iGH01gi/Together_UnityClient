using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyKillerSkill : MonoBehaviour
{
    public bool _canUseSkill;
    public float _skillCoolTime;
    public float _currentCoolTime; //현재 스킬쿨 값

    public void Init()
    {
        _skillCoolTime = Managers.Killer.GetMyKillerInfo().SkillCoolTimeSeconds;
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetSkillCooltime(_skillCoolTime);
        _currentCoolTime = _skillCoolTime;
        Managers.UI.GetComponentInSceneUI<InGameUI>().SetKillerSkillValue(_skillCoolTime);
        _canUseSkill = true;
    }

    public void TryUseSkill()
    {
        if (_canUseSkill)
        {
            _canUseSkill = false;
            Managers.Killer.UseSkill(Managers.Player._myDediPlayerId);
            _currentCoolTime = 0f;
            transform.AddComponent<SkillCountDownActivator>();
        }
    }
}
