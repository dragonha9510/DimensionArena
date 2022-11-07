using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using ManagerSpace;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject의 주인
    public string ownerID;
    
    protected AudioSource audioSource;
    //
    [SerializeField] protected int ultimatePoint;
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }
    public GameObject hitPrefab;

    protected bool isWater;

    [PunRPC]
    protected void OnCollisionToPlayer(string ownerId, string targetId, Vector3 pos)
    {

        FloatingText.Instance.CreateFloatingTextForDamage(pos, damage);

        PlayerInfoManager.Instance.
                        CurSkillPtIncrease(ownerId, ultimatePoint);
        //Damaged
        PlayerInfoManager.Instance.
                       CurHpDecrease(ownerId, targetId, damage);

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


    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;

    //    switch (collision.gameObject.tag)
    //    {
    //            //상대 Player에게 데미지를 준 경우, 
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
    //            //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
    //        case "Obstacle":
    //            {

    //            }
    //            break;   
    //    }
    //}
}
