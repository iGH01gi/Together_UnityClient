using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Hotbar _hotbar;
    public PlayerInventory _inventory;
    public Shop _shop;
    
    private int _totalPoint = 0; //상자로 얻은 총 포인트(낮마다 초기화)
    
    public Dictionary<int,int> _ownedItems; //key: 아이템Id, value: 아이템 개수

    public void Init()
    {
        _totalPoint = 0;
        _ownedItems = new Dictionary<int, int>();
        _hotbar = Managers.UI.GetComponentInSceneUI<InGameUI>()._hotbar.GetComponent<Hotbar>();
        _inventory = Managers.UI.GetComponentInSceneUI<InGameUI>()._inventory.GetComponent<PlayerInventory>();
        _shop = Managers.UI.GetComponentInSceneUI<InGameUI>()._shop.GetComponent<Shop>();
        _shop.Init();
    }
    
    public void Clear()
    {
        _totalPoint = 0;
        _ownedItems.Clear();
    }
    
    
    #region 포인트 관련 함수
    
    /// <summary>
    /// 포인트 설정
    /// </summary>
    /// <param name="item">설정할 포인트 int</param>
    public void SetTotalPoint(int point)
    {
        _totalPoint = point;
    }
    
    /// <summary>
    /// 포인트 리셋
    /// </summary>
    public void ResetPoint()
    {
        _totalPoint = 0;
    }
    
    #endregion

    #region 아이템 서버 요청 함수

    /// <summary>
    /// 아이템 구매 시도
    /// </summary>
    /// <param name = "itemID">구매하려는 아이템id</param>
    public void TryBuyItem(int itemID)
    {
        //TODO: 아이템 구매 서버 리퀘스트 보내기
    }
    

    #endregion

    #region 서버 답변 처리 함수

    /// <summary>
    /// 아이템을 인벤에 1개 추가함
    /// </summary>
    /// <param name="itemID">구매 가능한 아이템id</param>
    public void BuyItemSuccess(int itemID)
    {
        if(_ownedItems.ContainsKey(itemID))
        {
            _ownedItems[itemID]++;
        }
        else
        {
            _ownedItems.Add(itemID, 1);
        }
    }
    #endregion
    
    public void RemoveItemOnce(int itemID)
    {
        if(_ownedItems.ContainsKey(itemID))
        {
            _ownedItems[itemID]--;
            if(_ownedItems[itemID] == 0)
            {
                _ownedItems.Remove(itemID);
            }
        }
    }
}
