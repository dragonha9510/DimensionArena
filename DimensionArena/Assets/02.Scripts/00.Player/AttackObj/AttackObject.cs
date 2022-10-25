using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject의 주인
    public string ownerID;
    
    protected AudioSource audioSource;
    //
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

 
    [PunRPC]
    protected void OnCollisionToPlayer(string ownerId, string targetId )
    {
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

    private void OnTriggerEnter(Collider collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        switch (collision.gameObject.tag)
        {
                //상대 Player에게 데미지를 준 경우, 
            case "Player":
                {
                    if(ownerID != collision.gameObject.name)
                    {
                        FloatingText.Instance.CreateFloatingTextForDamage(collision.transform.position, damage);
                        photonView.RPC("OnCollisionToPlayer",
                        RpcTarget.All,
                        ownerID,
                        collision.gameObject.name);

                        
                    }
                }
                break;
                //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
