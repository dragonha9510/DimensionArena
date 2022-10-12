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
        //향후, 바뀌는게 없다면 Player Start, LateUpdate를 private로 변환
        base.Start();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Attack()
    {
        if (!isAttack && curMagazine >= 1)
            StartAttackCoroutine();
    }
    
    private void StartAttackCoroutine()
    {
        owner.CanDirectionChange = false;

        /*
        photonView.RPC("AttackCoroutine", RpcTarget.MasterClient
                                            , PhotonNetwork.NickName
                                            , attackDirection
                                            , range
                                            , projectileSpeed
                                            , this.gameObject.name);
        */
        StartCoroutine(AttackCoroutine(     null
                                            , attackDirection
                                            , range
                                            , projectileSpeed
                                            , this.gameObject.name));
    }

    public override void Skill()
    {
        //Skill구현
    }

    [PunRPC]
    private IEnumerator AttackCoroutine(string shooter, Vector3 shooterAttackDir, 
        float range, float speed, string ownerName)
    {
        isAttack = true;
        //탄창 하나 없애기
        curMagazine -= 1;
        curMagazine = Mathf.Max(curMagazine, 0);


        GameObject projectile;
       // Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);


        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {
                /*
                projectile = PhotonNetwork.Instantiate("projectile", shooterPosition.position + shooterAttackDir, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, range, speed);
                projectile.GetComponent<Projectile>().ownerID = ownerName;
                */
                yield return new WaitForSeconds(burst_delay);
            }
            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        owner.CanDirectionChange = true;

    }
    // Direction Change 삭제
    /*[PunRPC]
    private IEnumerator AttackCoroutine(Transform shooterTrans , Vector3 shooterCorrection , float range, float speed, string ownerName)
    {

        isAttack = true;
        //player.CanDirectionChange = false;
        GameObject projectile;

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {
                projectile = PhotonNetwork.Instantiate("projectile", shooterTrans.position + shooterCorrection , Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(shooterCorrection, range, speed);
                projectile.GetComponent<Projectile>().ownerID = ownerName;
                yield return new WaitForSeconds(burst_delay);
            }
            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        
    }*/


    /*IEnumerator AttackCoroutine()
    {

        isAttack = true;
        owner.CanDirectionChange = false;

        GameObject projectile;

        for (int i = 0; i < 2; ++i)
        {
            for(int j = 0; j < projectileCount; ++j)
            {
                projectile = PhotonNetwork.Instantiate("projectile", transform.position + attackDirection, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(attackDirection, range, projectileSpeed);
                projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;
                yield return new WaitForSeconds(burst_delay);
            }

            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        owner.CanDirectionChange = true;
    }*/
}


