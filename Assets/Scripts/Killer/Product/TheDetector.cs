using System.Collections;
using Google.Protobuf.Protocol;
using INab.WorldScanFX.Builtin;
using UnityEngine;

public class TheDetector : MonoBehaviour, IKiller
{
    //킬러 공통 보유 속성
    public int Id { get; set; }
    public string EnglishName { get; set; }
    public string KoreanName { get; set; }
    public string EnglishDescription { get; set; }
    public string KoreanDescription { get; set; }
    public string EnglishAbilityName { get; set; }
    public string KoreanAbilityName { get; set; }
    public string EnglishAbilityDescription { get; set; }
    public string KoreanAbilityDescription { get; set; }
    public float SkillCoolTimeSeconds { get; set; } //스킬 쿨타임 초
    
    public bool CanUseSkill { get; set; } //스킬 사용 가능 여부
    
    //Setting에 없는 속성
    public float _currentCoolTime; //현재 스킬쿨 값
    
    public void Setting()
    {
        //킬러 매니저로부터 킬러 데이터를 받아와서 설정
        TheDetector theDetectorData = Managers.Killer._killers[1] as TheDetector;
        
        Id = theDetectorData.Id;
        EnglishName = theDetectorData.EnglishName;
        KoreanName = theDetectorData.KoreanName;
        EnglishDescription = theDetectorData.EnglishDescription;
        KoreanDescription = theDetectorData.KoreanDescription;
        EnglishAbilityName = theDetectorData.EnglishAbilityName;
        KoreanAbilityName = theDetectorData.KoreanAbilityName;
        EnglishAbilityDescription = theDetectorData.EnglishAbilityDescription;
        KoreanAbilityDescription = theDetectorData.KoreanAbilityDescription;
        
        SkillCoolTimeSeconds = theDetectorData.SkillCoolTimeSeconds;

        //유니티 자체 설정
        CanUseSkill = true;
    }

    public void Use(int playerId)
    {
        if (Managers.Player._myDediPlayerId == playerId)
        {
            if (Managers.Player.IsMyDediPlayerKiller())
            {
                CanUseSkill = false;
                UseAbility();
                Managers.Game._myKillerSkill.UsedSkill(SkillCoolTimeSeconds);
            }
            else
            {
                SurvivorDetected();
            }
        }
    }

    public void BaseAttack()
    {
        transform.GetComponent<PlayerAnimController>().KillerBaseAttack();
    }

    private void UseAbility()
    {
        ScanFX scanFX = Managers.Player._myDediPlayer.GetComponentInChildren<ScanFX>();
        if (scanFX != null)
        {
            scanFX.PassScanOriginProperties();
            scanFX.StartScan(1);
        }
        //Use Ability
        //Detect survivors
        //Send packet
    }

    private void SurvivorDetected()
    {
        
    }
}