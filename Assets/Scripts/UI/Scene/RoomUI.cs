using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using JetBrains.Annotations;
using UnityEngine;

public class RoomUI : UI_scene
{
    private GameRoom thisRoom;
    GameObject myPlayer;
    GameObject readyButton;
    GameObject startGameButton;

    private enum Buttons
    {
        BackToLobbyButton,
        //TempGameStart,
        RefreshButton,
        ReadyButton,
        StartGame
    }

    void Start()
    {
        thisRoom = Managers.Player._myRoomPlayer.Room;
        InitButtons<Buttons>(gameObject);
        readyButton = transform.Find("ReadyButton").gameObject;
        startGameButton = transform.Find("StartGame").gameObject;
        GetPlayerList();
    }
    
    //만약 방장이라면 StartGameButton을, 아니라면 ReadyButton
    public void ButtonIfMaster()
    {
        if (Managers.Room.IsMyPlayerMaster())
        {
           readyButton.SetActive(false);
            startGameButton.SetActive(true);
        }
        else
        { 
            startGameButton.SetActive(false);
            readyButton.SetActive(true);
        }
    }

    /// <summary>
    /// 플레이어에 대한 PlayerInRoom 생성. 방장 표시 포함.
    /// </summary>
    public void GetPlayerList()
    {
        Transform PlayersPanel = transform.Find("PlayersPanel");
        ClearPlayerListPanel(PlayersPanel);
        foreach (var current in thisRoom._players)
        {
            GameObject currentPlayer = Managers.Resource.Instantiate("UI/Subitem/PlayerInRoom");
            currentPlayer.transform.SetParent(PlayersPanel);
            currentPlayer.GetComponent<PlayerInRoom>().Init(current);
            if (!Managers.Room.IsMaster(thisRoom.Info.RoomId, current.PlayerId))
            {
                currentPlayer.transform.Find("MasterIcon").gameObject.SetActive(false);
            }
            if (current.PlayerId == Managers.Player._myRoomPlayer.PlayerId)
            {
                ButtonIfMaster();
                myPlayer = currentPlayer;
            }
        }
    }

    void BackToLobbyButton()
    {
        UIPacketHandler.LeaveRoomSendPacket(thisRoom.Info.RoomId);
    }

    void ReadyButton()
    {
        if (myPlayer.transform.GetChild(1).gameObject.activeSelf)
        {
            myPlayer.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            myPlayer.transform.GetChild(1).gameObject.SetActive(true);
            Managers.Sound.Play("Effects/interface");
        }
    }

    public void RefreshButton()
    {
        GetPlayerList();
    }

    
    public void StartGame()
    {
        //TODO: 모든 플레이어가 Ready일 시 시작하도록 수정
        Debug.Log("겜시작버튼 눌림");
        CS_ConnectDedicatedServer sendPacket = new CS_ConnectDedicatedServer();
        sendPacket.RoomId = thisRoom.Info.RoomId;
        Managers.Network._roomSession.Send(sendPacket);
    }

    void ClearPlayerListPanel(Transform roomsPanel)
    {
        foreach (Transform child in roomsPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}