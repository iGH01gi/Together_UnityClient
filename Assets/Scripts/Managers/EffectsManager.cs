using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectsManager : MonoBehaviour
{
    Dictionary <string, PostProcessVolume> _postProcessVolumes = new Dictionary<string, PostProcessVolume>();

    /// <summary>
    /// Effects Manager의 Init. InGameScene으로 전환 후 호출할 것.
    /// </summary>
    public void Start()
    {
        flashlightStartCoroutineIsRunning = false;
        Managers.Effects = this;
        PostProcessVolume[] postProcessVolumes = GameObject.Find("PPVolumes").GetComponentsInChildren<PostProcessVolume>();
        foreach (PostProcessVolume postProcessVolume in postProcessVolumes)
        {
            _postProcessVolumes.Add(postProcessVolume.gameObject.name, postProcessVolume);
            _postProcessVolumes[postProcessVolume.gameObject.name].weight = 0;
        }
    }

    /// <summary>
    /// Detector킬러가 걸렸을 때 활성화
    /// </summary>
    public void DetectorPPEnable()
    {
        _postProcessVolumes["DetectorPP"].weight = 1;
    }
    
    /// <summary>
    /// Detector킬러가 해제되었을 때 비활성화
    /// </summary>
    public void DetectorPPDisable()
    {
        _postProcessVolumes["DetectorPP"].weight = 0;
    }

    /// <summary>
    /// Detector에게 감지 당한 생존자의 효과 재생.
    /// </summary>
    public void DetectedPPPlay()
    {
        
        PostProcessVolume cur = _postProcessVolumes["DetectedPP"];
        DepthOfField depthOfField;
        if (!cur.profile.TryGetSettings(out depthOfField))
        {
            Debug.LogError("DepthOfField settings not found in the PostProcessVolume.");
            return;
        }
        cur.weight = 1;
        StartCoroutine(DetectedPPCoroutine(depthOfField));
    }

    /// <summary>
    /// 손전등에 당했을 때 효과. 푸는건 따로 있음.
    /// </summary>

    //만약 손전등 효과가 이미 실행중이면 다시 실행하면 안됨.
    private bool flashlightStartCoroutineIsRunning;
    public void FlashlightPPPlay()
    {
        //check if coroutine is already running
        if (flashlightStartCoroutineIsRunning)
        {
            return;
        }
        
        PostProcessVolume cur = _postProcessVolumes["FlashlightPP"];
        Vignette vignette;
        //Bloom bloom;
        
        
        if (!cur.profile.TryGetSettings(out vignette))
        {
            Debug.LogError("Vignette settings not found in the PostProcessVolume.");
            return;
        }
        /*if (!cur.profile.TryGetSettings(out bloom))
        {
            Debug.LogError("Bloom settings not found in the PostProcessVolume.");
            return;
        }*/
        
        cur.weight = 1;
        flashlightStartCoroutineIsRunning = true;
        //StartCoroutine(FlashlightStartCoroutine(vignette, bloom));
        StartCoroutine(FlashlightStartCoroutine(vignette));
    }
    
    /// <summary>
    /// 손전등 효과가 끝나면 이 함수를 호출.
    /// </summary>
    public void FlashlightPPStop()
    {
        PostProcessVolume cur = _postProcessVolumes["FlashlightPP"];
        Vignette vignette;
        
        if (!cur.profile.TryGetSettings(out vignette))
        {
            Debug.LogError("Vignette settings not found in the PostProcessVolume.");
            return;
        }
        StartCoroutine(FlashlightStopCoroutine(vignette));
        cur.weight = 0;
    }

    public void Clear()
    {
        _postProcessVolumes.Clear();
    }
    
    #region Specific Effect functions
    
    ///////// DetectedPP's Coroutine /////////
    float DetectedEffectDuration = 2.5f;
    float DetectedMaxDepthOfField = 5f;

    IEnumerator DetectedPPCoroutine(DepthOfField depth)
    {
        float currentTime=0;
        while (currentTime <= DetectedEffectDuration)
        {
            depth.focusDistance.value = DetectedMaxDepthOfField * Mathf.Cos(2*Mathf.PI * (currentTime / DetectedEffectDuration));
            currentTime+=Time.deltaTime;
            yield return null;
        }
        _postProcessVolumes["DetectedPP"].weight = 0;
    }

    ///////// FlashlightPPPlay's Coroutine /////////
    
    /*//밝은 효과
    private float bloomIntensityDuration = 0.5f;
    private float bloomIntensityMax = 15f;*/

    //처음 살짝 눈 감을 때 
    private float vignetteFirstCloseIntensity = 0.7f;
    private float vignetteFirstCloseDuration = 0.4f;
    private float vignetteFirstOpenDuration = 0.9f;
    
    //완전히 눈 감을 때
    private float vignetteSecondCloseIntensity = 2f; //also used in FlashlightPPStop
    private float vignetteSecondCloseDuration = 2f;

    IEnumerator FlashlightStartCoroutine(Vignette vignette/*, Bloom bloom*/)
    {
        float currentTime=0;
        //while (currentTime < Mathf.Max(bloomIntensityDuration,vignetteSecondCloseDuration))
        while (currentTime <vignetteSecondCloseDuration)
        {
            /*if (currentTime <=bloomIntensityDuration)
            {
                bloom.intensity.value = Mathf.Lerp(0, bloomIntensityMax, currentTime / bloomIntensityDuration);
            }*/
            
            if (currentTime < vignetteFirstCloseDuration)
            {
                vignette.intensity.value = Mathf.Lerp(0, vignetteFirstCloseIntensity, currentTime / vignetteFirstCloseDuration);
            }
            else if (currentTime < vignetteFirstOpenDuration)
            {
                vignette.intensity.value = Mathf.Lerp(vignetteFirstCloseIntensity, 0, (currentTime - vignetteFirstCloseDuration) / vignetteFirstOpenDuration);
            }
            else if (currentTime < vignetteSecondCloseDuration)
            {
                vignette.intensity.value = Mathf.Lerp(0, vignetteSecondCloseIntensity, (currentTime - vignetteFirstCloseDuration - vignetteFirstOpenDuration) / vignetteSecondCloseDuration);
            }
            currentTime+=Time.deltaTime;
            yield return null;
        }
        flashlightStartCoroutineIsRunning = false;
    }
    
    ///////// FlashlightPPStop's Coroutine /////////
    
    private float flashlightStopDuration = 0.5f;
    
    IEnumerator FlashlightStopCoroutine (Vignette vignette)
    {
        if (flashlightStartCoroutineIsRunning)
        {
            yield break;
        }
        float currentTime=0;
        while (currentTime < flashlightStopDuration)
        {
            vignette.intensity.value = Mathf.Lerp(vignetteSecondCloseIntensity, 0, currentTime / flashlightStopDuration);
            currentTime+=Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
