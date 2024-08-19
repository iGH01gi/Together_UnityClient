﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MyDediPlayer : MonoBehaviour
{
    //현재 데디서버의 playerId는 독자적인 값(=데디서버의 sessionID)으로 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }
    
    public bool _isKiller = false; //킬러 여부
    public int _killerType = -1; //어떤 킬러타입인지를 나타내는 ID
    public string _killerEngName; //킬러의 영문 이름
    
    public float _gauge = 0; //생명력 게이지
    public float _gaugeDecreasePerSecond = 0; //생명력 게이지 감소량

    //public Dictionary<int,int> _ownedItems = new Dictionary<int, int>(); //key: 아이템Id, value: 아이템 개수

    
    public void Init(int playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
    }
}