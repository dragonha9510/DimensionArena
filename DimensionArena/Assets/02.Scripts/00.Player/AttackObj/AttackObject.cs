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

  
    private void OnTriggerEnter(Collider collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        switch (collision.gameObject.tag)
        {
                //��� Player���� �������� �� ���, 
            case "Player":
                {
                    PlayerInfoManager.Instance.
                        CurHpDecrease(collision.gameObject, damage);
                    //Point Get
                    PlayerInfoManager.Instance.
                        CurSkillPtIncrease(owner, ultimatePoint);

                    PhotonNetwork.Destroy(this.gameObject);
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
