using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public enum MODE
{
    MODE_SURVIVAL
}

public class LobbyManagerRenewal : MonoBehaviourPunCallbacks
{
    public static LobbyManagerRenewal Instance;

    [SerializeField] TextMeshProUGUI loadText;

    private string randomName = "Guest";

    bool isConnect = false;
    public bool IsConnect { get { return isConnect; } }

    // Survival , FreeforAll , TeamDeathMatch , SuperStar
    private string[] modeRoomNames = { "SV", "FFA", "TDM", "SS" };

    List<List<RoomInfo>> rooms = new List<List<RoomInfo>>();
    private int roomCount = 0;


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
        rooms.Add(new List<RoomInfo>());

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
        LoadingSceneController.Instance.LoadScene("LoadingTest1");
        return false;
    }

    // 남은 룸이 있는지 확인하는 함수이다.
    private string CheckingRoom(MODE gameMode)
    {
        if (0 == roomCount)
            return "empty";
        foreach (Room room in rooms[(int)gameMode])
        {
            if (room.IsOpen)
                return room.Name;
        } 
        return "empty";
    }


    public void JoinOrCreateRoom(MODE gameMode)
    {

        string roomName = CheckingRoom(gameMode);
        if (roomName == "empty")
        {
            // 임시로 방 생성은 일정한 이름으로 만들어 놨음
            bool roomMake = PhotonNetwork.CreateRoom("씨발", new RoomOptions { MaxPlayers = 8 }, null);
            if (roomMake)
            {
                roomName = modeRoomNames[(int)gameMode];
            }
        }
        if (PhotonNetwork.JoinRoom("씨발"))
        {
            Debug.Log("접속 성공");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("접속실패");
        PhotonNetwork.JoinRoom("씨발");
    }
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("MatchMaking");
        if (2 <= PhotonNetwork.CountOfPlayersInRooms)
        {
          PhotonNetwork.LoadLevel("Prototype");
        }
    }
    private void Update()
    {
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
    }
}
