using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // ���� �÷��̾� ���� �� �Դϴ�
    [SerializeField]
    int startPlayerCount = 4;
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
        //Screen.SetResolution(1920, 1080, false);
        
    }
    private void Start()
    {
        players = new Photon.Realtime.Player[startPlayerCount];

        SoundManager.Instance.LoadMusics();
    }


    private bool NameOverLapCheck(string name)
    {
        // �÷��̾� �̸� ��ϵ��� �޾ƿ´�.
        players = PhotonNetwork.PlayerList;
        foreach(Photon.Realtime.Player p in players)
        {
            if (p.NickName == name)
            {
                Debug.Log("NickName OverLap!!");
                return true;
            }
        }
        return false;
    }

    public void Connect()
    {
        if(!NameOverLapCheck(nameText.text))
        {
            PhotonNetwork.LocalPlayer.NickName = nameText.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        
        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = 2 },null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("���� ����");
        ++roomCount;
        PhotonNetwork.JoinOrCreateRoom(defaultRoomName + roomCount.ToString(), new RoomOptions { MaxPlayers = 2 }, null);
      
    }
    public override void OnJoinedRoom()
    {
        
        Debug.Log("OnJoinedRoom");

        LoadMatchMakingScene();
        if (startPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            GetComponent<PhotonView>().RPC("LoadingInGame", RpcTarget.All);
        //if (PhotonNetwork.IsConnected)
        //{
        //    connectCount = PhotonNetwork.CurrentRoom.PlayerCount;
        //    connectText.GetComponent<ConnectCountText>().RefreshServerText(connectCount);
        //}
    }


    private void LoadMatchMakingScene()
    {
        PhotonNetwork.LoadLevel("MathMaking");
    }

    public void LoadSingleTestMode()
    {
        SceneManager.LoadScene("SingleTestScene");
    }

    [PunRPC]
    private void LoadingInGame()
    {
        PhotonNetwork.LoadLevel("ProtoType");
    }
}
