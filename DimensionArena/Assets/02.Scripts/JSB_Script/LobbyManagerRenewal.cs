using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


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

    [SerializeField] TextMeshProUGUI loadText;

    private string randomName = "Guest";

    bool isConnect = false;
    public bool IsConnect { get { return isConnect; } }

    // Survival , FreeforAll , TeamDeathMatch , SuperStar
    private string[] modeRoomNames = { "SV", "FF", "TD", "SS" };

    private List<List<RoomInfo>> rooms = new List<List<RoomInfo>>();
    
    public MODE playMode;

    [SerializeField] private int leastStartPlayer = 4;
    public int LeastStartPlayer { get { return leastStartPlayer; } }
    [SerializeField] private int maxStartPlayer = 8;
    public int MaxStartPlayer { get { return maxStartPlayer; } }

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
        for (int i = (int)MODE.MODE_SURVIVAL; i < (int)MODE.MODE_END; ++i)
        {
            rooms.Add(new List<RoomInfo>());
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
        loadText.text = "서버 접속 성공";

        MakeRandNickname();
    }

    private void MakeRandNickname()
    {

        loadText.text = "랜덤 이름 생성중...";
        do
        {
            randomName += Random.Range(1, 100).ToString();
        } while (NameOverLapCheck(randomName));
    }


    private bool NameOverLapCheck(string name)
    {
        loadText.text = "이름 중복 확인중...";


        // 플레이어 이름 목록들을 받아온다.
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in players)
        {
            if (p.NickName == name)
            {
                loadText.text = "이름 중복";
                return true;
            }
        }
        loadText.text = "중복 체크 완료";
        PhotonNetwork.NickName = name;
        isConnect = true;
        LoadingSceneController.Instance.LoadScene("Lobby_Main");
        return false;
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
    private void SetUpRoomList(List<RoomInfo> roomList)
    {
        string roomName = "";
        foreach(RoomInfo info in roomList)
        {
            roomName += info.Name[0].ToString() + info.Name[1].ToString();
            switch(roomName)
            {
                case "SV":
                    rooms[(int)MODE.MODE_SURVIVAL].Add(info);
                    break;
                case "FF":
                    rooms[(int)MODE.MODE_FREEFALLALL].Add(info);
                    break;
                case "TD":
                    rooms[(int)MODE.MODE_TEAMDEATHMATCH].Add(info);
                    break;
                case "SS":
                    rooms[(int)MODE.MODE_SUPERSTAR].Add(info);
                    break;
            }
        }
    }

    // 남은 룸이 있는지 확인하는 함수이다.
    private string CheckingRoom(MODE gameMode)
    {
        OnRoomListUpdate(new List<RoomInfo>());
        if (0 == rooms.Count)
            return "empty";
        foreach (RoomInfo room in rooms[(int)gameMode])
        {
            if (room.IsOpen)
                return room.Name;
        } 
        return "empty";
    }

    private string GetNewRoomName()
    {
        int roomCount = 0;
        int playerCount = 0;
        for(int i = (int)MODE.MODE_SURVIVAL; i < (int)MODE.MODE_END; ++i)
        {
            roomCount += rooms[i].Count;
            for(int j = 0; j < rooms[i].Count;++j)
                playerCount += rooms[i][j].PlayerCount;
        }


        // 최소한의 방이 시작되는 조건이 4명 임으로
        if(15 < playerCount && 4 < roomCount)
            return "error";

        switch (playMode)
        {
            case MODE.MODE_SURVIVAL:
                return modeRoomNames[(int)MODE.MODE_SURVIVAL] + (rooms[(int)MODE.MODE_SURVIVAL].Count + 1).ToString();
            case MODE.MODE_FREEFALLALL:
                return modeRoomNames[(int)MODE.MODE_FREEFALLALL] + (rooms[(int)MODE.MODE_FREEFALLALL].Count + 1).ToString();
            case MODE.MODE_TEAMDEATHMATCH:
                return modeRoomNames[(int)MODE.MODE_TEAMDEATHMATCH] + (rooms[(int)MODE.MODE_TEAMDEATHMATCH].Count + 1).ToString();
            case MODE.MODE_SUPERSTAR:
                return modeRoomNames[(int)MODE.MODE_SUPERSTAR] + (rooms[(int)MODE.MODE_SUPERSTAR].Count + 1).ToString();
            default:
                return "error";
        }
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
        playMode = gameMode;
        string roomName = CheckingRoom(gameMode);
        if (roomName == "empty")
        {
            // 임시로 방 생성은 일정한 이름으로 만들어 놨음
            bool roomMake = PhotonNetwork.CreateRoom(GetNewRoomName(), new RoomOptions { MaxPlayers = 8 }, null);
            if(!roomMake)
            {
                // 방 생성 실패

            }
        }
        else
            PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("접속실패");
        PhotonNetwork.JoinRoom("room");
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
            photonView.RPC("PlayStart", RpcTarget.All);
            // 게임이 시작했으면 방을 닫는다.
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
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

}
