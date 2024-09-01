using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

/// <Summary>
/// UI popup과 sceneUI를 구성하는 모든 UI 요소들에 대한 상위 클래스
/// </Summary>
public class UI_subitem : MonoBehaviour
{
    /// <Summary>
    /// String gameobject와 Local String Reference를 입력하면 자동 업데이트
    /// </Summary>
    protected void BindLocalizedString(GameObject go, string description)
    {
        if (description == null)
        {
            go.GetComponent<LocalizeStringEvent>().StringReference = null;
            go.GetComponent<TMP_Text>().text = "";
        }
        go.GetComponent<LocalizeStringEvent>().StringReference
            .SetReference("StringTable", description);
    }

    public void SetChildString(string str, int childindex = 0)
    {
        transform.GetChild(childindex).GetComponent<UI_Text>().SetString(str);
    }
}
