using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


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

    private List<Dictionary<string,RoomInfo>> rooms = new List<Dictionary<string, RoomInfo>>();
    
    public MODE playMode;

    [SerializeField] private int leastStartPlayer = 4;
    public int LeastStartPlayer { get { return leastStartPlayer; } }
    [SerializeField] private int maxStartPlayer = 8;
    public int MaxStartPlayer { get { return maxStartPlayer; } }

    [SerializeField] private List<string> playersName;

    private bool isWillStartGame = false;
    public bool IsWillStartGame { get { return isWillStartGame; } }


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
            rooms.Add(new Dictionary<string, RoomInfo>());
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
        Debug.Log("�κ� ���� ����");

        loadText.text = "���� ���� ����";
        
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
        loadText.text = "���� �̸� ������...";
        return FirebaseDB_Manager.Instance.RegisterDataBase(loadText);
    } */

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
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC("SetUpRoomList",RpcTarget.All, roomList);
        SetUpRoomList(roomList);
        //rooms[(int)playMode] = roomList;
    }

    // ���� 2���ڷ� �ۿ� ������ ���Ѵ� �� �� ���� �̸��� ���� ���ļ��� �ȵȴ�
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
                    rooms[(int)MODE.MODE_SURVIVAL].Add(info.Name,info);
                    break;
                case "FF":
                    rooms[(int)MODE.MODE_FREEFALLALL].Add(info.Name, info);
                    break;
                case "TD":
                    rooms[(int)MODE.MODE_TEAMDEATHMATCH].Add(info.Name, info);
                    break;
                case "SS":
                    rooms[(int)MODE.MODE_SUPERSTAR].Add(info.Name, info);
                    break;
            }
        }
    }

    // ���� ���� �ִ��� Ȯ���ϴ� �Լ��̴�.
    private string CheckingRoom(MODE gameMode)
    {
        if (0 == rooms.Count)
            return "empty";
        foreach (Dictionary<string, RoomInfo> room in rooms)
        {
            foreach(string key in room.Keys)
            {
                if (room[key].IsOpen)
                {
                    return room[key].Name;
                }
            }
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
              foreach(string key in rooms[i].Keys)
            {
                playerCount += rooms[i][key].PlayerCount;
            }
            //for(int j = 0; j < rooms[i].Count;++j)
            //    playerCount += rooms[i][j].PlayerCount;
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
        FirebaseDB_Manager.Instance.IsInGame = true;    

        playMode = gameMode;
        string roomName = CheckingRoom(gameMode);
        if (roomName == "empty")
        {
            // �ӽ÷� �� ������ ������ �̸����� ����� ����
            string newRoomName = GetNewRoomName();
            bool roomMake = PhotonNetwork.CreateRoom(newRoomName, new RoomOptions { MaxPlayers = 8 }, null);
            if(!roomMake)
            {
                // �� ���� ����

            }
        }
        else
            PhotonNetwork.JoinRoom(roomName);
    }
    /*public void ChangeNickNmae(string name)
    {
        
        playerName = name;
        PhotonNetwork.NickName = playerName;

    }*/

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("���ӽ���");
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
            isWillStartGame = true;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            photonView.RPC("PlayStart", RpcTarget.All);
            // ������ ���������� ���� �ݴ´�. -> ���� �ݰ� ���� �ε��ؾ��� ��û��
            //PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public bool TryGetOutRoom()
    {
        if(false == PhotonNetwork.InRoom || true == PhotonNetwork.LeaveRoom())
            return true;
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

}
