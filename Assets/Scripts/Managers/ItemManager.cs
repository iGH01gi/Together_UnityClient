﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// json 데이터로부터 아이템 데이터를 로드하고, 아이템을 생성하기 위해서 필요한 클래스
/// </summary>
public class ItemManager
{
    private string _jsonPath;
    private string _itemPrefabFolderPath = "Items/"; //아이템 프리팹들이 들어있는 폴더 경로. 아이템id가 해당 폴더에서 프리팹의 이름
    private Dictionary<int, ItemFactory> _itemFactories; //key: 아이템Id, value: 아이템 팩토리 객체
    public Dictionary<int, IItem> _items; //key: 아이템Id, value: 아이템 객체(아이템별 데이터 저장용. 전시품이라고 생각)
    public Dictionary<int, GameObject> _itemPrefabs; //key: 아이템Id, value: 아이템 프리팹
    private static string _itemsDataJson; //json이 들어 있게 됨(파싱 해야 함)
    private ItemFactory itemf;

    
    public void Init()
    {
        _jsonPath = Application.persistentDataPath + "/Data/Item/Items.json";
        if (!Directory.Exists(Path.GetDirectoryName(_jsonPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_jsonPath));
        }
        if (!File.Exists(_jsonPath))
        {
            File.WriteAllText(_jsonPath, "{}"); // Create an empty JSON file
        }
        InitItemFactories();
        //LoadItemPrefabs();

        //itemf.CreateItem();
    }

    /// <summary>
    /// 아이템 세팅 및 생성
    /// </summary>
    /// <param name="itemId">아이템Id</param>
    /// <returns>초기 데이터 세팅까지 완료된 아이템 </returns>
    public GameObject CreateItem(int itemId)
    {
        if (_itemFactories.ContainsKey(itemId))
        {
            GameObject itemObj = _itemFactories[itemId].CreateItem();
            itemObj.transform.SetParent(Managers.Player._myDediPlayer.transform);
            return itemObj;
        }
        else
        {
            Debug.LogError("Cannot find item factory with id: " + itemId);
            return null;
        }
    }
    
    /// <summary>
    /// 아이템 팩토리 초기화
    /// </summary>
    public void InitItemFactories()
    {
        _itemFactories = new Dictionary<int, ItemFactory>();
        _items = new Dictionary<int, IItem>();
        
        //아이템 팩토리 생성
        _itemFactories.Add(0, new DashFactory());
        _itemFactories.Add(1, new FireworkFactory());
    }
    
    /// <summary>
    /// 아이템 프리팹 로드(반드시 아이템 팩토리 초기화 이후에 호출해야 함)
    /// </summary>
    public void LoadItemPrefabs()
    {
        _itemPrefabs = new Dictionary<int, GameObject>();
        
        _itemPrefabs.Add(0, Managers.Resource.Load<GameObject>(_itemPrefabFolderPath + "Dash"));
        _itemPrefabs.Add(1, Managers.Resource.Load<GameObject>(_itemPrefabFolderPath + "Firework"));
    }
    
    /// <summary>
    /// 서버로부터 받은 json데이터를 저장함
    /// </summary>
    /// <param name="jsonData">json 문자열</param>
    public void SaveJsonData(string jsonData)
    {
        //_jsonPath에다가 jsonData를 저장. 이미 존재한다면 지우고 덮어쓰기
        File.WriteAllText(_jsonPath, jsonData);
    }
    
    /// <summary>
    /// 아이템 데이터를 로드후 파싱(서버로부터 json데이터를 받은 후)
    /// </summary>
    public void LoadItemData()
    {
        if (File.Exists(_jsonPath))
        {
            string dataAsJson = File.ReadAllText(_jsonPath);
            _itemsDataJson = dataAsJson;
        }
        else
        {
            Debug.LogError("Cannot find file at " + _jsonPath);
            return;
        }
        
        //파싱
        ParseItemData();
    }

    /// <summary>
    /// json파일을 이미 받은 상태에서 아이템 데이터를 파싱
    /// </summary>
    private void ParseItemData()
    {
        var itemsData = JObject.Parse(_itemsDataJson)["Items"];
        _items = new Dictionary<int, IItem>();
        
        foreach (var itemData in itemsData)
        {
            IItem item = null;
            string className = itemData["EnglishName"].ToString();
            
            Type type = Type.GetType(className);
            if (type != null)
            {
                item = (IItem)itemData.ToObject(type);;
            }
            
            if (item != null)
            {
                _items.Add(item.Id, item);
            }
        }
    }
    /// <summary>
    /// 아이템 가격 반환
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public int GetItemPrice(int itemId)
    {
        if(_items!=null && _items.ContainsKey(itemId))
            return _items[itemId].Price;
        else
            return -1;
    }

    /// <summary>
    /// 아이템 영어 이름 반환
    /// </summary>
    /// <param name="itemId">아이템id</param>
    /// <returns>아이템 영어 이름</returns>
    public string GetItemEnglishName(int itemId)
    {
        if(_items!=null && _items.ContainsKey(itemId))
            return _items[itemId].EnglishName;
        else
            return null;
    }
    
    /// <summary>
    /// 아이템 한글 이름 반환
    /// </summary>
    /// <param name="itemId">아이템id</param>
    /// <returns>아이템 한글 이름</returns>
    public string GetItemKoreanName(int itemId)
    {
        if(_items!=null && _items.ContainsKey(itemId))
            return _items[itemId].KoreanName;
        else
            return null;
    }
    
    /// <summary>
    /// 아이템 영어 설명 반환
    /// </summary>
    /// <param name="itemId">아이템id</param>
    /// <returns>아이템 영어 설명</returns>
    public string GetItemEnglishDescription(int itemId)
    {
        if(_items!=null && _items.ContainsKey(itemId))
            return _items[itemId].EnglishDescription;
        else
            return null;
    }
    
    /// <summary>
    /// 아이템 한글 설명 반환
    /// </summary>
    /// <param name="itemId">아이템id</param>
    /// <returns>아이템 한글 설명</returns>
    public string GetItemKoreanDescription(int itemId)
    {
        if(_items!=null && _items.ContainsKey(itemId))
            return _items[itemId].KoreanDescription;
        else
            return null;
    }
    
    /// <summary>
    /// 아이템 사용
    /// </summary>
    /// <param name="itemId">사용할 아이템</param>
    public void UseItem(int itemId)
    {
        //인벤토리에 해당 아이템이 있는지 확인
        if (Managers.Inventory._ownedItems.ContainsKey(itemId))
        {
            //아이템 사용
            IItem item = _items[itemId];
            item.Use();
            
            //인벤토리에서 아이템 제거
        //TODO: 아이템 사용 패킷 서버에 보내기 만약에 서버에서 사용 불가 응답이 오면 아래 코드 리버스 해야함.
            Managers.Inventory.RemoveItemOnce(itemId);
        }
        else
        {
            Debug.Log($"인벤토리에 {itemId} 아이템이 없음.");
        }
    }
    
    /// <summary>
    /// 아이템 선택시 기능 실행
    /// </summary>
    /// <param name="itemId">아이템id</param>
    public void OnHoldItem(int itemId)
    {
        //인벤토리에 해당 아이템이 있는지 확인
        MyDediPlayer myDediPlayer = Managers.Player._myDediPlayer.GetComponent<MyDediPlayer>();
        if (Managers.Inventory._ownedItems.ContainsKey(itemId))
        {
            //아이템 선택시 기능 실행
            IItem item = _items[itemId];
            item.OnHold();
        }
        else
        {
            Debug.Log($"인벤토리에 {itemId} 아이템이 없음.");
        }
    }
}