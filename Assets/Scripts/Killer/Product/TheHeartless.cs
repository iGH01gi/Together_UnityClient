using UnityEngine;

public class TheHeartless : MonoBehaviour, IKiller
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
    
    
    //이 킬러만의 속성
    public float HeartlessSeconds { get; set; } //심장소리 안들리게 할 시간
    
    public void Setting()
    {
        //킬러 매니저로부터 킬러 데이터를 받아와서 설정
        TheHeartless theHeartlessData = Managers.Killer._killers[0] as TheHeartless;
        
        Id = theHeartlessData.Id;
        EnglishName = theHeartlessData.EnglishName;
        KoreanName = theHeartlessData.KoreanName;
        EnglishDescription = theHeartlessData.EnglishDescription;
        KoreanDescription = theHeartlessData.KoreanDescription;
        EnglishAbilityName = theHeartlessData.EnglishAbilityName;
        KoreanAbilityName = theHeartlessData.KoreanAbilityName;
        EnglishAbilityDescription = theHeartlessData.EnglishAbilityDescription;
        KoreanAbilityDescription = theHeartlessData.KoreanAbilityDescription;
        SkillCoolTimeSeconds = theHeartlessData.SkillCoolTimeSeconds;
        
        HeartlessSeconds = theHeartlessData.HeartlessSeconds;
    }

    public void Use()
    {
        //TODO: 심장소리를 일정 시간동안 들리지 않게 하는 기능을 구현 + 이펙트
    }
}