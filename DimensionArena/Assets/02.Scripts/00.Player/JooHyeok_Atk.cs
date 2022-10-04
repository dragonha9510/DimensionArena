using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JooHyeok_Atk: Player_Atk
{
    [SerializeField] GameObject prefab_Projectile;

    private int     projectileCount = 3;
    private float   projectileSpeed = 8.0f;

    private float   burst_delay = 0.1f;
    private float   attack_delay = 0.25f;


    protected override void Start()
    {
        base.Start();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Attack()
    {
        if(!isAttack)
            StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        GameObject projectile;

        for (int i = 0; i < 2; ++i)
        {
            for(int j = 0; j < projectileCount; ++j)
            {
                projectile = PhotonNetwork.Instantiate("projectile", transform.position + attackDirection, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(attackDirection, range, projectileSpeed);
                projectile.GetComponent<Projectile>().owner = this.gameObject;
                yield return new WaitForSeconds(burst_delay);
            }
            yield return new WaitForSeconds(attack_delay);
        }

        isAttack = false;
    }

    public override void Skill1()
    {
        
    }

}


