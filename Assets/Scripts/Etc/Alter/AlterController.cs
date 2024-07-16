using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AlterController : MonoBehaviour
{
    public float _timeToCleanse = 3f;
    public float _resetTime = 3f;
    public void Init()
    {
        //TODO: 이닛 함수 구현
    }
    
    public void TryCleanse(int alterID)
    {
        //TODO: 서버에게 alterID를 보내서 사용 가능여부 확인
        Managers.UI.LoadPopupPanel<AlterPopup>(true, false);
    }

    public void CleanseSuccess()
    {
        Debug.Log("Cleanse Success");
        //TODO: 클린즈 성공시
    }
    
    public void CleanseQuit()
    {
        Debug.Log("Cleanse Quit");
        //TODO: 클린즈 중단시
    }
}
