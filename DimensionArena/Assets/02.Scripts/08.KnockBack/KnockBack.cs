using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

[RequireComponent(typeof(SphereCollider))]
public class KnockBack : MonoBehaviourPun
{
    [HideInInspector] public KnockBackInfo info;

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item_Box"))
        {
            ItemBox itembox = collision.gameObject.GetComponent<ItemBox>();

            itembox.HpDecrease_KnockBack(info.damage, info.ownerID);

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (this.gameObject.name.Equals(other.gameObject.name))
            return;

        if (other.CompareTag("Item_Box"))
        {
            if (info.ownerID == "RedZone")
            {
                PhotonNetwork.Destroy(this.gameObject);
                return;
            }
            ItemBox itembox = other.GetComponent<ItemBox>();

            itembox.HpDecrease_KnockBack(info.damage, info.ownerID);

            PhotonNetwork.Destroy(this.gameObject);
            return;
        }

        isKnockBack temp = other.GetComponent<isKnockBack>();

        if (temp == null)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return; 
        }

        if (temp.info.isOn && temp.enabled)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return;
        }

        temp.enabled = true;
        temp.info.isOn = true;

        temp.info.direction = (other.transform.position - transform.position);
        temp.info.direction.y = 0;
        temp.info.direction.Normalize();

        temp.info.speed = info.speed;
        temp.info.distance = info.distance;

        if (!info.isEnvironment)
            PlayerInfoManager.Instance.
                            CurSkillPtIncrease(info.ownerID, info.ultimatePoint);

        if (info.isDamage || info.isPercentDamage)
            photonView.RPC(nameof(DamageForAllClient), RpcTarget.AllViaServer, info.ownerID,other.name, other.transform.position,info.damage);


        temp.SetValue();

    }


    [PunRPC]
    private void DamageForAllClient(string ownerID,string target, Vector3 position,float damage)
    {
        if (info.isPercentDamage)
            damage = PlayerInfoManager.Instance.CurHPDecreaseRatio(ownerID, target, damage);
        else
            damage = PlayerInfoManager.Instance.CurHpDecrease(ownerID, target, damage);

        FloatingText.Instance.CreateFloatingTextForDamage(position, damage);
        
        if(photonView.IsMine)
            PhotonNetwork.Destroy(this.gameObject);
    }


}
