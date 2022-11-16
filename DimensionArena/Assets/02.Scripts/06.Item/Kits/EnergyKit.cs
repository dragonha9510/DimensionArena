using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;
public class EnergyKit : Item
{
    protected override void InteractItem(string targetID)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.AllViaServer, targetID);
    }

    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.CurSkillPtIncrease(targetID, info.skillRecovery);

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject); 
    }
}
