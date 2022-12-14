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


        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;

        [SerializeField]
        private Animator auraAnimator;


        [SerializeField]
        private string[] attackClip;

        private Queue<Quaternion> projectileDirection = new Queue<Quaternion>();

        private int nowPlayAnimationIndex = 0;

        bool isFinishShoot = true;

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
            //isAura = true;
        }


        public override void Attack()
        {
            if (!photonView.IsMine)
                return;

            if(base.tmpDirection != this.transform.forward)
            {
                this.transform.rotation = Quaternion.LookRotation(tmpDirection);
            }
            
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (true == isFinishShoot)
            {
                // 아우라 공격
                projectileDirection.Enqueue(Quaternion.LookRotation(tmpDirection, Vector3.up));
                atkInfo.SubCost(atkInfo.ShotCost);
                AnimationTriggerSetting();
            }
           
        }

        [PunRPC]
        private void IsBattleOn()
        {
            SoundManager.Instance.PlaySFXOneShot(attackClip[Random.Range(0, attackClip.Length)]);
            Owner.Info.BattleOn();
        }


        [PunRPC]
        private void MakeProjectileOnServer(string prefapName,Vector3 attackDirection,Vector3 pos,Quaternion direction)
        {
            photonView.RPC(nameof(IsBattleOn), RpcTarget.All);
            GameObject proj  = PhotonNetwork.Instantiate(prefapName, pos + (Vector3.up * 1f) , direction);
            proj.GetComponent<Projectile>().ownerID = this.gameObject.name;
            owner.CanDirectionChange = true;
        }

        private void MakeProjectile()
        {

            if (PhotonNetwork.InRoom && photonView.IsMine)
            {
                Quaternion direction = projectileDirection.Dequeue();
                photonView.RPC(nameof(MakeProjectileOnServer), RpcTarget.MasterClient, prefab_Projectile.name, owner.Attack.tmpDirection, this.transform.position, direction);
                isFinishShoot = true;
            }
            else if(!PhotonNetwork.InRoom)
            {

                Quaternion direction = projectileDirection.Dequeue();
                GameObject projectile;
                projectile = Instantiate(prefab_Projectile, this.transform.position + (Vector3.up * 0.5f) + (attackDirection * 0.5f), direction);
                projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;

            }
        }
        
        IEnumerator AttackTime()
        {
            isFinishShoot = false;
            owner.CanDirectionChange = false;
            yield return new WaitForSeconds(nextAnimation_delay[nowPlayAnimationIndex]);
            projectileDirection.Clear();
            isFinishShoot = true;

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
            tmpDirection = (autoAtk.targetPos - this.transform.position).normalized;
            Attack();
        }


    }

}
