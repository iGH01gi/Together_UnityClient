using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Google.Protobuf;
using ServerCore;
using UnityEngine;

public class NetworkManager
{
    public ServerSession _roomSession = new ServerSession(); //게임룸 서버와 연결된 세션
    public DedicatedServerSession _dedicatedServerSession = new DedicatedServerSession(); //데디케이티드 서버와 연결된 세션

    /// <summary>
    /// 룸서버와 연결하고 전용세션을 생성
    /// </summary>
    public void Init()
    {
        //DNS
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();
        
        connector.Connect(endPoint, () => { return _roomSession; }, 1); //게임룸 서버에 연결
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
                handler.Invoke(packet.Session, packet.Message);
        }	
    }

    /// <summary>
    /// 유니티가 종료될때 연결 끊음. Managers에서 실행시킴. 결론적으로 Discconect실행됨
    /// </summary>
    public void OnQuitUnity()
    {
        if(_roomSession!= null)
            _roomSession.Disconnect();
    }
    
}