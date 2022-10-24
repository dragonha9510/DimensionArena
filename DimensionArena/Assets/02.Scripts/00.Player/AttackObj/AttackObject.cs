using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject의 주인
    public string ownerID;
    //JSB
    protected AudioSource audioSource;
    //
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

 
    [PunRPC]
    protected void OnCollisionToPlayer(string targetId )
    {
        PlayerInfoManager.Instance.
                        CurSkillPtIncrease(ownerID,ultimatePoint);
        //Damaged
        PlayerInfoManager.Instance.
                       CurHpDecrease(ownerID, targetId, damage);

        PlayerInfoManager.Instance.DeadCheckCallServer(ownerID);
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
                        collision.gameObject.name);
                        PhotonNetwork.Destroy(this.gameObject);
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
