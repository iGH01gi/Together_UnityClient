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

    void Init()
    {
        thisRoom = Managers.Player._myRoomPlayer.Room;
        InitButtons<Buttons>(gameObject);
        readyButton = transform.Find("ReadyButton").gameObject;
        startGameButton = transform.Find("StartGame").gameObject;
    }
    void Start()
    {
        Init();
        GetPlayerList();
    }
    
    //만약 방장이라면 StartGameButton을, 아니라면 ReadyButton
    public void IfMaster()
    {
        if (Managers.Room.IsMyPlayerMaster())
        {
           readyButton.SetActive(false);
           startGameButton.SetActive(true);
           startGameButton.GetComponent<UI_Button>().Activation(Managers.Room.IsMyRoomAllPlayerReady());
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
        if (thisRoom == null)
        {
            Init();
        }
        Transform PlayersPanel = transform.Find("PlayersPanel");
        ClearPlayerListPanel(PlayersPanel);
        foreach (var current in thisRoom._players)
        {
            GameObject currentPlayer = Managers.Resource.Instantiate("UI/Subitem/PlayerInRoom");
            currentPlayer.transform.SetParent(PlayersPanel);
            currentPlayer.GetComponent<PlayerInRoom>().Init(current);
            
            if (current.PlayerId == Managers.Player._myRoomPlayer.PlayerId)
            {
                IfMaster();
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
        readyButton.GetComponent<UI_Button>().PlayButtonClick();
        myPlayer.GetComponent<PlayerInRoom>().ToggleReady();
    }

    public void RefreshButton()
    {
        GetPlayerList();
    }

    
    public void StartGame()
    {
        if (!Managers.Room.IsMyRoomAllPlayerReady())
        {
            return;
        }
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