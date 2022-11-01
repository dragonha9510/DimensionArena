using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

public class PowerKit : Item
{
    protected override void InteractItem(string targetID)
    {
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.All, targetID);
    }

    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.DmgUp(targetID, info.attackIncrement);
    }
}