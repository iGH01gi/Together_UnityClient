using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using INab.WorldScanFX;
using INab.WorldScanFX.Builtin;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    private DetectorCamera _detectorCamera;
    private Camera _mainCamera;
    private ScanFX _scanFX;
    private List<GameObject> _highlightUIGO;
    
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
        Assign();
    }

    public void Use(int usePlayerId)
    {
        if (Managers.Player.IsMyDediPlayerKiller() && CanUseSkill)
        {
            CanUseSkill = false;
            Managers.Sound.Play("SonarPing");
        }
        else if (Managers.Player._myDediPlayerId == usePlayerId & !Managers.Player.IsMyDediPlayerKiller())
        {
        }
    }

    public void BaseAttack()
    {
        transform.GetComponent<PlayerAnimController>().KillerBaseAttack();
    }

    private void UseAbility()
    {
        _scanFX.PassScanOriginProperties();
        _scanFX.StartScan(1);
    }

    private void SurvivorDetected()
    {
        
    }

    private void Assign()
    {
        _mainCamera = Camera.main;
        
        //highlightObjects 설정
        List<ScanFXHighlight> highlightObjects = new List<ScanFXHighlight>();
        _highlightUIGO = new List<GameObject>();

        foreach(GameObject survivor in Managers.Player._otherDediPlayers.Values)
        {
            ScanFXHighlight cur = survivor.GetComponentInChildren<ScanFXHighlight>();
            if (cur != null)
            {
                cur.FindRenderersInChildren();
                
                //커스텀 UI 설정
                CustomUIHighlight customUIHighlight = survivor.GetComponentInChildren<CustomUIHighlight>();
                if (customUIHighlight != null)
                {
                    GameObject uiObject = Managers.Resource.Instantiate("WorldScanFX/HighlightUI", Managers.UI.SceneUI.transform);
                    _highlightUIGO.Add(uiObject);
                    customUIHighlight.uiComponent = uiObject;
                    customUIHighlight.uiText = uiObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    customUIHighlight.playerTransform = transform;
                    customUIHighlight.playerCamera = _mainCamera;
                    customUIHighlight.enabled = true;
                }
                
                cur.enabled = true;
                highlightObjects.Add(cur);
            }
        }

        //Scanfx 설정
        _scanFX = _mainCamera.GetComponent<ScanFX>();
        _scanFX.scanOrigin = transform;
        _scanFX.highlightObjects = highlightObjects;
        _scanFX.enabled = true;
        
        //PostProcessing 키기
        _mainCamera.GetComponent<PostProcessLayer>().enabled = true;

        //감지된 플레이어 카메라 Init
        _detectorCamera = Managers.Resource.Instantiate("WorldScanFX/DetectorCamera").GetComponent<DetectorCamera>();
        _detectorCamera.mainCamera = _mainCamera;
        _detectorCamera.enabled = true;
        
        
        //디버깅용 ScanControl
        _mainCamera.GetComponent<ScanControl>().enabled = true;
    }
    public void UnAssign()
    {
        //감지된 플레이어 카메라 삭제
        CanUseSkill = false;
    }
}