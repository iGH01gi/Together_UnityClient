using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFactory : ItemFactory
{
    public float TrapDuration { get; set; }
    public float TrapRadius { get; set; }
    public float StunDuration { get; set; }
    
    public TrapFactory(int id, int price, string englishName, string koreanName, string englishDescription,
        string koreanDescription, float trapDuration, float trapRadius, float stunDuration)
    {
        base.FactoryInit(id, price, englishName, koreanName, englishDescription, koreanDescription);
        TrapDuration = trapDuration;
        TrapRadius = trapRadius;
        StunDuration = stunDuration;
    }
    
    public override GameObject CreateItem(int playerId)
    {

    }

    public override GameObject CreateOnHoldItem(int playerId)
    {
        //내가 아닌경우 onHoldItem을 생성하지 않음
        if (playerId != Managers.Player._myDediPlayerId)
        {
            return null;
        }
        else
        {
            GameObject trapGameObject = Managers.Resource.Instantiate("Items/Trap/Trap");
            trapGameObject.name = "Trap";
            Trap trap = trapGameObject.AddComponent<Trap>();
            trap.Init(FactoryId, playerId, FactoryEnglishName, TrapDuration, TrapRadius, StunDuration);

            return trapGameObject;
        }
    }
}
