using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Google.Protobuf;
using ServerCore;
using UnityEngine;

public class NetworkManager
{
    public ServerSession _session = new ServerSession();

    public void Init()
    {
        //DNS
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();
        
        connector.Connect(endPoint, () => { return _session; }, 1); //게임룸 서버에 연결
    }

    /// <summary>
    /// 패킷큐에서 지속적으로 패킷을 뽑아서 처리하는 함수 (서버로부터 받은걸 처리) 
    /// 매 프레임마다 큐에 있는 모든걸 꺼내기 위해 PopAll() 사용
    /// 실제 뽑는건 메인쓰레드가 Managers의 Update에서 처리
    /// </summary>
    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }	
    }

    public void OnQuitUnity() //유니티가 종료될때 연결 끊음. Managers에서 실행시킴
    {
        if(_session!= null)
            _session.Disconnect();
    }


    public void ConnectDedicatedServer(string ip, int port)
    {
        
    }
}