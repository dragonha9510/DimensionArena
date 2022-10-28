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

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject gameMgr = GameObject.Find("GameManager");//new GameObject("PlayerInfoManager");

                if (!gameMgr)
                {
                    gameMgr = new GameObject("GameManager");
                    gameMgr.AddComponent<GameManager>();
                }
                instance = gameMgr.GetComponent<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {

        //Test Code
        GameMode = GAMEMODE.Survival;

        Vector3 spawnPoint = new Vector3(0, 0, 0);

        //   추 후 이거 유동적으로 바꿔야함. 게임매니저 문제점 기록하기
        
        //GameObject testObject = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
        //    + playerPrefab.name, spawnPoint, Quaternion.identity
    }

    
    private void Start()
    {
        //SoundManager.Instance.PlayBGM("BattleMusic");
        //SoundManager.Instance.AddPhotonView();
        PlayerInfoManager.Instance.RegisterPlayer();
        SoundManager.Instance.PlayRandomInGameSound();
        LobbyManagerRenewal.Instance.ReadyToPlay();

        spawnPoint = FindObjectsOfType<SpawnPoint>();     
        // 플레이어 대기 상태
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
        PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
          + playerPrefab.name, Vector3.zero, Quaternion.identity);
        Debug.Log(LobbyManagerRenewal.Instance.InGameReadyPlayer);

        while (true)
        {
            if (LobbyManagerRenewal.Instance.InGameReadyPlayer == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("실행됨");
                WatingCanvas.SetActive(false);
                //바로, 생성하면 안됨    
                break;
            }
            yield return null;
        }

        if (!PhotonNetwork.IsMasterClient)
            yield break;

        
        ManagerMediator mediator = GetComponent<ManagerMediator>();
        mediator.enabled = true;

        yield return new WaitUntil(() => mediator.IsAllManagerActive);
        //모든 플레이어들이 등록된 상황이라면, 
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
                photonView.RPC(nameof(SetPlayerPositionForAllClient), RpcTarget.All, players[i].name, spawnPoint[idx].transform.position);
                break;
            }
        }
    }

    [PunRPC]
    public void SetPlayerPositionForAllClient(string playerName, Vector3 position)
    {
        GameObject obj;
        PlayerInfoManager.Instance.DicPlayer.TryGetValue(playerName, out obj);
        obj.transform.position = position;
    }


   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }


    public void GameEnd()
    {
        isGameEnd = true;
        ObjectPool.Instance.ResetPool();
    }
}
