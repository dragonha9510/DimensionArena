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


    // ���� ������ �ʱ�ȭ ���������
    private int inGameReadyPlayer = 0;
    public int InGameReadyPlayer { get { return inGameReadyPlayer; } }
    private int nowGameStartCount = 0;
    public int NowGameStartCount { get { return nowGameStartCount; } }

    [SerializeField]
    private float waitOtherPlayerTime = 5.0f;

    private float waitTimeRemain = 0f;
    public float WaitTimeRemain { get { return waitTimeRemain; } }

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

        loadText.text = "서버 탐색충...";
        // ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        loadText.text = "서버 연결중...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� ����");

        loadText.text = "서버 연결 완료...";
        
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
        SetUpRoomList(roomList);
        //rooms[(int)playMode] = roomList;
    }

    // ���� 2���ڷ� �ۿ� ������ ���Ѵ� �� �� ���� �̸��� ���� ���ļ��� �ȵȴ�
    // ���⼭�� Ȯ�Ǽ��� ���� OnRoomListUpdate �� ��Ͽ��� �ݵ�� �߰��� �̸��� �߰��� ����.
    // �ƴϴ� , �÷��̾� ī��Ʈ�� Ȯ���ϰ� ����.
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

    // ���� ���� �ִ��� Ȯ���ϴ� �Լ��̴�.
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


        // �ּ����� ���� ���۵Ǵ� ������ 4�� ������
        if(15 < playerCount && 4 < roomCount)
            return null;

        // ���ο� ���� �ε����� ���ϱ� ���� �� �� �̸��� �޴´�.
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

    public void JoinOrCreateRoom()
    {
        inGameReadyPlayer = 0;
        FirebaseDB_Manager.Instance.IsInGame = true;    

        string roomName = CheckingRoom(playMode);
        nowInRoomName = roomName;

        if (roomName == "empty")
        {
            // �ӽ÷� �� ������ ������ �̸����� ����� ����
            CustomRoomInfo newRoomInfo = GetNewRoomName();
            Debug.Log(newRoomInfo.RoomName);
            rooms[(int)playMode].Add(newRoomInfo.RoomName,newRoomInfo);
            string newRoomName = newRoomInfo.RoomName;
            bool roomMake = PhotonNetwork.CreateRoom(newRoomName, new RoomOptions { MaxPlayers = 8 }, null);
            if(!roomMake)
            {
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
        Debug.Log("���ӽ���");
    }
    public override void OnJoinedRoom()
    {

        //  JoinRandomRoom failed. Client is on GameServer (must be Master Server for matchmaking) and ready


        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject obj = GameObject.Find("MatchState");

        obj.GetComponent<MatchStateText>().RefreshTextAllClient(playerCount, maxStartPlayer);

        if (leastStartPlayer <= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC(nameof(SettingWaitStateUI), RpcTarget.AllViaServer);

            photonView.RPC(nameof(StartWaitForPlayerMaster),RpcTarget.MasterClient);
            //PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }


    Coroutine temp = null;
    [PunRPC]
    private void StartWaitForPlayerMaster()
    {
        if(temp == null)
            temp = StartCoroutine(WaitOtherPlayer());

    }
    [PunRPC]
    private void GetTimeToMaster_PlayerWaitTime(float time)
    {
        waitTimeRemain = time;
    }
    IEnumerator WaitOtherPlayer()
    {
        waitTimeRemain = waitOtherPlayerTime;
        isWillStartGame = true;
        while (0 < waitTimeRemain)
        {
            waitTimeRemain -= Time.deltaTime;
            if (waitTimeRemain <= 0)
                waitTimeRemain = 0f;
            photonView.RPC(nameof(GetTimeToMaster_PlayerWaitTime), RpcTarget.Others, waitTimeRemain);
            yield return null;
        }
        nowGameStartCount = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        photonView.RPC("PlayStart", RpcTarget.All);
        //PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    [PunRPC]
    private void SettingWaitStateUI()
    {
        GameObject ui = GameObject.Find("UI");
        ui.GetComponent<MatchMaking>().MatchBtnSetFalse();
        ui.GetComponent<MatchMaking>().SetWaitingState();
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
                PhotonNetwork.LoadLevel("Map1");
                break;
            case MODE.MODE_FREEFALLALL:
                PhotonNetwork.LoadLevel("Map2");
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
