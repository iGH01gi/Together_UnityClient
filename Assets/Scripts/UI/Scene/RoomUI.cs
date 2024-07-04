using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using JetBrains.Annotations;
using UnityEngine;

public class RoomUI : UI_scene
{
    // Start is called before the first frame update
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
    
    public void ButtonIfMaster()
    {
        if (Managers.Room.IsMyPlayerMaster())
        {
            Debug.Log("Master eyyyy");
           readyButton.SetActive(false);
            startGameButton.SetActive(true);
        }
        else
        { 
            Debug.Log("Not Master");
            startGameButton.SetActive(false);
            readyButton.SetActive(true);
        }
    }

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
            //준비완료 -> 준비취소
            CS_ReadyRoom readyRoomPacket = new CS_ReadyRoom();
            readyRoomPacket.RoomId = thisRoom.Info.RoomId;
            readyRoomPacket.PlayerId = Managers.Player._myRoomPlayer.PlayerId;
            readyRoomPacket.IsReady = false;
            Managers.Network._roomSession.Send(readyRoomPacket);
            
            myPlayer.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            //준비x -> 준비완료
            CS_ReadyRoom readyRoomPacket = new CS_ReadyRoom();
            readyRoomPacket.RoomId = thisRoom.Info.RoomId;
            readyRoomPacket.PlayerId = Managers.Player._myRoomPlayer.PlayerId;
            readyRoomPacket.IsReady = true;
            Managers.Network._roomSession.Send(readyRoomPacket);
            
            myPlayer.transform.GetChild(1).gameObject.SetActive(true);
            Managers.Sound.Play("Effects/interface");
        }
    }

    public void RefreshButton()
    {
        GetPlayerList();
    }

    //레디 상태인지 확인 가능 할 시 추가
    public void StartGame()
    {
        //Implement Game start
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