using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;
public class ShieldKit : Item
{
    protected override void InteractItem(string targetID)
    {
        photonView.RPC(nameof(InteractItemForAllcient),Photon.Pun.RpcTarget.All, targetID);
    }

    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.GetShield(targetID, info.shieldAmount);
    }

}
