using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonOfflineMode : MonoBehaviour
{
    [SerializeField] private bool offlineMode;

    private void Awake()
    {
        PhotonNetwork.OfflineMode = offlineMode;
    }
}
