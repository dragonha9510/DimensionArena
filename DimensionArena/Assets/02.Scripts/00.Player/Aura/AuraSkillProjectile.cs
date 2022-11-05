using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AuraSkillProjectile : AttackObject
{
    [SerializeField]
    private Rigidbody rigid;

    [SerializeField] 
    private Vector3 originPos;


    float range = 10.0f;


    private void LateUpdate()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log((this.transform.position - originPos).magnitude);
            if (range < (this.transform.position - originPos).magnitude)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (range < (this.transform.position - originPos).magnitude)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void StartAttack(float _speed , float _range)
    {
        originPos = this.transform.position;
        range = _range;
        rigid.velocity = this.transform.forward * _speed;
    }



}
