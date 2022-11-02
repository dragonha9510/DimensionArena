using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace PlayerSpace
{
    public class Aura_Attack : Player_Atk
    {
        [Header("AnimationCustom")]
        [SerializeField] private List<float> animation_speed = new List<float>();
        [SerializeField] private List<float> nextAnimation_delay = new List<float>();
        [Header("AuraAttackInfo")]
        [SerializeField] private int attackCount = 3;
        [SerializeField] private int projectileCount = 3;
        [SerializeField] private float projectileSpeed = 8.0f;
        [SerializeField] private float attack_delay = 0.5f;

        [SerializeField] private float attack_range = 10f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;

        [SerializeField]
        private Animator auraAnimator;

        private int nowPlayAnimationIndex = 0;

        protected override void InitalizeAtkInfo()
        {
            atkInfo = new PlayerAtkInfo(attack_range, projectileCount, attack_delay);
        }

        protected override void Start()
        {
            base.Start();
            string auraAttackName = "AttackSpeed";
            for(int i = 0; i < attackCount; ++i)
            {
                auraAnimator.SetFloat(auraAttackName + i.ToString(), animation_speed[i]);
            }
        }

        public override void Attack()
        {
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
            {
                // 아우라 공격
                AnimationTriggerSetting();
            }
        }
        private void MakeProjectile()
        {
            GameObject projectile;
            projectile = Instantiate(prefab_Projectile, this.transform.position + (Vector3.up * 0.5f) + (attackDirection * 0.5f), this.transform.rotation);
            projectile.GetComponent<Aura_Projectile>().AttackToDirection(this.transform.position, attackDirection, AtkInfo.Range, projectileSpeed);
            projectile.GetComponent<Aura_Projectile>().ownerID = this.gameObject.name;
        }
        
        IEnumerator AttackTime()
        {
            owner.CanDirectionChange = false;
            yield return new WaitForSeconds(nextAnimation_delay[nowPlayAnimationIndex]);
            Debug.Log("Bool초기화");
            auraAnimator.SetBool("Attack1", false);
            auraAnimator.SetBool("Attack2", false);
            auraAnimator.SetBool("Attack3", false);
            owner.CanDirectionChange = true;
            yield break;
        }


        private void AnimationTriggerSetting()
        {
            StopCoroutine(nameof(AttackTime));
            if(true == auraAnimator.GetBool("Attack3"))
            {
                auraAnimator.SetBool("Attack1", true);
                auraAnimator.SetBool("Attack3", false);
                nowPlayAnimationIndex = 0;
            }
            else if (true == auraAnimator.GetBool("Attack2"))
            {
                auraAnimator.SetBool("Attack3", true);
                auraAnimator.SetBool("Attack2", false);
                nowPlayAnimationIndex = 1;
            }
            else if (true == auraAnimator.GetBool("Attack1"))
            {
                auraAnimator.SetBool("Attack2", true);
                auraAnimator.SetBool("Attack1", false);
                nowPlayAnimationIndex = 2;
            }
            else
                auraAnimator.SetBool("Attack1", true);
            StartCoroutine(nameof(AttackTime));
        }

    }

}
