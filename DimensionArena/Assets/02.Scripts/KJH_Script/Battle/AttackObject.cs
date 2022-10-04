using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviour
{
    public GameObject owner;
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
                //상대 Player에게 데미지를 준 경우, 
            case "Player":
                {
                    Destroy(this.gameObject);

                    if(collision.GetComponentInParent<Player>().gameObject != owner)
                    {
                        collision.GetComponentInParent<Player>().Damaged(damage);

                        if (!owner.GetComponentInParent<PhotonView>().IsMine)
                            return;

                        owner.GetComponent<Player>().GetSkillPoint(ultimatePoint);
                    }
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
