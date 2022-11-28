using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

public class DemensionKit : Item
{
    protected override void InteractItem(string targetID)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.All, targetID);
        photonView.RPC(nameof(CreateDropEffect), RpcTarget.All, eatTrans, eatRot);
        PhotonNetwork.Destroy(this.gameObject);

    }
    [PunRPC]
    private void CreateDropEffect(Vector3 trans, Quaternion quaternion)
    {
        EffectManager.Instance.CreateParticleEffectOnGameobject(trans, quaternion, "ItemDrop");
    }
    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.DmgUp(targetID, info.attackIncrement);
        PlayerInfoManager.Instance.SpeedIncrease(targetID, info.speedAmount,info.statusDuration);
        PlayerInfoManager.Instance.CurHpIncrease(targetID, info.healthAmount);

    }

}
