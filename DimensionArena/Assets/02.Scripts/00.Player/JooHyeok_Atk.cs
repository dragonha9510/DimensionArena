using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JooHyeok_Atk: Player_Atk
{
    // JSB
    private AudioSource audioSource;
    //
    [SerializeField] GameObject prefab_Projectile;

    private int     projectileCount = 3;
    private float   projectileSpeed = 8.0f;

    private float   burst_delay = 0.1f;
    private float   attack_delay = 0.25f;


    protected override void Start()
    {
        base.Start();
        // JSB
        audioSource = GetComponent<AudioSource>();

        //
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
    //JSB
    [PunRPC]
    private void EffectSoundPlay()
    {
        audioSource.clip = SoundManager.Instance.GetClip("JiJooEffect");
        audioSource.Play();
    }
    //
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
            
        GameObject projectile;

        for (int i = 0; i < 2; ++i)
        {
            for(int j = 0; j < projectileCount; ++j)
            {
                //JSB
                base.photonView.RPC("EffectSoundPlay", RpcTarget.All);
                //
                projectile = PhotonNetwork.Instantiate("projectile", transform.position + attackDirection, Quaternion.identity);
                projectile.GetComponent<Projectile>().AttackToDirection(attackDirection, range, projectileSpeed);
                projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;
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


