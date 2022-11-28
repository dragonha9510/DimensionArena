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
        photonView.RPC(nameof(InteractItemForAllcient), RpcTarget.All, targetID);
        photonView.RPC(nameof(CreateDropEffect), RpcTarget.All, eatTrans,eatRot);

    }
    [PunRPC]
    private void CreateDropEffect(Vector3 trans, Quaternion quaternion)
    {
        EffectManager.Instance.CreateParticleEffectOnGameobject(trans, quaternion, "ItemDrop");
    }
    [PunRPC]
    public void InteractItemForAllcient(string targetID)
    {
        PlayerInfoManager.Instance.CurSkillPtIncrease(targetID, info.skillRecovery);
        
    }
}
