using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : UI_scene
{
    static int currentPage;
    static int maxPage;
    private TMP_Text pageText;
    private UI_Button leftPageButton;
    private UI_Button rightPageButton;
    Transform roomsPanel;

    private static int roomsPerPage = 5;

    private List<GameRoom> _gameRooms = new List<GameRoom>();
    private enum Buttons
    {
        MainMenuButton,
        LeftPageButton,
        RightPageButton,
        CreateRoom,
        RefreshButton
    }

    private void Start()
    {
        InitButtons<Buttons>();
        
        pageText = transform.GetChild(2).GetComponent<TMP_Text>();
        leftPageButton = transform.GetChild(1).GetComponent<UI_Button>();
        rightPageButton = transform.GetChild(3).GetComponent<UI_Button>();
        roomsPanel = transform.GetChild(6);
        
        UIPacketHandler.RoomListSendPacket();
    }

    private void MainMenuButton()
    {
        Managers.UI.LoadScenePanel(SceneUIType.MainMenuUI.ToString());
    }
    
    private void LeftPageButton()
    {
        currentPage--;
        ShowPage();
    }
    
    private void RightPageButton()
    {
        currentPage++;
        ShowPage();
    }
    
    private void CreateRoom()
    {
        Managers.UI.LoadPopupPanel<CreateRoomPopup>(true);
    }
    
    private void RefreshButton()
    {
        UIPacketHandler.RoomListSendPacket();
    }

    private void DisplayPageNumber()
    {
        pageText.text = $"{currentPage}/{maxPage}";
    }

    public void ShowPage()
    {
        maxPage = (_gameRooms.Count + roomsPerPage - 1) / roomsPerPage;
        DisplayPageNumber();
        CheckForButtonActivation();
        ClearRoomListPanel();
        for (int i = (currentPage-1)*roomsPerPage; i < Math.Min(currentPage*roomsPerPage,_gameRooms.Count); i++)
        {
            GameObject currentRoom = Managers.Resource.Instantiate("UI/Subitem/Room_Info");
            currentRoom.transform.SetParent(roomsPanel);
            currentRoom.GetComponent<Room_Info>().Init(_gameRooms[i]);
        }
    }

    public void ReceiveNewRoomList()
    {
        currentPage = 1;
        _gameRooms.Clear();
        foreach (var current in Managers.Room._rooms)
        {
            _gameRooms.Add(current.Value);
        }
        
        ShowPage();
    }

    void CheckForButtonActivation()
    {
        leftPageButton.Activation((currentPage>1));
        rightPageButton.Activation((maxPage - currentPage) > 0);
    }

    void ClearRoomListPanel()
    {
        foreach (Transform child in roomsPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
