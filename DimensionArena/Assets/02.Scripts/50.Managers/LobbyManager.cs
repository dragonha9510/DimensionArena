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

    // 시작 플레이어 제한 수 입니다
    [SerializeField]
    int startPlayerCount;
    // 현 접속한 플레이어의 카운트 입니다
    public int connectCount = 0;
    // 이름 InputField Text 입니다
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
        // 플레이어 이름 목록들을 받아온다.
        players = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in players)
        {
            if (p.NickName == name)
            {
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
        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = (byte)startPlayerCount }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ++roomCount;
        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = (byte)startPlayerCount }, null);

    }
    public override void OnJoinedRoom()
    {

        if (NameOverLapCheck(nameText.text))
            return;

        LoadMatchMakingScene();

        if (startPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            photonView.RPC("LoadingInGame", RpcTarget.All);
        

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
        
        //로비 매니저에서 고쳐야됨.
        PhotonNetwork.LoadLevel("Map1");
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }

}
