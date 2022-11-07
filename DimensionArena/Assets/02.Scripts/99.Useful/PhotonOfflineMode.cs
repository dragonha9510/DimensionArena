using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

public class PhotonOfflineMode : MonoBehaviour
{
    [SerializeField] private bool offlineMode;
    private bool once;

    private void Awake()
    {
        PhotonNetwork.OfflineMode = offlineMode;
    }
    private void Start()
    {
    }

    private void Update()
    {
        if(!once)
        {
            PlayerInfoManager.Instance.RegisterPlayer();

            once = true;
        }
    }
}
