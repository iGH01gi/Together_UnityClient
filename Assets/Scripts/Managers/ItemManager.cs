using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf.Protocol;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// json 데이터로부터 아이템 데이터를 로드하고, 아이템을 생성하기 위해서 필요한 클래스
/// </summary>
public class ItemManager
{
    private string _jsonPath;
    private string _itemPrefabFolderPath = "Items/"; //아이템 프리팹들이 들어있는 폴더 경로. 아이템id가 해당 폴더에서 프리팹의 이름
    private static string _itemsDataJson; //json이 들어 있게 됨(파싱 해야 함)
    public Dictionary<int, ItemFactory> _itemFactories = new Dictionary<int, ItemFactory>(); //아이템 팩토리들을 저장하는 딕셔너리

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
    }
    
    /// <summary>
    /// 아이템을 들고 있는 상태로 변경
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="playerID"></param>
    public void HoldItem(int itemId, int playerID)
    {
        Managers.Player.ChangeHoldingItem(itemId, playerID);
    }

    public void UseItem(int itemId)
    {
        if (_itemFactories.ContainsKey(itemId))
        { 
            //TODO: 아이템 사용 구현
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다.");
        }
    }

    
    #region json관련
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
        
        foreach (var itemData in itemsData)
        {
            //아이템 타입에 따라서 아이템 팩토리 생성
            //Dash 아이템 팩토리 생성
            Debug.Log(itemData["EnglishName"]?.ToString());
            if(itemData["EnglishName"]?.ToString() == "Dash")
            {
                DashFactory itemFactory = new DashFactory(itemData["Id"].Value<int>(), 
                    itemData["Price"].Value<int>(),
                    itemData["EnglishName"].ToString(), 
                    itemData["KoreanName"].ToString(), 
                    itemData["EnglishDescription"].ToString(),
                    itemData["KoreanDescription"].ToString(),
                    itemData["DashDistance"].Value<float>());
                
                _itemFactories.Add(itemFactory.FactoryId, itemFactory);
            }
            //Firework 아이템 팩토리 생성
            else if(itemData["EnglishName"]?.ToString() == "Firework")
            {
                FireworkFactory itemFactory = new FireworkFactory(itemData["Id"].Value<int>(), 
                    itemData["Price"].Value<int>(),
                    itemData["EnglishName"].ToString(), 
                    itemData["KoreanName"].ToString(), 
                    itemData["EnglishDescription"].ToString(),
                    itemData["KoreanDescription"].ToString(),
                    itemData["FlightHeight"].Value<float>());
                _itemFactories.Add(itemFactory.FactoryId, itemFactory);
            }
            //Invisible 아이템 팩토리 생성
            else if(itemData["EnglishName"]?.ToString() == "Invisible")
            {
                InvisibleFactory itemFactory = new InvisibleFactory(itemData["Id"].Value<int>(), 
                    itemData["Price"].Value<int>(),
                    itemData["EnglishName"].ToString(), 
                    itemData["KoreanName"].ToString(), 
                    itemData["EnglishDescription"].ToString(),
                    itemData["KoreanDescription"].ToString(),
                    itemData["InvisibleSeconds"].Value<float>());
                _itemFactories.Add(itemFactory.FactoryId, itemFactory);
            }
            //Flashlight 아이템 팩토리 생성
            else if(itemData["EnglishName"]?.ToString() == "Flashlight")
            {
                FlashlightFactory itemFactory = new FlashlightFactory(itemData["Id"].Value<int>(), 
                    itemData["Price"].Value<int>(),
                    itemData["EnglishName"].ToString(), 
                    itemData["KoreanName"].ToString(), 
                    itemData["EnglishDescription"].ToString(),
                    itemData["KoreanDescription"].ToString(),
                    itemData["BlindDuration"].Value<float>(),
                    itemData["FlashlightDistance"].Value<float>(),
                    itemData["FlashlightAngle"].Value<float>(),
                    itemData["FlashlightAvailableTime"].Value<float>(),
                    itemData["FlashlightTimeRequired"].Value<float>()
                    );
                _itemFactories.Add(itemFactory.FactoryId, itemFactory);
            }
            // Trap 아이템 팩토리 생성
            else if(itemData["EnglishName"]?.ToString() == "Trap")
            {
                TrapFactory itemFactory = new TrapFactory(itemData["Id"].Value<int>(), 
                    itemData["Price"].Value<int>(),
                    itemData["EnglishName"].ToString(), 
                    itemData["KoreanName"].ToString(), 
                    itemData["EnglishDescription"].ToString(),
                    itemData["KoreanDescription"].ToString(),
                    itemData["TrapDuration"].Value<float>(),
                    itemData["TrapRadius"].Value<float>(),
                    itemData["StunDuration"].Value<float>()
                    );
                _itemFactories.Add(itemFactory.FactoryId, itemFactory);
            }
            
            else
            {
                Debug.LogError("읽을 수 없는 아이템이 입력되었습니다.");
                return;
            }
        }
    }
    #endregion
    

    #region GetterMethods

    public int GetItemPrice(int id)
    {
        if (_itemFactories.ContainsKey(id))
        {
            return(_itemFactories[id].FactoryPrice);
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다: " + id);
            return 0;
        }
    }
    
    public string GetItemEnglishName(int id)
    {
        if (_itemFactories.ContainsKey(id))
        {
            return(_itemFactories[id].FactoryEnglishName);
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다: " + id);
            return null;
        }
    }
    
    public string GetItemKoreanName(int id)
    {
        if (_itemFactories.ContainsKey(id))
        {
            return(_itemFactories[id].FactoryKoreanName);
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다: " + id);
            return null;
        }
    }
    
    public string GetItemEnglishDescription(int id)
    {
        if (_itemFactories.ContainsKey(id))
        {
            return(_itemFactories[id].FactoryEnglishDescription);
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다: " + id);
            return null;
        }
    }
    
    public string GetItemKoreanDescription(int id)
    {
        if (_itemFactories.ContainsKey(id))
        {
            return (_itemFactories[id].FactoryKoreanDescription);
        }
        else
        {
            Debug.LogError("해당 아이템이 존재하지 않습니다: " + id);
            return null;
        }
    }

    #endregion
}