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
        if(!PhotonNetwork.InRoom)
        {
            if (this.gameObject.name.Equals(other.gameObject.name))
                return;

            if (other.CompareTag("Item_Box"))
            {
                if (info.ownerID == "RedZone")
                {
                    Destroy(this.gameObject);
                    return;
                }
                ItemBox itembox = other.GetComponent<ItemBox>();

                itembox.HpDecrease_KnockBack(info.damage, info.ownerID);

                Destroy(this.gameObject);
                return;
            }

            isKnockBack temp = other.GetComponent<isKnockBack>();

            if (temp == null)
            {
                Destroy(this.gameObject);
                return;
            }

            if (temp.info.isOn && temp.enabled)
            {
                Destroy(this.gameObject);
                return;
            }

            temp.enabled = true;
            temp.info.isOn = true;

            temp.info.direction = (other.transform.position - transform.position);
            temp.info.direction.y = 0;
            temp.info.direction.Normalize();

            temp.info.speed = info.speed;
            temp.info.distance = info.distance;

            if (info.isDamage || info.isPercentDamage)
            {
                if (!info.isEnvironment)
                    PlayerInfoManager.Instance.
                                    CurSkillPtIncrease(info.ownerID, info.ultimatePoint);

                //damage 贸府
                int damage;
                if (info.isPercentDamage)
                    damage = PlayerInfoManager.Instance.CurHPDecreaseRatio(info.ownerID, other.name, info.damage);
                else
                    damage = PlayerInfoManager.Instance.CurHpDecrease(info.ownerID, other.name, info.damage);

                FloatingText.Instance.CreateFloatingTextForDamage(other.transform.position, damage);
            }

            temp.SetValue();

            Destroy(this.gameObject);
        }
        else if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            if (this.gameObject.name.Equals(other.gameObject.name))
                return;
            Debug.Log("面倒眉农沁澜");
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

            if (info.isDamage || info.isPercentDamage)
            {
                if (!info.isEnvironment)
                {
                    photonView.RPC(nameof(CurSkillPtIncreaseAllClient), RpcTarget.AllViaServer, info.ownerID, info.ultimatePoint);
                }

                //damage 贸府
                int damage;
                if (info.isPercentDamage)
                {
                    damage = PlayerInfoManager.Instance.CurHPDecreaseRatio(info.ownerID, other.name, info.damage);
                    photonView.RPC(nameof(CurHPDecreaseRatioAllClient), RpcTarget.Others, info.ownerID, other.name, info.damage);
                }
                else
                {
                    damage = PlayerInfoManager.Instance.CurHpDecrease(info.ownerID, other.name, info.damage);
                    photonView.RPC(nameof(CurHpDecreaseAllClient), RpcTarget.Others, info.ownerID, other.name, info.damage);
                }

                FloatingText.Instance.CreateFloatingTextForDamage(other.transform.position, damage);
            }

            temp.SetValue();

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    [PunRPC]
    private void CurHpDecreaseAllClient(string ownerID, string name,float damage)
    {
        PlayerInfoManager.Instance.CurHpDecrease(ownerID, name, damage);
    }

    [PunRPC]
    private void CurSkillPtIncreaseAllClient(string ownerID,float point)
    {
         PlayerInfoManager.Instance.
                                    CurSkillPtIncrease(info.ownerID, info.ultimatePoint);
    }
    [PunRPC]
    private void CurHPDecreaseRatioAllClient(string ownerID,string name,float damage)
    {
        PlayerInfoManager.Instance.CurHPDecreaseRatio(ownerID, name, damage);
    }

}
