using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using UnityEngine;

using ExitGames.Client.Photon;

public enum GAMEMODE
{
    Survival,
    FreeForAll
}


public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    PlayerData data;
    [SerializeField]
    public GameObject playerPrefab;
    public GAMEMODE GameMode { get; private set; }

    int startLeastNum = 1;
    public int StartLeastNum { get { return startLeastNum; } }

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

        //   추 후 이거 유동적으로 바꿔야함. 게임매니저 문제점 기록하기
        
        //GameObject testObject = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
        //    + playerPrefab.name, spawnPoint, Quaternion.identity);

        InGameServerManager.GetInstance().PhotonNetworkInstantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER
            + playerPrefab.name, spawnPoint, Quaternion.identity);

        //PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER 
        //    + playerPrefab.name, spawnPoint, Quaternion.identity);

        //photonView.RPC("CreatePlayer", RpcTarget.All, PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + playerPrefab.name, spawnPoint, Quaternion.identity);

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

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BattleMusic");
        SoundManager.Instance.AddPhotonView();
        PlayerInfoManager.Instance.RegisterPlayer();
    }

   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
