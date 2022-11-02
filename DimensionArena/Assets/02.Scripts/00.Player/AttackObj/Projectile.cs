using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : AttackObject
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Vector3 originPos;

    //basic Range 10
    float range = 10.0f;

    private void Awake()
    {
        //JSB Sound
        SoundManager.Instance.PlaySFXOneShotInRange(2.0f, this.transform, "juhyeok_shot");

        originPos = transform.position;
        //SoundManager.Instance.PlaySFXOneShot("JiJooNormalEffect");
    }

    private void LateUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if(range < (this.transform.position - originPos).magnitude)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void AttackToDirection(Vector3 dir, float range, float speed)
    {
        this.range = range;
        rigid.velocity = dir * speed;
    }

    protected virtual void OnTriggerEnter(Collider other)
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
            //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "ParentObstacle":
                {
                    Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);/*Quaternion.FromToRotation(Vector3.up, contact.normal);*/
                    Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                    if (hitPrefab != null)
                    {
                        var hitVFX = PhotonNetwork.Instantiate(hitPrefab.name, pos, rot);
                        var psHit = hitVFX.GetComponentInChildren<ParticleSystem>();
                        if (psHit != null)
                            Destroy(hitVFX, psHit.main.duration);
                        else
                        {
                            var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                            Destroy(hitVFX, psChild.main.duration);
                        }
                    }

                    PhotonNetwork.Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }
}
