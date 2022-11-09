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
        [SerializeField] private float attack_delay = 6f;

        [SerializeField] private float attack_range = 10f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;


        [SerializeField]
        private Animator auraAnimator;

        private Queue<Quaternion> projectileDirection = new Queue<Quaternion>();

        private int nowPlayAnimationIndex = 0;

        protected override void InitalizeAtkInfo()
        {
        }

        protected override void Start()
        {
            base.Start();
            string auraAttackName = "AttackSpeed";
            for(int i = 0; i < attackCount; ++i)
            {
                auraAnimator.SetFloat(auraAttackName + i.ToString(), animation_speed[i]);
            }
            isAura = true;
        }


        public override void Attack()
        {
            Debug.Log(tmpDirection);
            if(base.tmpDirection != this.transform.forward)
            {
                Debug.Log("강제 회전");
                this.transform.rotation = Quaternion.LookRotation(tmpDirection);
            }
            projectileDirection.Enqueue(this.transform.rotation);

            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
            {
                // 아우라 공격
                atkInfo.SubCost(atkInfo.ShotCost);
                AnimationTriggerSetting();
            }
        }

        [PunRPC]
        private void MakeProjectileOnServer(string prefapName,Vector3 attackDirection,Vector3 pos,Quaternion rot)
        {
            GameObject proj  = PhotonNetwork.Instantiate(prefapName, pos, rot);
            proj.GetComponent<Projectile>().AttackToDirection(AtkInfo.Range, projectileSpeed);
            proj.GetComponent<Projectile>().ownerID = this.gameObject.name;
            owner.CanDirectionChange = true;
        }

        private void MakeProjectile()
        {
            Quaternion direction = projectileDirection.Dequeue();

            if (PhotonNetwork.InRoom && photonView.IsMine)
            {
               photonView.RPC(nameof(MakeProjectileOnServer), RpcTarget.MasterClient, prefab_Projectile.name, owner.Attack.tmpDirection, this.transform.position + (Vector3.up * 0.5f) + (attackDirection * 0.5f), direction);
            }
            else
            {
                GameObject projectile;
                projectile = Instantiate(prefab_Projectile, this.transform.position + (Vector3.up * 0.5f) + (attackDirection * 0.5f), direction);
                projectile.GetComponent<Projectile>().AttackToDirection(AtkInfo.Range, projectileSpeed);
                projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;

            }
        }
        
        IEnumerator AttackTime()
        {
            owner.CanDirectionChange = false;
            yield return new WaitForSeconds(nextAnimation_delay[nowPlayAnimationIndex]);
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
            {
                auraAnimator.SetBool("Attack1", true);
            }
            StartCoroutine(nameof(AttackTime));
        }

        public override void AutoAttack()
        {
        }
    }

}
