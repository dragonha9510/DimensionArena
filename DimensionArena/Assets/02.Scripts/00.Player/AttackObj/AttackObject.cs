using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using ManagerSpace;

public class AttackObject : MonoBehaviourPun
{
    //AttackObject¿« ¡÷¿Œ
    public string ownerID;

    [SerializeField] protected string audioClipName;
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

    }
}
