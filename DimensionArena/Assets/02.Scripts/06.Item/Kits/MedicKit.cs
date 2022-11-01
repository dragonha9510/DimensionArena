using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;
public class MedicKit : Item
{
    protected override void InteractItem(string targetID)
    {
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.All, targetID);
    }


    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.CurHpIncrease(targetID, info.healthAmount);
    }

}