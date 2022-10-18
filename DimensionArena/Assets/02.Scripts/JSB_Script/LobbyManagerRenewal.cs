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

    [SerializeField] public TextMeshProUGUI loadText;

    private string playerName = "Guest";
    public string PlayerName { get { return playerName; } }

    
    // Survival , FreeforAll , TeamDeathMatch , SuperStar
    private string[] modeRoomNames = { "SV", "FF", "TD", "SS" };

    private List<List<RoomInfo>> rooms = new List<List<RoomInfo>>();
    
    public MODE playMode;

    [SerializeField] private int leastStartPlayer = 4;
    public int LeastStartPlayer { get { return leastStartPlayer; } }
    [SerializeField] private int maxStartPlayer = 8;
    public int MaxStartPlayer { get { return maxStartPlayer; } }

    [SerializeField] private List<string> playersName;


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

        loadText.text = "���� ���� �õ���...";
        // ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        loadText.text = "���� �κ� ������...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        loadText.text = "���� ���� ����";
        
        playerName = MakeRandNickname();

        PhotonNetwork.NickName = playerName;
        LoadingSceneController.Instance.LoadScene("Lobby_Main");

    }

    private string MakeRandNickname()
    {
        loadText.text = "���� �̸� ������...";
        return FirebaseDB_Manager.Instance.RegisterDataBase(loadText);
    } 

    [PunRPC]
    public void PlayerNameAdd(string name)
    {
        playersName.Add(name);
    }

    // �ش� �Լ��� �κ� ���ư��� �� �ڵ������� ȣ��Ǵ� �Լ��̸� , �κ� ���ο����� ������ �� �� ����.
    // -> �׷� �̰� �κ� �� �� ���� �� ���� ����� ȣ���� �Ǵ°ǰ�..;;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        SetUpRoomList(roomList);
        //rooms[(int)playMode] = roomList;
    }

    // ���� 2���ڷ� �ۿ� ������ ���Ѵ� �� �� ���� �̸��� ���� ���ļ��� �ȵȴ�
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

    // ���� ���� �ִ��� Ȯ���ϴ� �Լ��̴�.
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


        // �ּ����� ���� ���۵Ǵ� ������ 4�� ������
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
            // �ӽ÷� �� ������ ������ �̸����� ����� ����
            bool roomMake = PhotonNetwork.CreateRoom(GetNewRoomName(), new RoomOptions { MaxPlayers = 8 }, null);
            if(!roomMake)
            {
                // �� ���� ����

            }
        }
        else
            PhotonNetwork.JoinRoom(roomName);
    }
    public void ChangeNickNmae(string name)
    {
        
        playerName = name;
        PhotonNetwork.NickName = playerName;

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("���ӽ���");
        PhotonNetwork.JoinRoom("room");
    }
    public override void OnJoinedRoom()
    {
       
        //  �� ���� ��������µ� �� ���� �����ΰǰ�...
        //  JoinRandomRoom failed. Client is on GameServer (must be Master Server for matchmaking) and ready
        
        
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject obj = GameObject.Find("MatchState");

        obj.GetComponent<MatchStateText>().RefreshTextAllClient(playerCount, maxStartPlayer);

        if (leastStartPlayer <= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC("PlayStart", RpcTarget.All);
            // ������ ���������� ���� �ݴ´�.
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
