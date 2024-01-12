using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DummyClient;
using ServerCore;
using UnityEngine;

public class NetworkManager
{
    ServerSession _session = new ServerSession();

    public void Init()
    {
        //DNS
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();
        //현재 count개의 클라이언트를 시뮬레이팅한다는 설정(count)
        connector.Connect(endPoint, () => { return _session; }, 1);

       
    }

    /// <summary>
    /// 패킷큐에서 지속적으로 패킷을 뽑아서 처리하는 함수
    /// </summary>
    public void Update()
    {
        IPacket packet = PacketQueue.Instance.Pop();
        if (packet != null)
        {
            PacketManager.Instance.HandlePacket(_session, packet);
        }
    }

    //3초마다 코루틴을 이용해서 패킷 보내는 용도
    public IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);

            C_Chat chatPacket = new C_Chat();
            chatPacket.chat = "Hello Unity!";
            ArraySegment<byte> segment = chatPacket.Write();
            
            _session.Send(segment);
        }
    }
}