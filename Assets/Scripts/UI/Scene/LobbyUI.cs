using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : UI_scene
{
    private int currentPage;
    private int maxPage;
    private TMP_Text pageText;
    private UI_Button leftPageButton;
    private UI_Button rightPageButton;

    private static int roomsPerPage = 5;
    private List<GameRoom> _gameRoom = new List<GameRoom>();
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
        
        currentPage = 1;
        maxPage = 1;
        DisplayPageNumber();
        CheckForButtonActivation();
        
        UIPacketHandler.RoomListSendPacket();
    }

    private void MainMenuButton()
    {
        Managers.UI.LoadScenePanel(SceneUIType.MainMenuUI.ToString());
    }
    
    private void LeftPageButton()
    {
        currentPage--;
        DisplayPageNumber();
        CheckForButtonActivation();
        ShowPage();
    }
    
    private void RightPageButton()
    {
        currentPage++;
        DisplayPageNumber();
        CheckForButtonActivation();
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
        Transform roomsPanel = transform.GetChild(6);
        ClearRoomListPanel();
        for (int i = (currentPage-1)*roomsPerPage; i < Math.Min(currentPage*roomsPerPage,_gameRoom.Count); i++)
        {
            GameObject currentRoom = Managers.Resource.Instantiate("UI/Subitem/Room_Info");
            currentRoom.transform.SetParent(roomsPanel);
            currentRoom.GetComponent<Room_Info>().Init(_gameRoom[i]);
        }
    }

    public void ReceiveNewRoomList()
    {
        _gameRoom.Clear();

        foreach (var current in Managers.Room._rooms)
        {
            _gameRoom.Add(current.Value);
        }

        currentPage = 1;
        maxPage = (_gameRoom.Count + roomsPerPage - 1) / roomsPerPage;
        CheckForButtonActivation();
        DisplayPageNumber();
        ShowPage();
    }

    void CheckForButtonActivation()
    {
        leftPageButton.Activation((currentPage>1));
        rightPageButton.Activation((maxPage - currentPage) > 0);
    }

    void ClearRoomListPanel()
    {
        Transform roomsPanel = transform.GetChild(6);
        int loop = roomsPanel.transform.childCount;
        for (int i = 0; i < loop; i++)
        {
            Destroy(roomsPanel.GetChild(0).gameObject);
        }
    }
}
