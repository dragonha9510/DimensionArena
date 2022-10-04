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
                //상대 Player에게 데미지를 준 경우, 
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
                //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
