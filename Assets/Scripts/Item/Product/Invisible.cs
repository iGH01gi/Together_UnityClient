using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Unity.VisualScripting;
using UnityEngine;

public class Invisible : MonoBehaviour, IItem
{
    //IItem 인터페이스 구현
    public int ItemID { get; set; }
    public int PlayerID { get; set; }
    public string EnglishName { get; set; }

    //이 아이템만의 속성
    public float InvisibleSeconds { get; set; }


    private GameObject _player;
    private GameObject _rootM; //투명 처리를 위해서 껐다 킬 오브젝트
    private float _animationSeconds = 0.7f;

    public void Init(int itemId, int playerId, string englishName)
    {
        this.ItemID = itemId;
        this.PlayerID = playerId;
        this.EnglishName = englishName;
    }

    public void Init(int itemId, int playerId, string englishName, float invisibleSeconds)
    {
        Init(itemId,playerId, englishName);
        InvisibleSeconds = invisibleSeconds;
    }
    
    public bool Use(IMessage recvPacket = null)
    {
        Managers.Player.GetAnimator(PlayerID).SetTriggerByString("Invisible");
        Debug.Log("Item Invisible Use");

        if (PlayerID == Managers.Player._myDediPlayerId)
        {
            //투명 아이템 사용 패킷 서버로 보내기
            CDS_UseInvisibleItem cdsUseInvisibleItem = new CDS_UseInvisibleItem();
            cdsUseInvisibleItem.MyDediplayerId = PlayerID;
            cdsUseInvisibleItem.ItemId = ItemID;
            Managers.Network._dedicatedServerSession.Send(cdsUseInvisibleItem);

            _player = Managers.Player._myDediPlayer;
            _rootM = Util.FindChild(_player, "Root_M", true);
        }
        else
        {
            _player = Managers.Player._otherDediPlayers[PlayerID];
            _rootM = Util.FindChild(_player, "Root_M", true);
        }

        StartCoroutine(ToggleRootM(_rootM));
        return true;
    }

    IEnumerator ToggleRootM(GameObject rootM)
    {
        //_animationSeconds만큼 대기(애니메이션 재생시간)
        yield return new WaitForSeconds(_animationSeconds);

        // Root_M을 비활성화
        rootM.SetActive(false);
        Debug.Log("Root_M has been turned off.");

        // 2초 대기
        yield return new WaitForSeconds(InvisibleSeconds);

        //TODO:동일 현재 아이템이 아닌 playerID의 Invisible아이템이 존재한다면 rootM을 다시 활성화하지 않고 
        GameObject itemRoot = GameObject.Find("@Item");
        bool isUsingMoreInvisible = false; //해당 플레이어가 추가로 투명아이템을 사용했는지 여부
        if (itemRoot != null)
        {
            foreach (Transform child in itemRoot.transform)
            {
                if (child.name == "Invisible" && child.GetComponent<Invisible>().PlayerID == PlayerID && !child.gameObject.Equals(gameObject))
                {
                    Debug.Log("Invisible item is ");
                    isUsingMoreInvisible = true;
                    break;
                }
            }
        }

        if (!isUsingMoreInvisible) //추가로 투명아이템을 사용하지 않았다면
        {
            // Root_M을 다시 활성화
            rootM.SetActive(true);
            Debug.Log("Root_M has been turned on.");
        }

        //투명 끝났으므로 오브젝트 삭제
        Destroy(gameObject);
    }

    public void OnHold()
    {

    }

    public void OnHit()
    {
        
    }
}
