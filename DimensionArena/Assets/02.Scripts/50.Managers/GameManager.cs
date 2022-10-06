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

    [SerializeField]
    public GameObject playerPrefab;
    public GAMEMODE GameMode { get; private set; }
    private float startTime = 1.0f;

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

        //Test Code
        GameMode = GAMEMODE.Survival;


        Vector3 spawnPoint = new Vector3(0, 1, 0);

        PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER 
            + playerPrefab.name, spawnPoint, Quaternion.identity);

    }



    private void InitPlayer()
    { 

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
        SoundManager.Instance.PlayMusic("BattleMusic");
    }

    private void FixedUpdate()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

            startTime -= Time.deltaTime;

            if (0 > startTime)
                PlayerInfoManager.Instance.AddPlayer();
        
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
