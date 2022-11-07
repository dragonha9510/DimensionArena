using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Aura_Projectile : AttackObject
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Vector3 originPos;

    float range = 10.0f;


    private void LateUpdate()
    {
        if (!PhotonNetwork.InRoom)
        {
            if (range < (this.transform.position - originPos).magnitude)
            {
                Destroy(this.gameObject);
            }
        }
        else if (!PhotonNetwork.IsMasterClient)
            return;
        if (range < (this.transform.position - originPos).magnitude)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void AttackToDirection(Vector3 pos, Vector3 dir, float range, float speed)
    {
        originPos = transform.position;
        this.range = range;
        rigid.velocity = dir * speed;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if(PhotonNetwork.InRoom)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            switch (other.tag)
            {
                //상대 Player에게 데미지를 준 경우, 
                case "Player":
                    {
                        if (ownerID != other.gameObject.name)
                        {
                            photonView.RPC("OnCollisionToPlayer",
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (other.tag)
            {
                //상대 Player에게 데미지를 준 경우, 
                case "Player":
                    OnCollisionToPlayer(ownerID, other.gameObject.name, other.gameObject.transform.position);
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

}
