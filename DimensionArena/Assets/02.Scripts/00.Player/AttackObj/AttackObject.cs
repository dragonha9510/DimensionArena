using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{

    //AttackObject�� ����
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
                //��� Player���� �������� �� ���, 
            case "Player":
                {
                    photonView.RPC("OnCollisionToPlayer", 
                        RpcTarget.All,
                        collision.gameObject.name);
                }
                break;
                //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
