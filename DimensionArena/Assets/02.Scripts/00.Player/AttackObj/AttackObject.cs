using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject�� ����
    public string ownerID;
    
    protected AudioSource audioSource;
    //
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;
    public GameObject hitPrefab;



    [PunRPC]
    protected void OnCollisionToPlayer(string ownerId, string targetId, Vector3 pos)
    {

        FloatingText.Instance.CreateFloatingTextForDamage(pos, damage);

        PlayerInfoManager.Instance.
                        CurSkillPtIncrease(ownerId, ultimatePoint);
        //Damaged
        PlayerInfoManager.Instance.
                       CurHpDecrease(ownerId, targetId, damage);

        PlayerInfoManager.Instance.DeadCheckCallServer(ownerId);

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }

    //JSB
    [PunRPC]
    protected void EffectSoundPlay(string clipName)
    {
        audioSource.clip = SoundManager.Instance.GetClip("clipName");
        audioSource.Play();
    }
    //

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        switch (other.tag)
        {
            //��� Player���� �������� �� ���, 
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
            //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
            case "ParentObstacle":
                {
                    Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);/*Quaternion.FromToRotation(Vector3.up, contact.normal);*/
                    Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                    if (hitPrefab != null)
                    {
                        var hitVFX = Instantiate(hitPrefab, pos, rot);
                        var psHit = hitVFX.GetComponent<ParticleSystem>();
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

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;

    //    switch (collision.gameObject.tag)
    //    {
    //            //��� Player���� �������� �� ���, 
    //        case "Player":
    //            {
    //                if(ownerID != collision.gameObject.name)
    //                {                   
    //                    photonView.RPC("OnCollisionToPlayer",
    //                    RpcTarget.All,
    //                    ownerID,
    //                    collision.gameObject.name,
    //                    collision.transform.position);
    //                }
    //            }
    //            break;
    //            //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
    //        case "Obstacle":
    //            {

    //            }
    //            break;   
    //    }
    //}
}
