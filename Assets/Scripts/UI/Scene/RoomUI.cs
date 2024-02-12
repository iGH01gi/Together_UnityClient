using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RoomUI : UI_scene
{
    // Start is called before the first frame update
    private GameRoom thisRoom;
    GameObject myPlayer;
    
    private enum Buttons
    {
        BackToLobbyButton,
        ReadyButton,
        TempGameStart,
        RefreshButton
    }
    void Start()
    {
        InitButtons<Buttons>();
        thisRoom = Managers.Player._myPlayer.Room;
        GetPlayerList();
    }

    private void GetPlayerList()
    {
        Transform PlayersPanel = transform.GetChild(4);
        ClearPlayerListPanel(PlayersPanel);
        foreach (var current in thisRoom._players)
        {
            GameObject currentPlayer = Managers.Resource.Instantiate("UI/Subitem/PlayerInRoom");
            currentPlayer.transform.SetParent(PlayersPanel);
            currentPlayer.GetComponent<PlayerInRoom>().Init(current);
            if (current.PlayerId == Managers.Player._myPlayer.PlayerId)
            {
                myPlayer = currentPlayer;
            }
        }
    }

    void BackToLobbyButton()
    {
        Managers.UI.LoadScenePanel(SceneUIType.LobbyUI.ToString());
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

    void RefreshButton()
    {
        GetPlayerList();
    }

    void TempGameStart()
    {
        //Implement Game start
    }

    void ClearPlayerListPanel(Transform roomsPanel)
    {
        foreach (Transform child in roomsPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
