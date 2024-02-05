using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    public GameRoom Room { get; set; } //현재 내가 참가하고 있는 게임룸
    void Start()
    {
        //StartCoroutine("CoSendPacket");
    }

    void Update()
    {
        
    }
    
    /*//이동 패킷 보냄
    public IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = UnityEngine.Random.Range(-3, 3);
            movePacket.posY = 1;
            movePacket.posZ = UnityEngine.Random.Range(-3, 3);

            Managers.Network.Send(movePacket.Write());
        }
    }*/
}
