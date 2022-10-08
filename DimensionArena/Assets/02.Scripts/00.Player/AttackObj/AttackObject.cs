using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject�� ����
    public string ownerID;
    //JSB
    protected AudioSource audioSource;
    //
    [SerializeField] private int ultimatePoint;
    [SerializeField] private int damage;

 
    [PunRPC]
    protected void OnCollisionToPlayer(string id)
    {
        PlayerInfoManager.Instance.
                        CurSkillPtIncrease(ownerID,ultimatePoint);
        //Damaged
        PlayerInfoManager.Instance.
                       CurHpDecrease(id, damage);

        if (!photonView.IsMine)
            return;

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
                        photonView.RPC("OnCollisionToPlayer",
                        RpcTarget.All,
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
