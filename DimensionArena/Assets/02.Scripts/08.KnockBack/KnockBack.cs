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
        if (!PhotonNetwork.InRoom)
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
        else if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
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
            if (other.gameObject.tag == "Player")
            {
                temp.enabled = true;
                temp.info.isOn = true;

                temp.info.direction = (other.transform.position - transform.position);
                temp.info.direction.y = 0;
                temp.info.direction.Normalize();

                temp.info.speed = info.speed;
                temp.info.distance = info.distance;

                PlayerInfoManager.Instance.DicPlayer[other.gameObject.name].GetComponent<isKnockBack>().AllClientKncokBackCall(other.gameObject.name
                                                                                                                                , this.transform.position
                                                                                                                                , temp.info.direction
                                                                                                                                , temp.info.speed
                                                                                                                                , temp.info.distance);
                if (info.isDamage || info.isPercentDamage)
                {
                    if (!info.isEnvironment)
                        PlayerInfoManager.Instance.
                                        CurSkillPtInCreaseAllClient(info.ownerID, info.ultimatePoint);

                    //damage 贸府
                    int damage;
                    if (info.isPercentDamage)
                        damage = PlayerInfoManager.Instance.CurHPDecreaseRatioAllClient(info.ownerID, other.name, info.damage);
                    else
                        damage = PlayerInfoManager.Instance.CurHpDecreaseAllClient(info.ownerID, other.name, info.damage);

                    FloatingText.Instance.CreateFloatingTextForDamage(other.transform.position, damage);
                }

                temp.SetValue();

                Destroy(this.gameObject);
            }
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
            
            
        }
    }
}
