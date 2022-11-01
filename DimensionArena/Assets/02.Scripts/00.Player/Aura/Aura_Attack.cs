using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerSpace
{
    public class Aura_Attack : Player_Atk
    {

        [Header("AuraAttackInfo")]
        [SerializeField] private int projectileCount = 1;
        [SerializeField] private float projectileSpeed = 8.0f;
        [SerializeField] private float attack_delay = 0.25f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;

        protected override void InitalizeAtkInfo()
        {
            atkInfo = new PlayerAtkInfo(6.0f, 1, 3f);
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Attack()
        {
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
            {
                // 아우라 공격
            }
        }


    }

}
