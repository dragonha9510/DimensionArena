using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using UnityEngine;

using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks,IPunObservable
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if(null == m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    [SerializeField]
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
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
        Vector3 spawnPoint = new Vector3(0, 1, 0);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
