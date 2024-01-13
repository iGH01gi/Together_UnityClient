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
        
        connector.Connect(endPoint, () => { return _session; }, 1);
    }

    /// <summary>
    /// 패킷큐에서 지속적으로 패킷을 뽑아서 처리하는 함수 (서버로부터 받은걸 처리)
    /// </summary>
    public void Update()
    {
        IPacket packet = PacketQueue.Instance.Pop();
        if (packet != null)
        {
            PacketManager.Instance.HandlePacket(_session, packet);
        }
    }
    
    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }
}