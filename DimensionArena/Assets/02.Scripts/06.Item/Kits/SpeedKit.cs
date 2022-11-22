using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

public class SpeedKit : Item
{
    protected override void InteractItem(string targetID)
    {
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.AllViaServer, targetID);
    }

    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.SpeedIncrease(targetID, info.speedAmount,info.statusDuration);

        if (PhotonNetwork.IsMasterClient)
        {
            CreateDropEffect();
            PhotonNetwork.Destroy(this.gameObject);
        }

    }
}
