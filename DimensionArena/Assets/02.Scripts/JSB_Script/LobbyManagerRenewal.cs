using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

// Test
using Firebase;
using Firebase.Database;
//

public enum MODE
{
    MODE_SURVIVAL,
    MODE_FREEFALLALL,
    MODE_TEAMDEATHMATCH,
    MODE_SUPERSTAR,
    MODE_TRAINING,
    MODE_END
}

public class LobbyManagerRenewal : MonoBehaviourPunCallbacks
{
    public static LobbyManagerRenewal Instance;

    [SerializeField] public TextMeshProUGUI loadText;

    //private string playerName = "Guest";
    //public string PlayerName { get { return playerName; } }

    
    // Survival , FreeforAll , TeamDeathMatch , SuperStar
    private string[] modeRoomNames = { "SV", "FF", "TD", "SS" };

    private List<Dictionary<string,CustomRoomInfo>> rooms = new List<Dictionary<string, CustomRoomInfo>>();

    public MODE playMode;

    [SerializeField] private int leastStartPlayer = 4;
    public int LeastStartPlayer { get { return leastStartPlayer; } }
    [SerializeField] private int maxStartPlayer = 8;
    public int MaxStartPlayer { get { return maxStartPlayer; } }

    [SerializeField] private List<string> playersName;

    private bool isWillStartGame = false;
    public bool IsWillStartGame { get { return isWillStartGame; } }

    private string nowInRoomName = "";


    // 게임 끝나면 초기화 시켜줘야함
    private int inGameReadyPlayer = 0;
    public int InGameReadyPlayer { get { return inGameReadyPlayer; } }
    private int nowGameStartCount = 0;
    public int NowGameStartCount { get { return nowGameStartCount; } }

    //Test 
    [SerializeField] Dictionary<string, PlayerData> playerDatas = new Dictionary<string, PlayerData>();

    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = FirebaseDB_Manager.Instance.PlayerNickName;
        for (int i = (int)MODE.MODE_SURVIVAL; i < (int)MODE.MODE_END; ++i)
        {
            rooms.Add(new Dictionary<string, CustomRoomInfo>());
        }

        loadText.text = "서버 접속 시도중...";
        // 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        loadText.text = "서버 로비 접속중...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 성공");

        loadText.text = "서버 접속 성공";
        
        /*if(isReconnect)
        {
            PhotonNetwork.NickName = playerName;
            isReconnect = false;
            return;
        }*/
        //playerName = MakeRandNickname();
        LoadingSceneController.Instance.LoadScene("Lobby_Main");
    }

    /*private string MakeRandNickname()
    {
        loadText.text = "랜덤 이름 생성중...";
        return FirebaseDB_Manager.Instance.RegisterDataBase(loadText);
    } */

    [PunRPC]
    public void PlayerNameAdd(string name)
    {
        playersName.Add(name);
    }

    // 해당 함수는 로비에 돌아갔을 시 자동적으로 호출되는 함수이며 , 로비 내부에서는 갱신을 할 수 없다.
    // -> 그럼 이거 로비에 둘 다 있을 때 방을 만들면 호출이 되는건가..;;
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        SetUpRoomList(roomList);
        //rooms[(int)playMode] = roomList;
    }

    // 룸은 2글자로 밖에 관리를 못한다 즉 룸 앞의 이름이 절대 겹쳐서는 안된다
    // 여기서는 확실성이 없는 OnRoomListUpdate 의 목록에서 반드시 추가된 이름만 추가를 하자.
    // 아니다 , 플레이어 카운트도 확실하게 받자.
    [PunRPC]
    private void SetUpRoomList(List<RoomInfo> roomList)
    {
        string roomName = "";
        foreach(RoomInfo info in roomList)
        {
            roomName += info.Name[0].ToString() + info.Name[1].ToString();
            switch(roomName)
            {
                case "SV":
                    if (!rooms[(int)MODE.MODE_SURVIVAL].ContainsKey(info.Name))
                        rooms[(int)MODE.MODE_SURVIVAL].Add(info.Name, new CustomRoomInfo(roomName, info.PlayerCount, info.IsOpen));
                    else
                        rooms[(int)MODE.MODE_SURVIVAL][info.Name].IsOpen = info.IsOpen;
                    break;
                case "FF":
                    if (!rooms[(int)MODE.MODE_FREEFALLALL].ContainsKey(info.Name))
                        rooms[(int)MODE.MODE_FREEFALLALL].Add(info.Name, new CustomRoomInfo(roomName, info.PlayerCount, info.IsOpen));
                    else
                        rooms[(int)MODE.MODE_FREEFALLALL][info.Name].IsOpen = info.IsOpen;
                    break;
                case "TD":
                    if (!rooms[(int)MODE.MODE_TEAMDEATHMATCH].ContainsKey(info.Name))
                        rooms[(int)MODE.MODE_TEAMDEATHMATCH].Add(info.Name, new CustomRoomInfo(roomName, info.PlayerCount, info.IsOpen));
                    else
                        rooms[(int)MODE.MODE_TEAMDEATHMATCH][info.Name].IsOpen = info.IsOpen;
                    break;
                case "SS":
                    if (!rooms[(int)MODE.MODE_SUPERSTAR].ContainsKey(info.Name))
                        rooms[(int)MODE.MODE_SUPERSTAR].Add(info.Name, new CustomRoomInfo(roomName, info.PlayerCount, info.IsOpen));
                    else
                        rooms[(int)MODE.MODE_SUPERSTAR][info.Name].IsOpen = info.IsOpen;
                    break;
            }
        }
    }

    // 남은 룸이 있는지 확인하는 함수이다.
    private string CheckingRoom(MODE gameMode)
    {
        if (0 == rooms.Count)
            return "empty";
        foreach (Dictionary<string, CustomRoomInfo> room in rooms)
        {
            foreach(string key in room.Keys)
            {
                if (room[key].IsOpen)
                {
                    return key;
                }
            }
        } 
        return "empty";
    }

    private CustomRoomInfo GetNewRoomName()
    {
        int roomCount = 0;
        int playerCount = 0;
        for(int i = (int)MODE.MODE_SURVIVAL; i < (int)MODE.MODE_END; ++i)
        {
            roomCount += rooms[i].Count;
              foreach(string key in rooms[i].Keys)
            {
                playerCount += rooms[i][key].PlayerCount;
            }
            //for(int j = 0; j < rooms[i].Count;++j)
            //    playerCount += rooms[i][j].PlayerCount;
        }


        // 최소한의 방이 시작되는 조건이 4명 임으로
        if(15 < playerCount && 4 < roomCount)
            return null;

        // 새로운 방의 인덱스를 구하기 위해 맨 끝 이름을 받는다.
        int newRoomIndex = 0;
        foreach (string key in rooms[(int)playMode].Keys)
        {
            Int32.TryParse(key[2].ToString(),out newRoomIndex);
        }

        ++newRoomIndex;


        switch (playMode)
        {
            case MODE.MODE_SURVIVAL:
                return new CustomRoomInfo(modeRoomNames[(int)MODE.MODE_SURVIVAL] + newRoomIndex.ToString(),0,true);
            case MODE.MODE_FREEFALLALL:
                return new CustomRoomInfo(modeRoomNames[(int)MODE.MODE_FREEFALLALL] + newRoomIndex.ToString(), 0, true);
            case MODE.MODE_TEAMDEATHMATCH:
                return new CustomRoomInfo(modeRoomNames[(int)MODE.MODE_TEAMDEATHMATCH] + newRoomIndex.ToString(), 0, true);
            case MODE.MODE_SUPERSTAR:
                return new CustomRoomInfo(modeRoomNames[(int)MODE.MODE_FREEFALLALL] + newRoomIndex.ToString(), 0, true);
            default:
                return null;
        }
    }

    public void Reconnect()
    {
        if(PhotonNetwork.IsConnected == false)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterTrainingMode()
    {
        if (playMode == MODE.MODE_TRAINING)
        {
            SceneManager.LoadScene("OfflineMode");
            return;
        }
    }

    public void JoinOrCreateRoom(MODE gameMode)
    {
        inGameReadyPlayer = 0;
        FirebaseDB_Manager.Instance.IsInGame = true;    

        playMode = gameMode;
        string roomName = CheckingRoom(gameMode);
        nowInRoomName = roomName;

        if (roomName == "empty")
        {
            // 임시로 방 생성은 일정한 이름으로 만들어 놨음
            CustomRoomInfo newRoomInfo = GetNewRoomName();
            Debug.Log(newRoomInfo.RoomName);
            rooms[(int)gameMode].Add(newRoomInfo.RoomName,newRoomInfo);
            string newRoomName = newRoomInfo.RoomName;
            bool roomMake = PhotonNetwork.CreateRoom(newRoomName, new RoomOptions { MaxPlayers = 8 }, null);
            if(!roomMake)
            {
                // 방 생성 실패

            }
            else
            { }
        }
        else
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 8 }, null);
    }
    /*public void ChangeNickNmae(string name)
    {
        
        playerName = name;
        PhotonNetwork.NickName = playerName;

    }*/

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("접속실패");
    }
    public override void OnJoinedRoom()
    {

        //  아 방이 만들어졌는데 또 들어가는 오류인건가...
        //  JoinRandomRoom failed. Client is on GameServer (must be Master Server for matchmaking) and ready


        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject obj = GameObject.Find("MatchState");

        obj.GetComponent<MatchStateText>().RefreshTextAllClient(playerCount, maxStartPlayer);

        if (leastStartPlayer <= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            nowGameStartCount = PhotonNetwork.CurrentRoom.PlayerCount;
            isWillStartGame = true;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            photonView.RPC("PlayStart",RpcTarget.All);
            // 게임이 시작했으면 방을 닫는다. -> 방을 닫고 씬을 로드해야지 댕청아
            //PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    [PunRPC]
    private void RegisterCustomRoomInfo(CustomRoomInfo info,MODE gameMode)
    {
        if (!rooms[(int)gameMode].ContainsKey(info.RoomName))
            rooms[(int)gameMode].Add(info.RoomName, info);
    }

    public bool TryGetOutRoom()
    {
        if(false == PhotonNetwork.InRoom || true == PhotonNetwork.LeaveRoom())
        {
            return true;
        }
        else
            return false;
    }

    [PunRPC]
    private void PlayStart()
    {
        switch(playMode)
        {
            case MODE.MODE_SURVIVAL:
                PhotonNetwork.LoadLevel("Prototype");
                break;
            
        }
    }
    
    public void ReadyToPlay()
    {
        photonView.RPC(nameof(PlayersReadyForStartGame), RpcTarget.All);
    }

    [PunRPC]
    public void PlayersReadyForStartGame()
    {
        ++this.inGameReadyPlayer;

    }
    [PunRPC]
    public void ExitGameAndMigration()
    {
    }
    public void MinusLeastStartPlayer()
    {
        --leastStartPlayer;
    }
    public void PlusLeastStartPlayer()
    {
        ++leastStartPlayer;
    }
}
