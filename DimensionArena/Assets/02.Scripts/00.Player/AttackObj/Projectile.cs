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
        //SoundManager.Instance.PlaySFXOneShotInRange(2.0f, this.transform, "juhyeok_shot");

        originPos = transform.position;
        //SoundManager.Instance.PlaySFXOneShot("JiJooNormalEffect");
    }

    private void LateUpdate()
    {
        if(PhotonNetwork.OfflineMode)
        {
            if (range < (this.transform.position - originPos).magnitude)
            {
                Destroy(this.gameObject);
            }
        }
        else if (!PhotonNetwork.IsMasterClient)
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
    public void AttackToDirection(float range, float speed)
    {
        this.range = range;
        rigid.velocity = this.transform.forward * speed;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        bool onEffect = false;

        switch (other.tag)
        {
            //상대 Player에게 데미지를 준 경우, 
            case "Player":
                {
                    if (ownerID != other.gameObject.name)
                    {

                        if (!PhotonNetwork.OfflineMode)
                        {
                            photonView.RPC("OnCollisionToPlayer",
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);
                        }
                        else
                            OnCollisionToPlayer(ownerID, other.gameObject.name, other.transform.position);

                        onEffect = true;
                    }
                }
                break;
            //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "ParentObstacle":
            case "Item_Box":
                onEffect = true;
                break;
            default:
                break;
        }

        if (!onEffect)
            return;

        PhotonNetwork.Destroy(this.gameObject);

        if (PhotonNetwork.OfflineMode)
        {
            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot);
                var psHit = hitVFX.GetComponentInChildren<ParticleSystem>();
                if (psHit != null)
                    Destroy(hitVFX, psHit.main.duration);
                else
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
            }
        }
        else
        {
            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up); 
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
        }
    }
}
