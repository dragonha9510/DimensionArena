using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using ExitGames.Client.Photon;
using ManagerSpace;
using PlayerSpace;
using UnityEngine.UI;
using TMPro;
public enum GAMEMODE
{
    Survival,
    FreeForAll
}


public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private GameObject WatingCanvas;

    [SerializeField] private SpawnPoint[] spawnPoint;

    [SerializeField]
    public GameObject playerPrefab;
    public GAMEMODE GameMode { get; private set; }

    int startLeastNum = 1;
    public int StartLeastNum { get { return startLeastNum; } }

    private bool isGameEnd = false;
    public bool IsGameEnd { get { return isGameEnd; } set { isGameEnd = value; } }

    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
   

    private void Awake()
    {

        if (instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(this.gameObject);

        //Test Code
        GameMode = GAMEMODE.Survival;

        Vector3 spawnPoint = new Vector3(0, 0, 0);

        //   �� �� �̰� ���������� �ٲ����. ���ӸŴ��� ������ ����ϱ�
        
        //GameObject testObject = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
        //    + playerPrefab.name, spawnPoint, Quaternion.identity);


        //�ٷ�, �����ϸ� �ȵ�
        PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
            + playerPrefab.name, spawnPoint, Quaternion.identity);


    }


    private void Start()
    {
        //SoundManager.Instance.PlayBGM("BattleMusic");
        //SoundManager.Instance.AddPhotonView();
        PlayerInfoManager.Instance.RegisterPlayer();
        SoundManager.Instance.PlayRandomInGameSound();
        LobbyManagerRenewal.Instance.ReadyToPlay();

        spawnPoint = FindObjectsOfType<SpawnPoint>();     
        // �÷��̾� ��� ����
        WatingCanvas.SetActive(true);
        StartCoroutine(nameof(WaitAllPlayers));
    }

    public void OnEvent(EventData photonEvent)
    {
        if (0 == photonEvent.Code)
        {
            object[] data = (object[])photonEvent.CustomData;
            for (int i = 0; i < data.Length; ++i)
            {
                print(data[i]);
            }
        }
    }

    IEnumerator WaitAllPlayers()
    {
        while(true)
        {
            if (LobbyManagerRenewal.Instance.InGameReadyPlayer == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                WatingCanvas.SetActive(false);
                break;
            }
            yield return null;
        }

        if (!PhotonNetwork.IsMasterClient)
            yield break;

        ManagerMediator mediator = GetComponent<ManagerMediator>();
        mediator.enabled = true;

        yield return new WaitUntil(() => mediator.IsAllManagerActive);
        //��� �÷��̾���� ��ϵ� ��Ȳ�̶��, 
        StartCoroutine(SpawnPointRegisterPlayer());



    }

    IEnumerator SpawnPointRegisterPlayer()
    {
        GameObject[] players = PlayerInfoManager.Instance.PlayerObjectArr;

        int idx;

        for(int i = 0; i < players.Length; ++i)
        {
            while(true)
            {
                yield return null;
                idx = Random.Range(0, spawnPoint.Length);

                if (spawnPoint[idx].GetRegisterState())
                    continue;

                spawnPoint[idx].SetRegisterOn();
                players[i].transform.position = spawnPoint[idx].transform.position;
                photonView.RPC(nameof(SetPlayerPositionForAllClient), RpcTarget.All, players[i], spawnPoint[idx].transform.position);
                break;
            }
        }
    }

    [PunRPC]
    public void SetPlayerPositionForAllClient(GameObject player, Vector3 position)
    {
        player.transform.position = position;
    }




   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
