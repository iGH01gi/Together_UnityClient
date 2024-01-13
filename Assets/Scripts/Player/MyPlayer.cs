using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{

    void Start()
    {
        StartCoroutine("CoSendPacket");
    }

    void Update()
    {
        
    }
    
    //이동 패킷 보냄
    public IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = UnityEngine.Random.Range(-50, 50);
            movePacket.posY = 0;
            movePacket.posZ = UnityEngine.Random.Range(-50, 50);

            Managers.Network.Send(movePacket.Write());
        }
    }
}
