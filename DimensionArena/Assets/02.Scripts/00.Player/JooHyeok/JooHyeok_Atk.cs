using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JooHyeok_Atk: Player_Atk
{
    [Header("JooHyeokAttackInfo")]
    [SerializeField] private int     projectileCount = 3;
    [SerializeField] private float   projectileSpeed = 8.0f;
    [SerializeField] private float   burst_delay = 0.1f;
    [SerializeField] private float   attack_delay = 0.25f;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab_Projectile;
    [SerializeField] private AudioSource audioSource;


    protected override void Start()
    {
        //ÇâÈÄ, ¹Ù²î´Â°Ô ¾ø´Ù¸é Player Start, LateUpdate¸¦ private·Î º¯È¯
        base.Start();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Attack()
    {
        if (!isAttack && curCost >= shotCost)
            StartAttackCoroutine();
        else if(curCost < shotCost)
            WaitAttack();
    }
    
    private void StartAttackCoroutine()
    {
        owner.CanDirectionChange = false;

        if(PhotonNetwork.IsConnected)
        {
            photonView.RPC("AttackCoroutine", RpcTarget.MasterClient
                                                , PhotonNetwork.NickName
                                                , attackDirection
                                                , range
                                                , projectileSpeed
                                                , gameObject.name);

        }
        else
            StartCoroutine(AttackCoroutineSingle(null
                                                ,attackDirection
                                                ,range
                                                ,projectileSpeed
                                                ,gameObject.name));

    }

    public override void Skill()
    {
        //Skill±¸Çö
    }

    [PunRPC]
    private IEnumerator AttackCoroutine(string shooter, Vector3 shooterAttackDir, 
        float range, float speed, string ownerName)
    {
        isAttack = true;

        //ÇÑ¹ß ½ò¶§¸¶´Ù ShotCost°¡ »©Áü
        curCost -= shotCost;
       
        GameObject projectile;
        Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);


        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {

                projectile = PhotonNetwork.Instantiate("projectile", shooterPosition.position + shooterAttackDir, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, range, speed);
                projectile.GetComponent<Projectile>().ownerID = ownerName;
                yield return new WaitForSeconds(burst_delay);
            }
            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        owner.CanDirectionChange = true;

    }

    private IEnumerator AttackCoroutineSingle(string shooter, Vector3 shooterAttackDir,
        float range, float speed, string ownerName)
    {
        isAttack = true;

        //ÇÑ¹ß ½ò¶§¸¶´Ù ShotCost°¡ »©Áü
        curCost -= shotCost;

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {
                yield return new WaitForSeconds(burst_delay);
            }
            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        owner.CanDirectionChange = true;
    }

}


