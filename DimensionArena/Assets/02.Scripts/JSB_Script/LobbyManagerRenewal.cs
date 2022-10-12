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

        MakeRandNickname();
    }

    private void MakeRandNickname()
    {

        loadText.text = "���� �̸� ������...";
        do
        {
            randomName += Random.Range(1, 100).ToString();
        } while (NameOverLapCheck(randomName));
    }


    private bool NameOverLapCheck(string name)
    {
        loadText.text = "�̸� �ߺ� Ȯ����...";


        // �÷��̾� �̸� ��ϵ��� �޾ƿ´�.
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in players)
        {
            if (p.NickName == name)
            {
                loadText.text = "�̸� �ߺ�";
                return true;
            }
        }
        loadText.text = "�ߺ� üũ �Ϸ�";
        PhotonNetwork.NickName = name;
        isConnect = true;
        LoadingSceneController.Instance.LoadScene("LoadingTest1");
        return false;
    }

    // ���� ���� �ִ��� Ȯ���ϴ� �Լ��̴�.
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
            // �ӽ÷� �� ������ ������ �̸����� ����� ����
            bool roomMake = PhotonNetwork.CreateRoom("����", new RoomOptions { MaxPlayers = 8 }, null);
            if (roomMake)
            {
                roomName = modeRoomNames[(int)gameMode];
            }
        }
        if (PhotonNetwork.JoinRoom("����"))
        {
            Debug.Log("���� ����");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("���ӽ���");
        PhotonNetwork.JoinRoom("����");
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
