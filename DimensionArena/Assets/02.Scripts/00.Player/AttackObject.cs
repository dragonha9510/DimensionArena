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
                //��� Player���� �������� �� ���, 
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
                //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
