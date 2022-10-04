using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{

    //AttackObject의 주인
    public string ownerID;
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

  

    [PunRPC]
    protected void OnCollisionToPlayer(string id)
    {
        PlayerInfoManager.Instance.
                        CurSkillPtIncrease(ownerID,ultimatePoint);
        //Damaged
        PlayerInfoManager.Instance.
                       CurHpDecrease(id, damage);

        PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        switch (collision.gameObject.tag)
        {
                //상대 Player에게 데미지를 준 경우, 
            case "Player":
                {
                    photonView.RPC("OnCollisionToPlayer", 
                        RpcTarget.All,
                        collision.gameObject.name);
                }
                break;
                //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
