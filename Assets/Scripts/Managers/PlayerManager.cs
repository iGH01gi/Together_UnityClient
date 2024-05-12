using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

/// <summary>
/// 룸서버와 데디서버의 플레이어 정보를 들고있는 매니저
/// </summary>
public class PlayerManager
{
    public MyRoomPlayer _myRoomPlayer; //내 룸서버 플레이어
    public Dictionary<int, RoomPlayer> _otherRoomPlayers = new Dictionary<int, RoomPlayer>(); //다른 룸서버 플레이어들 (key: playerId, value: 플레이어 정보)
    
    
    public GameObject _myDediPlayer; //내 데디서버 플레이어
    public int _myDediPlayerId = -1; //내 데디서버 플레이어의 playerId (초기값 -1)
    public Dictionary<int,GameObject> _otherDediPlayers = new Dictionary<int, GameObject>(); //다른 데디서버 플레이어들 (key: 데디playerId, value: 플레이어 오브젝트)
    public Dictionary<int,GameObject> _ghosts = new Dictionary<int, GameObject>()   ; //key: 데디playerId, value: 고스트 오브젝트
    public string _tempMyPlayerPrefabPath = "Player/MyPlayer";
    public string _tempOtherPlayerPrefabPath = "Player/OtherPlayer";
    public string _tempTargetGhost = "Player/TargetGhost";

    
    public void Clear()
    {
        _myRoomPlayer = null;
        _otherRoomPlayers.Clear();
        
        ClearDedi();
    }

    public void ClearDedi()
    {
        _myDediPlayerId = -1;
        if (_myDediPlayer != null)
        {
            GameObject.Destroy(_myDediPlayer);
            _myDediPlayer = null;
        }
        
        foreach (GameObject dediPlayer in _otherDediPlayers.Values)
        {
            if (dediPlayer != null)
            {
                GameObject.Destroy(dediPlayer);
            }
        }
        _otherDediPlayers.Clear();
        
        foreach (GameObject ghost in _ghosts.Values)
        {
            if (ghost != null)
            {
                GameObject.Destroy(ghost);
            }
        }
        _ghosts.Clear();
    }
    
    
    
    /// <summary>
    /// 데디서버 플레이어+고스트(다른 플레이어일 경우에만)를 실제로 생성하는 함수
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <param name="transformInfo"></param>
    /// <param name="isMyPlayer"></param>
    /// <returns></returns>
    public GameObject SpawnPlayer(PlayerInfo playerInfo, TransformInfo transformInfo, bool isMyPlayer)
    {
        GameObject obj = null;
        if(isMyPlayer) //본인 플레이어용 프리팹 이용
        {
            obj = Managers.Resource.Instantiate(_tempMyPlayerPrefabPath);
            obj.name = $"MyPlayer_{playerInfo.PlayerId}";
            _myDediPlayer = obj;
            _myDediPlayerId = playerInfo.PlayerId;
            obj.transform.position = new Vector3(transformInfo.Position.PosX, transformInfo.Position.PosY, transformInfo.Position.PosZ);
            obj.transform.rotation = new Quaternion(transformInfo.Rotation.RotX, transformInfo.Rotation.RotY, transformInfo.Rotation.RotZ, transformInfo.Rotation.RotW);
            
            MyDediPlayer myDediPlayerComponent = obj.AddComponent<MyDediPlayer>();
            myDediPlayerComponent.Init(playerInfo.PlayerId, playerInfo.Name);
        }
        else //다른 플레이어용 프리팹 이용
        {
            obj = Managers.Resource.Instantiate(_tempOtherPlayerPrefabPath);
            obj.name = $"OtherPlayer_{playerInfo.PlayerId}";
            Managers.Player._otherDediPlayers.Add(playerInfo.PlayerId, obj);
            obj.transform.position = new Vector3(transformInfo.Position.PosX, transformInfo.Position.PosY, transformInfo.Position.PosZ);
            obj.transform.rotation = new Quaternion(transformInfo.Rotation.RotX, transformInfo.Rotation.RotY, transformInfo.Rotation.RotZ, transformInfo.Rotation.RotW);
            
            OtherDediPlayer otherDediPlayerComponent = obj.AddComponent<OtherDediPlayer>();
            otherDediPlayerComponent.Init(playerInfo.PlayerId, playerInfo.Name);
        }
        
        
        //다른 플레이어라면 고스트 생성 및 등록
        if (!isMyPlayer)
        {
            GameObject newGhost = Managers.Resource.Instantiate(_tempTargetGhost);
            newGhost.transform.position = new Vector3(transformInfo.Position.PosX, transformInfo.Position.PosY, transformInfo.Position.PosZ);
            newGhost.transform.rotation = new Quaternion(transformInfo.Rotation.RotX, transformInfo.Rotation.RotY, transformInfo.Rotation.RotZ, transformInfo.Rotation.RotW);
            _ghosts.Add(playerInfo.PlayerId,newGhost);
            newGhost.name = $"Ghost_{playerInfo.PlayerId}"; //고스트 오브젝트 이름을 "Ghost_플레이어id"로 설정
            
            //만약 newGhost가 Ghost.cs컴포넌트 가지고 있지 않다면 추가
            if (newGhost.GetComponent<Ghost>() == null)
            {
                newGhost.AddComponent<Ghost>();
            }
        }

        return obj;
    }
    

    /// <summary>
    /// 플레이어를 게임상에서 제거하는 함수 (Destroy처리)
    /// </summary>
    /// <param name="dediPlayer">Destroy할 플레이어 오브젝트</param>
    public void DespawnPlayer(GameObject dediPlayerObj)
    {
        Managers.Resource.Destroy(dediPlayerObj);
    }
    public void DespawnGhost(GameObject ghostObj)
    {
        Managers.Resource.Destroy(ghostObj);
    }


    /// <summary>
    /// 다른 플레이어의 움직임을 동기화 (정확히는 고스트를 데디서버와 동기화시킴)
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="transformInfo"></param>
    /// <param name="keyboardInput"></param>
    public void SyncOtherPlayerMove(int playerId, TransformInfo ghostTransformInfo, int keyboardInput, RotationInfo playerRotation)
    {
        if (_ghosts.TryGetValue(playerId, out GameObject ghostObj))
        {
            float posX = ghostTransformInfo.Position.PosX;
            float posY = ghostTransformInfo.Position.PosY;
            float posZ = ghostTransformInfo.Position.PosZ;
            float rotX = ghostTransformInfo.Rotation.RotX;
            float rotY = ghostTransformInfo.Rotation.RotY;
            float rotZ = ghostTransformInfo.Rotation.RotZ;
            float rotW = ghostTransformInfo.Rotation.RotW;
            Quaternion localRotation = new Quaternion(rotX, rotY, rotZ, rotW);

            CharacterController ghostController = ghostObj.GetComponent<CharacterController>();
            ghostController.transform.position = new Vector3(posX, posY, posZ); //고스트 위치 순간이동
            ghostObj.transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW); //고스트 회전

            ghostObj.GetComponent<Ghost>().CalculateVelocity(keyboardInput, localRotation);
            if (_otherDediPlayers.TryGetValue(playerId, out GameObject playerObj))
            {
                playerObj.GetComponent<OtherDediPlayer>().SetGhostLastState(keyboardInput, localRotation);
                
                //플레이어 회전정보 동기화
                float playerRotX = playerRotation.RotX;
                float playerRotY = playerRotation.RotY;
                float playerRotZ = playerRotation.RotZ;
                float playerRotW = playerRotation.RotW;
                playerObj.transform.rotation = new Quaternion(playerRotX, playerRotY, playerRotZ, playerRotW);
            }
        }
    }
    

}
