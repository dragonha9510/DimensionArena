using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : AttackObject
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Vector3 originPos;

    //basic Range 10
    [SerializeField]
    float range;

    [SerializeField]
    private float speed;

    protected virtual void Awake()
    {
        originPos = transform.position;
        rigid.velocity = this.transform.forward * speed;

        if (audioClipName == null)
           return;

        SoundManager.Instance.PlaySFXOneShotInRange(60f, this.transform, audioClipName);
    }


    private void LateUpdate()
    {
        if(!PhotonNetwork.InRoom)
        {
            if (range < (this.transform.position - originPos).magnitude)
            {
                Destroy(this.gameObject);
            }
        }
       
        if(PhotonNetwork.IsMasterClient && range < (this.transform.position - originPos).magnitude)
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
                            photonView.RPC(nameof(OnCollisionToPlayer),
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);

                            if(other.gameObject.GetComponent<PhotonView>().IsMine)
                            {
                               // Android_Vibrator.CreateOneShot(250);
                            }
                        }
                        else
                        {
                            OnCollisionToPlayer(ownerID, other.gameObject.name, other.transform.position);
                        }

                        onEffect = true;
                    }
                }
                break;
            //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "ParentObstacle":
                onEffect = true;
                break;
            case "Item_Box":
                onEffect = true;
                other.gameObject.GetComponent<ItemBox>().HpDecrease_KnockBack(this.Damage, ownerID);
                break;
            default:
                break;
        }

        if (!onEffect)
            return;

        if (hitPrefab == null)
            return;

        if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
        {
            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (hitPrefab != null)
            {
                CreateEffectForAllClient(pos, rot);
            }
        }
        else
        {
            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up); 
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (hitPrefab != null && PhotonNetwork.InRoom)
            {
                photonView.RPC(nameof(CreateEffectForAllClient), RpcTarget.All, pos, rot);
            }
        }
    }



    [PunRPC]
    private void CreateEffectForAllClient(Vector3 pos, Quaternion rot)
    {

        if (hitAudioClipName != null)
            SoundManager.Instance.PlaySFXOneShot(hitAudioClipName);


        var hitVFX = Instantiate(hitPrefab, pos, rot);
        var psHit = hitVFX.GetComponentInChildren<ParticleSystem>();

        if(psHit != null)
            Destroy(hitVFX, psHit.main.duration);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
            PhotonNetwork.Destroy(this.gameObject);
        else
            Destroy(this.gameObject);
    }



    [PunRPC]
    protected void SetDisAbleObjectForServer()
    {
        gameObject.SetActive(false);
    }

    protected void DestroyObjectForServer()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

}
