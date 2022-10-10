using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private static LobbyManager Instance;

    // ���� �÷��̾� ���� �� �Դϴ�
    [SerializeField]
    int startPlayerCount;
    // �� ������ �÷��̾��� ī��Ʈ �Դϴ�
    public int connectCount = 0;
    // �̸� InputField Text �Դϴ�
    [SerializeField]
    TextMeshProUGUI nameText;

    [SerializeField]
    TextMeshProUGUI connectText;

    int roomCount = 0;
    string defaultRoomName = "Room";

    //
    Photon.Realtime.Player[] players;


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
    private void Start()
    {
        players = new Photon.Realtime.Player[startPlayerCount];

    }


    private bool NameOverLapCheck(string name)
    {
        // �÷��̾� �̸� ��ϵ��� �޾ƿ´�.
        players = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in players)
        {
            if (p.NickName == name)
            {
                Debug.Log("NickName OverLap!!");
                PhotonNetwork.Disconnect();
                return true;
            }
        }
        return false;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = (byte)startPlayerCount }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("���� ����");
        ++roomCount;
        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = (byte)startPlayerCount }, null);

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");

        if (NameOverLapCheck(nameText.text))
            return;


        if (startPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            photonView.RPC("LoadingInGame", RpcTarget.All);
        else
            LoadMatchMakingScene();

        //if (PhotonNetwork.IsConnected)
        //{
        //    connectCount = PhotonNetwork.CurrentRoom.PlayerCount;
        //    connectText.GetComponent<ConnectCountText>().RefreshServerText(connectCount);
        //}
    }


    private void LoadMatchMakingScene()
    {
        PhotonNetwork.NickName = nameText.text;
        PhotonNetwork.LoadLevel("MathMaking");
    }

    public void LoadSingleTestMode()
    {
        SceneManager.LoadScene("SingleTestScene");
    }

    [PunRPC]
    private void LoadingInGame()
    {
        PhotonNetwork.NickName = nameText.text;
        PhotonNetwork.LoadLevel("ProtoType");
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LoadOptionScene()
    {
        SceneManager.LoadScene("Option");
    }

}
