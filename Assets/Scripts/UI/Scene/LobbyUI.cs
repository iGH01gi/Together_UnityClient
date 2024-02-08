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

    private int roomsPerPage = 5;
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
        pageText = transform.GetChild(2).GetComponent<TMP_Text>();
        leftPageButton = transform.GetChild(1).GetComponent<UI_Button>();
        rightPageButton = transform.GetChild(3).GetComponent<UI_Button>();
        
        
        currentPage = 1;
        maxPage = 1;
        CheckForButtonActivation();
        ShowPage();
        
        
        InitButtons<Buttons>();
        UIPacketHandler.RoomListSendPacket();
    }

    private void MainMenuButton()
    {
        Managers.UI.LoadScenePanel(SceneUIType.MainMenuUI.ToString());
    }
    
    private void LeftPageButton()
    {
        ShowPage(--currentPage);
        DisplayPageNumber();
    }
    
    private void RightPageButton()
    {
        ShowPage(++currentPage);
        DisplayPageNumber();
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

    public void ShowPage(int pageNum = 1)
    {
        Transform roomsPanel = transform.GetChild(6);
        for (int i = 0; i < roomsPanel.transform.childCount; i++)
        {
            Destroy(roomsPanel.GetChild(i));
        }

        for (int i = (currentPage-1)*roomsPerPage; i < Math.Min(currentPage*roomsPerPage,Managers.Room._rooms.Count); i++)
        {
            GameObject currentRoom = Managers.Resource.Instantiate("Subitem/Room_Info");
            currentRoom.GetComponent<Room_Info>().Init(Managers.Room._rooms[i]);
        }
    }

    public void ReceiveNewRoomList()
    {
        Transform roomsPanel = transform.GetChild(6);
        for (int i = 0; i < roomsPanel.transform.childCount; i++)
        {
            Destroy(roomsPanel.GetChild(i));
        }

        currentPage = 1;
        maxPage = (Managers.Room._rooms.Count + roomsPerPage - 1) / roomsPerPage;
        CheckForButtonActivation();
        ShowPage();
    }

    void CheckForButtonActivation()
    {
        leftPageButton.Activation((currentPage>1));
        rightPageButton.Activation((maxPage - currentPage) > 0);
    }
}
