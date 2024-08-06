using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.Serialization;

public class SteamManager : MonoBehaviour
{
    public static GameObject root;
    public bool _isSteamInitialized = false;

    private Callback<GameLobbyJoinRequested_t> lobbyInviteCallback;

    //Managers Init과 함께 불리는 Init
    public void Init()
    {
        root = GameObject.Find("@Steam");
        if (root == null)
        {
            root = new GameObject { name = "@Steam" };
            Object.DontDestroyOnLoad(root);
        }

        if (SteamAPI.Init())
        {
            _isSteamInitialized = true;
            Debug.Log("Steamworks initialized successfully.");
            
            //내 스팀 이름 가져와서 저장
            SetName();

            // 게임 초대 수락시 호출되는 콜백 함수
            lobbyInviteCallback = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        }
        else
        {
            Debug.LogError("Failed to initialize Steamworks.");
            _isSteamInitialized = false;
        }

        /*
        GetFriendsList();
        InviteFriendToGame();*/
    }

    public void GetFriendsList()
    {
        // 친구 목록을 저장할 리스트
        List<CSteamID> friends = new List<CSteamID>();

        // 친구 수를 가져옵니다
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

        // 모든 친구를 리스트에 추가합니다
        for (int i = 0; i < friendCount; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            friends.Add(friendSteamID);
        }

        // 친구 목록을 출력하거나 필요한 작업을 수행합니다
        foreach (var friend in friends)
        {
            string friendName = SteamFriends.GetFriendPersonaName(friend);
            Debug.Log("Friend: " + friendName);
        }
    }

    public void InviteFriendToGame()
    {
        // 친구 목록을 저장할 리스트
        List<CSteamID> friends = new List<CSteamID>();

        // 친구 수를 가져옵니다
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

        // 모든 친구를 리스트에 추가합니다
        for (int i = 0; i < friendCount; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            friends.Add(friendSteamID);
        }

        // 친구 목록을 출력하거나 필요한 작업을 수행합니다
        foreach (var friend in friends)
        {
            string friendName = SteamFriends.GetFriendPersonaName(friend);
            if (friendName == "텍사스준구앞치마도둑")
            {
                SteamFriends.InviteUserToGame(friend, "");
                Debug.Log("Invited " + friendName + " to the game.");
                return;
            }
        }
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t joinRequested)
    {
        Debug.Log("Game lobby join requested by: " + joinRequested.m_steamIDFriend);
        Debug.Log("Lobby ID: " + joinRequested.m_steamIDLobby); // 이건 스팀에서 제공하는 로비 아이디인데, 자체 로비id로 어떻게
        //읽어올지는 고민해봐야 할듯
    }

    /// <summary>
    /// 내 스팀 이름을 가져와서 저장 및 사용
    /// </summary>
    private void SetName()
    {
        string steamUserName = SteamFriends.GetPersonaName();
        Debug.Log("My Steam Name: " + steamUserName);
        
        //이름 저장
        Managers.Player._myRoomPlayer.Name = steamUserName;
    }
}