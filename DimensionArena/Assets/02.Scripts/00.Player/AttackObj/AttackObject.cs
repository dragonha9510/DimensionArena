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
                //��� Player���� �������� �� ���, 
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
                //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
            case "Obstacle":
                {

                }
                break;   
        }
    }
}
