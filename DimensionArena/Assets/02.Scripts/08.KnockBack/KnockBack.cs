using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

[RequireComponent(typeof(SphereCollider))]
public class KnockBack : MonoBehaviour
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
        if(other.CompareTag("Item_Box"))
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

        if(info.isDamage || info.isPercentDamage)
        {
            FloatingText.Instance.CreateFloatingTextForDamage(other.transform.position, info.damage);

            if(!info.isEnvironment)
                PlayerInfoManager.Instance.
                                CurSkillPtIncrease(info.ownerID, info.ultimatePoint);

            if (info.isPercentDamage)
                PlayerInfoManager.Instance.CurHPDecreaseRatio(info.ownerID, other.name, info.damage);
            else
                PlayerInfoManager.Instance.CurHpDecrease(info.ownerID, other.name, info.damage);
        }

        temp.SetValue();

        Destroy(this.gameObject);
    }
}
