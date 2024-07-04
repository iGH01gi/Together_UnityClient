using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 모든 룸들을 볼 수 있는 로비 씬 UI
/// </summary>
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
        InitButtons<Buttons>(gameObject);
        
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

    //페이지 넘버 업데이트
    private void DisplayPageNumber()
    {
        pageText.text = $"{currentPage}/{maxPage}";
    }

    //현재 페이지에 해당하는 룸들 띄우기.
    public void ShowPage()
    {
        maxPage = Math.Max(1,(_gameRooms.Count + roomsPerPage - 1) / roomsPerPage);
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

    //룸 리스트를 다시 불러온다.
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

    //페이지 좌 우 이동 버튼이 활성화 되어야 하는지 확인
    void CheckForButtonActivation()
    {
        leftPageButton.Activation((currentPage>1));
        rightPageButton.Activation((maxPage - currentPage) > 0);
    }

    //모든 룸 UI들을 삭제
    void ClearRoomListPanel()
    {
        foreach (Transform child in roomsPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
