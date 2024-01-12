using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유니티메인쓰레드와 백그라운드 쓰레드(네트워크를 처리하는) 사이의 소통을 PacketQueue라는 통로를 이용해서 처리
//메인 쓰레드에서는 Pop을 사용해서 처리
public class PacketQueue
{
    public static PacketQueue Instance { get; } = new PacketQueue();
    
    Queue<IPacket> _packetQueue = new Queue<IPacket>();
    object _lock = new object();

    public void Push(IPacket packet)
    {
        lock (_lock)
        {
            _packetQueue.Enqueue(packet);
        }
    }
        
    public IPacket Pop()
    {
        lock (_lock)
        {
            if (_packetQueue.Count == 0)
                return null;
            
            return _packetQueue.Dequeue();
        }
    }
}

