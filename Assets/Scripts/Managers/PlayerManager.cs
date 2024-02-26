using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 룸서버와 데디서버의 플레이어 정보를 들고있는 매니저
/// </summary>
public class PlayerManager
{
    public MyRoomPlayer _myRoomPlayer; //내 룸서버 플레이어
    public Dictionary<int, Player> _otherRoomPlayers = new Dictionary<int, Player>(); //다른 룸서버 플레이어들 (key: playerId, value: 플레이어 정보)
    
    
    public GameObject _myDediPlayer; //내 데디서버 플레이어
    public Dictionary<int,GameObject> _otherDediPlayers = new Dictionary<int, GameObject>(); //다른 데디서버 플레이어들 (key: 데디playerId, value: 플레이어 정보)
    public string _tempMyPlayerPrefabPath = "Player/MyPlayer";
    public string _tempOtherPlayerPrefabPath = "Player/OtherPlayer";

    
    public void Clear()
    {
        _myRoomPlayer = null;
        _otherRoomPlayers.Clear();
        
        ClearDedi();
    }

    public void ClearDedi()
    {
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
    }
    
    
    
    /// <summary>
    /// 데디서버 플레이어를 실제로 생성하는 함수
    /// </summary>
    /// <param name="dediPlayer">갖고 있어야할 플레이어 정보</param>
    /// <returns></returns>
    public GameObject SpawnPlayer(DediPlayer dediPlayer)
    {
        GameObject obj = null;
        if(dediPlayer.IsMyPlayer) //본인 플레이어용 프리팹 이용
        {
            obj = Managers.Resource.Instantiate(_tempMyPlayerPrefabPath);
        }
        else //다른 플레이어용 프리팹 이용
        {
            obj = Managers.Resource.Instantiate(_tempOtherPlayerPrefabPath);
        }
        DediPlayer dediPlayerComponent = obj.AddComponent<DediPlayer>();
        dediPlayerComponent.CopyFrom(dediPlayer);

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

}
