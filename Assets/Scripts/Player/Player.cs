using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;

public class Player
{
    //현재 playerId는 sessionId와 같게 처리하고 있음
    public int PlayerId { get; set; }
    public string Name { get; set; }

    private float posX;
    private float posY;
    private float posZ;
    private float rotX;
    private float rotY;
    private float rotZ;

}
