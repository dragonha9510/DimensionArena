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
        if (!isAttack)
            StartAttackCoroutine();
    }
    
    private void StartAttackCoroutine()
    {
        photonView.RPC("AttackCoroutine", RpcTarget.MasterClient 
                                            , transform.position + attackDirection
                                            , attackDirection
                                            , range
                                            , projectileSpeed
                                            , this.gameObject.name);
    }

    public override void Skill()
    {
        //Skill구현
    }
                         

    // Direction Change 삭제
    [PunRPC]
    private IEnumerator AttackCoroutine(Vector3 Pos , Vector3 dir, float range, float speed, string ownerName)
    {

        isAttack = true;
        //player.CanDirectionChange = false;

        GameObject projectile;

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {

                Debug.Log("총알생성");

                projectile = PhotonNetwork.Instantiate("projectile", Pos, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(dir, range, speed);
                projectile.GetComponent<Projectile>().ownerID = ownerName;
                yield return new WaitForSeconds(burst_delay);
            }

            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
        //owner.CanDirectionChange = true;
    }


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


