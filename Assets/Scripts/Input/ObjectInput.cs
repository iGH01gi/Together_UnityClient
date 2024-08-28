using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInput : MonoBehaviour
{
    public GameObject _currentChest;
    public GameObject _currentCleanse;

    /// <summary>
    /// 열수있는 상자 표시(애니메이션 느려짐)
    /// </summary>
    /// <param name="newChest">범위안에 감지된 상자</param>
    public void ChangeHighlightChest(GameObject newChest)
    {
        if (_currentChest != null)
        {
            if (
            Vector3.Angle(transform.forward, _currentChest.transform.position - transform.position) <
            Vector3.Angle(transform.forward, newChest.transform.position - transform.position))
            {
                return;
            }
            _currentChest.GetComponent<Chest>().UnHighlightChest();
        }
        _currentChest = newChest;
        _currentChest.GetComponent<Chest>().HighlightChest();
    }

    /// <summary>
    /// 좌클릭: 낮-상자열기 , 밤-생존자면 클렌즈;킬러면 기본공격
    /// </summary>
    /// <param name="value">뉴인풋시스템 사용하느라 필요한거</param>
    void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract");
        if (Managers.Game._isDay) //낮이면
        {
            if (_currentChest != null)
            {
                //상자 열기 시도
                Managers.Object._chestController.TryOpenChest(_currentChest.GetComponent<Chest>()._chestId);
            }
        }
        else //밤이면
        {
            //생존자일때
            if (!Managers.Player.IsMyDediPlayerKiller())
            {
                if (_currentCleanse != null)
                {
                    if ((Managers.Object._cleanseController._myPlayerCurrentCleanse == null) && value.isPressed)
                    {
                        Managers.Object._cleanseController.TryCleanse(
                            _currentCleanse.GetComponent<Cleanse>()._cleanseId);
                    }
                    else if (value.isPressed)
                    {
                        QuitCleansing();
                    }
                }
            }
            //킬러일때
            else
            {
                Managers.Killer.BaseAttack(Managers.Player._myDediPlayerId);
            }
        }
    }

    /// <summary>
    /// 클렌즈 취소
    /// </summary>
    public void QuitCleansing()
    {
        if ((Managers.Object._cleanseController._myPlayerCurrentCleanse != null))
        {
            Managers.Object._cleanseController.QuitCleansing(_currentCleanse.GetComponent<Cleanse>()._cleanseId);
        }
    }

    /// <summary>
    /// 트랩에 걸렸을때
    /// </summary>
    /// <param name="dediPlayerId">트랩에 걸린 데디플레이어id</param>
    public IEnumerator Trapped(int dediPlayerId)
    {
        float trapDuration = (Managers.Item._itemFactories[4] as TrapFactory).TrapDuration;

        if (dediPlayerId == Managers.Player._myDediPlayerId) //내 플레이어일때
        {
            GameObject myDediPlayerGameObject = Managers.Player._myDediPlayer;
            PlayerAnimController playerAnimController = myDediPlayerGameObject.GetComponentInChildren<PlayerAnimController>();
            playerAnimController.isTrapped = true;
            playerAnimController.PlayAnim();

            yield return new WaitForSeconds(trapDuration);

            playerAnimController.isTrapped = false;
            playerAnimController.PlayAnim();
        }
        else //다른 플레이어일때
        {

        }
    }

    public void Clear()
    {
        _currentChest = null;
        _currentCleanse = null;
    }
}
