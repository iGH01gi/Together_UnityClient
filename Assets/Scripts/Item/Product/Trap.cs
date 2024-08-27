using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Trap : MonoBehaviour, IItem
{
    //IItem 인터페이스 구현
    public int ItemID { get; set; }
    public int PlayerID { get; set; }
    public string EnglishName { get; set; }

    public float TrapDuration { get; set; }
    public float TrapRadius { get; set; }
    public float StunDuration { get; set; }


    private GameObject _myDediPlayer;
    private float _forwardRayDistance = 0.5f;
    private float _downRayDistance = 0.2f;
    private bool _onHoldTrigger = false;
    private bool _isMyPlayerTrapAvailable = false; //현재 트랩을 내 캐릭터가 설치할 수 있는지를 확인하는 변수

    public void Init(int itemId,  int playerId, string englishName)
    {
        this.ItemID = itemId;
        this.PlayerID = playerId;
        this.EnglishName = englishName;
    }

    public void Init(int itemId, int playerId, string englishName, float trapDuration, float trapRadius, float stunDuration)
    {
        Init(itemId,playerId,englishName);
        TrapDuration = trapDuration;
        TrapRadius = trapRadius;
        StunDuration = stunDuration;
    }

    public void Update()
    {
        if (_onHoldTrigger)
        {
            // _myDediPlayer의 forward 방향으로 ray를 쏴서 아무 것도 없고 && 그곳에서부터 아래로 ray를 쏴서 땅이 닿는다면 _isMyPlayerTrapAvailable를 true로 설정
            RaycastHit hit;
            Vector3 forwardRayStart = _myDediPlayer.transform.position + Vector3.up * 0.2f;
            Vector3 forwardRayDirection = _myDediPlayer.transform.forward;
            float forwardRayDistance = _forwardRayDistance;

            // forward 방향으로 ray를 쏘기
            Debug.DrawRay(forwardRayStart, forwardRayDirection * forwardRayDistance, Color.red);

            if (Physics.Raycast(forwardRayStart, forwardRayDirection, out hit, forwardRayDistance))
            {
                _isMyPlayerTrapAvailable = false;

                // ray가 충돌한 위치에서부터 아래로 ray를 쏘기
                Vector3 downRayStart = hit.point;
                Vector3 downRayDirection = Vector3.down;
                float downRayDistance = _downRayDistance;

                Debug.DrawRay(downRayStart, downRayDirection * downRayDistance, Color.blue);

                if (Physics.Raycast(downRayStart, downRayDirection, out hit, downRayDistance))
                {
                    // 땅에 설치 불가능하다는 시각적 표시
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.transform.Find("TrapGreen").GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("TrapRed").GetComponent<MeshRenderer>().enabled = true;

                    gameObject.transform.position = hit.point;
                }
                else
                {
                    // 밑이 허공이니까 딱히 아무것도 표시하지 않아도 됨 (원래 있던걸 다 비활성화)
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.transform.Find("TrapGreen").GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("TrapRed").GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else
            {
                // 앞 ray가 충돌하지 않았다면 해당 ray의 끝점에서 아래로 ray를 쏴서 땅이 닿는지 확인
                Vector3 downRayStart = forwardRayStart + forwardRayDirection * forwardRayDistance;
                Vector3 downRayDirection = Vector3.down;
                float downRayDistance = _downRayDistance;

                Debug.DrawRay(downRayStart, downRayDirection * downRayDistance, Color.green);

                if (Physics.Raycast(downRayStart, downRayDirection, out hit, downRayDistance))
                {
                    _isMyPlayerTrapAvailable = true;

                    // 땅에 설치 가능하다는 시각적 표시
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.transform.Find("TrapGreen").GetComponent<MeshRenderer>().enabled = true;
                    gameObject.transform.Find("TrapRed").GetComponent<MeshRenderer>().enabled = false;

                    gameObject.transform.position = hit.point;
                }
                else
                {
                    _isMyPlayerTrapAvailable = false;

                    // 밑이 허공이니까 딱히 아무것도 표시하지 않아도 됨 (원래 있던걸 다 비활성화)
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.transform.Find("TrapGreen").GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("TrapRed").GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
    
    public bool Use(IMessage recvPacket = null)
    {
        if (PlayerID == Managers.Player._myDediPlayerId && _isMyPlayerTrapAvailable)
        {
            _onHoldTrigger = false;
            _isMyPlayerTrapAvailable = false;

            Managers.Player.GetAnimator(PlayerID).SetTriggerByString(EnglishName);
            //TODO: onhold가 표시되던 위치에 트랩을 생성

            //TODO: trapGameObject의 Mesh Renderer와 Sphere Collider를 활성화

            //TODO: trapGameObject의 자식인 TrapGreen와 TrapRed의 Mesh Renderer를 비활성화

            //TODO: 트랩이 설치된 트랜스폼을 패킷에 담아서 서버로 전송

            Debug.Log("Item Trap Use");
            return true;
        }
        else
        {
            Managers.Player.GetAnimator(PlayerID).SetTriggerByString(EnglishName);
            //TODO: trapGameObject의 Mesh Renderer와 Sphere Collider를 활성화

            //TODO: 패킷에 담긴 설치 트랜스폼에 트랩을 설치

            Debug.Log("Item Trap Use");
            return true;
        }
        
        return false;
    }

    public void OnHold()
    {
        if (PlayerID != Managers.Player._myDediPlayerId)
        {
            Destroy(gameObject);
        }

        _myDediPlayer = Managers.Player._myDediPlayer;

        _onHoldTrigger = true;
    }

    public void OnHit()
    {
        
    }
}
