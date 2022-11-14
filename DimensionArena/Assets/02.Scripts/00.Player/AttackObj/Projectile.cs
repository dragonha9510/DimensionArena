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
    float range = 10.0f;

    [SerializeField]
    private float speed;


    private void Awake()
    {
        originPos = transform.position;
        rigid.velocity = this.transform.forward * speed;

        //if (audioClipName == null)
        //    return;
        //SoundManager.Instance.PlaySFXOneShotAudioSource(audioClipName, this.audioSource);
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
                            photonView.RPC("OnCollisionToPlayer",
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);
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
            case "Item_Box":
                onEffect = true;
                break;
            default:
                break;
        }

        if (!onEffect)
            return;

        if (hitPrefab == null)
            return;

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
                photonView.RPC(nameof(CreateEffectForAllClient), RpcTarget.All, pos, rot);
            }
        }

        PhotonNetwork.Destroy(this.gameObject);  
    }



    [PunRPC]
    private void CreateEffectForAllClient(Vector3 pos, Quaternion rot)
    {
        var hitVFX = Instantiate(hitPrefab, pos, rot);
        var psHit = hitVFX.GetComponentInChildren<ParticleSystem>();
        Destroy(hitVFX, psHit.main.duration);
    }



    IEnumerator DestroyEffect(GameObject effectObj,float time)
    {

        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(effectObj);
    }
}
