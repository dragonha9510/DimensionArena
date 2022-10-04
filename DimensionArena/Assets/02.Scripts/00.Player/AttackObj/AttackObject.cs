using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{
    public GameObject owner;
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

    private void OnDamaged(GameObject obj)
    {
        Destroy(this.gameObject);

        //Target damage
        PlayerInfoManager.Instance.
            CurHpDecrease(obj, damage);
        //Point Get
        PlayerInfoManager.Instance.
            CurSkillPtIncrease(owner, ultimatePoint);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
                //��� Player���� �������� �� ���, 
            case "Player":
                {
                    Destroy(this.gameObject);

                    //Target damage
                    PlayerInfoManager.Instance.
                        CurHpDecrease(collision.gameObject, damage);
                    //Point Get
                    PlayerInfoManager.Instance.
                        CurSkillPtIncrease(owner, ultimatePoint);
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
