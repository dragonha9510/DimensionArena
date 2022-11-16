using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

public class DemensionKit : Item
{
    protected override void InteractItem(string targetID)
    {
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.AllViaServer, targetID);
    }

    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.DmgUp(targetID, info.attackIncrement);
        PlayerInfoManager.Instance.SpeedIncrease(targetID, info.speedAmount,info.statusDuration);
        PlayerInfoManager.Instance.CurHpIncrease(targetID, info.healthAmount);
    }

}
