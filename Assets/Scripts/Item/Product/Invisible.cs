using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using Unity.VisualScripting;
using UnityEngine;

public class Invisible : MonoBehaviour, IItem
{
    //IItem �������̽� ����
    public int ItemID { get; set; }
    public int PlayerID { get; set; }
    public string EnglishName { get; set; }

    //�� �����۸��� �Ӽ�
    public float InvisibleSeconds { get; set; }


    private GameObject _player;
    private GameObject _rootM; //���� ó���� ���ؼ� ���� ų ������Ʈ

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
    
    public void Use()
    {
        Managers.Player.GetAnimator(PlayerID).SetTriggerByString("Invisible");
        Debug.Log("Item Invisible Use");

        if (PlayerID == Managers.Player._myDediPlayerId)
        {
            //���� ������ ��� ��Ŷ ������ ������
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
    }

    IEnumerator ToggleRootM(GameObject rootM)
    {
        // Root_M�� ��Ȱ��ȭ
        rootM.SetActive(false);
        Debug.Log("Root_M has been turned off.");

        // 2�� ���
        yield return new WaitForSeconds(InvisibleSeconds);

        //TODO: ���� playerID�� Invisible�������� �����Ѵٸ� rootM�� �ٽ� Ȱ��ȭ���� �ʰ� Destroy


        // Root_M�� �ٽ� Ȱ��ȭ
        rootM.SetActive(true);
        Debug.Log("Root_M has been turned on.");

        //���� �������Ƿ� ������Ʈ ����
        Destroy(gameObject);
    }

    public void OnHit()
    {
        
    }
}
