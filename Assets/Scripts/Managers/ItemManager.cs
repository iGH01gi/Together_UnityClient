using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ItemManager
{
    private string _jsonPath = Application.dataPath+"/Data/Item/Items.json";
    private Dictionary<int, ItemFactory> _itemFactories; //key: 아이템Id, value: 아이템 팩토리 객체
    public Dictionary<int, IItem> _items; //key: 아이템Id, value: 아이템 객체(아이템별 데이터 저장용)
    private static string _itemsDataJson; //json이 들어 있게 됨(파싱 해야 함)
    
    public void Init()
    {
        InitItemFactories();
    }
    
    /// <summary>
    /// 아이템 생성
    /// </summary>
    /// <param name="itemId">아이템Id</param>
    /// <returns>초기 데이터 세팅까지 완료된 아이템 </returns>
    public IItem CreateItem(int itemId)
    {
        if (_itemFactories.ContainsKey(itemId))
        {
            return _itemFactories[itemId].CreateItem();
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
        _itemFactories.Add(1, new DashFactory());
        _itemFactories.Add(2, new FireworkFactory());
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
        var items = new Dictionary<int, IItem>();
        
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
                items.Add(item.Id, item);
            }
        }
    }
    
    
}