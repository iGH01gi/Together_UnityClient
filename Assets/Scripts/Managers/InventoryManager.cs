using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    InGameUI _inGameUI;
    public Hotbar _hotbar;
    public PlayerInventory _inventory;
    public Shop _shop;
    
    private int _totalPoint = 0; //상자로 얻은 총 포인트(낮마다 초기화)
    
    //하나 더 생기면 걍 클래스를 만들자...
    public Dictionary<int,int> _ownedItems = new Dictionary<int, int>(); //key: 아이템Id, value: 아이템 개수
    public Dictionary<int,InventorySlot> _address = new Dictionary<int, InventorySlot>();

    public void Init()
    {
        _totalPoint = 0;
        _inGameUI = Managers.UI.GetComponentInSceneUI<InGameUI>();
        _hotbar = _inGameUI._hotbar.GetComponent<Hotbar>();
        _inventory = _inGameUI._inventory.GetComponent<PlayerInventory>();
        _shop = _inGameUI._shop.GetComponent<Shop>();
        _shop.Init();
    }
    
    public void Clear()
    {
        _totalPoint = 0;
        foreach (var key in _address.Keys)
        {
            _address[key].ClearSlot();
        }
        _ownedItems.Clear();
        _address.Clear();
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
    /// 포인트와 아이템 리셋
    /// </summary>

    #endregion

    #region 아이템 서버 요청 함수

    /// <summary>
    /// 아이템 구매 시도
    /// </summary>
    /// <param name = "itemID">구매하려는 아이템id</param>
    public void TryBuyItem(int itemID)
    {
        BuyItemSuccess(itemID);
        /*if((_totalPoint < Managers.Item._items[itemID].Price)&&(Managers.Game._isDay))
        {
            Managers.Sound.Play("Error", Define.Sound.Effects,null,1.3f);
        }
        else
        {
            //TODO: 아이템 구매 서버 리퀘스트 보내기
            BuyItemSuccess(itemID);
        }*/
    }
    

    #endregion

    #region 서버 답변 처리 함수

    /// <summary>
    /// 아이템을 인벤에 1개 추가함
    /// </summary>
    /// <param name="itemID">구매 가능한 아이템id</param>
    public void BuyItemSuccess(int itemID)
    {
        Managers.Sound.Play("PurchaseSuccess");
        int price = Managers.Item._items[itemID].Price;
        _totalPoint -=price;
        _inGameUI.SetCurrentCoin(_totalPoint);
        _inGameUI.AddGetCoin(price,false);
        
        if(_ownedItems.ContainsKey(itemID))
        {
            _ownedItems[itemID]++;
            _address[itemID].UpdateAmount();
        }
        else
        {
            _ownedItems.Add(itemID, 1);
            _inventory.AddNewItem(itemID);
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
                _address[itemID].ClearSlot();
                _ownedItems.Remove(itemID);
            }
            else
            {
                _address[itemID].UpdateAmount();
            }
        }
    }
}
