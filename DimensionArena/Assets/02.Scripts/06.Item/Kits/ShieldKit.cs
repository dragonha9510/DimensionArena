using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;
public class ShieldKit : Item
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
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.GetShield(targetID, info.shieldAmount,info.statusDuration);

    }
    [PunRPC]
    private void CreateDropEffect(Vector3 trans, Quaternion quaternion)
    {
        EffectManager.Instance.CreateParticleEffectOnGameobject(trans, quaternion, "ItemDrop");
    }
}
