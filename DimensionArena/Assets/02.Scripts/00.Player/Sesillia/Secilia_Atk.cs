using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

namespace PlayerSpace
{
    public class Secilia_Atk : Player_Atk
    {
        [Header("SesilliaAttackInfo")]
        [SerializeField] private int projectileCount = 3;
        [SerializeField] private float projectileSpeed = 8.0f;
        [SerializeField] private float attack_delay = 0.05f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;

        protected override void Start()
        {
            base.Start();
        }

        [PunRPC]
        public void SetAttackTrigger()
        {
            AtkTrigger();
        }

        public override void Attack()
        {
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
                StartAttackCoroutine();

            // Animation Temp

        }

        private void StartAttackCoroutine()
        {
            owner.CanDirectionChange = false;
            isAttack = true;

            if (PhotonNetwork.InRoom)
            {
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient
                                                             , gameObject.name
                                                             , tmpDirection
                                                             , photonView.Controller
                                                    );

            }
            else
                StartCoroutine(AttackCoroutineSingle(null
                                                    , transform.rotation
                                                    , attackDirection
                                                    , gameObject.name));

        }



        [PunRPC]
        private IEnumerator MasterCreateProjectile(string shooter, Vector3 shooterAttackDir,
                                                   Photon.Realtime.Player controller)
        {
            GameObject projectile;
            Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);
            WaitForSeconds attackDelay = new WaitForSeconds(attack_delay);

            photonView.RPC(nameof(SubMagazine), controller, shooter);
            float offset;

            photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
            for (int i = 0; i < 3; ++i)
            {
                offset = (i % 2 == 0) ? 0.3f : -0.3f;
                projectile = PhotonNetwork.Instantiate("SA_Projectile", shooterPosition.position + (Vector3.right * offset) + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
                projectile.GetComponent<Projectile>().ownerID = shooter;

                yield return attackDelay;
            }
            photonView.RPC("EndAttack", controller, shooter);
        }

        private IEnumerator AttackCoroutineSingle(string shooter, Quaternion playerRotation, Vector3 shooterAttackDir, string ownerName)
        {
            atkInfo.SubCost(atkInfo.ShotCost);

            GameObject projectile;
            WaitForSeconds attackDelay = new WaitForSeconds(attack_delay);
            float right = 0.15f;
            Vector3 offset = shooterAttackDir * 0.1f + (Vector3.up * 0.5f);
            Vector3 position1 = transform.position + offset + transform.right * 0.15f;
            Vector3 position2 = transform.position + offset + transform.right * -0.15f;
            Vector3 pos;


            AtkTrigger();
            for (int i = 0; i < 3; ++i)
            {
                pos = i % 2 == 0 ? position1 : position2;
                right = i % 2 == 0 ? 0.15f : -0.15f;
                projectile = Instantiate(prefab_Projectile, pos, playerRotation);
                projectile.GetComponent<Projectile>().ownerID = ownerName;
                yield return attackDelay;
            }

            isAttack = false;
            owner.CanDirectionChange = true;
        }



        [PunRPC]
        private void SubMagazine(string name)
        {
            if (gameObject.name == name)
                atkInfo.SubCost(atkInfo.ShotCost);
        }


        [PunRPC]
        private void EndAttack(string name)
        {
            if (gameObject.name == name)
            {
                owner.CanDirectionChange = true;
                isAttack = false;
            }
        }


        

        public override void AutoAttack()
        {
            // 자동 공격 루틴 추가
        }

        protected override void InitalizeAtkInfo()
        {
        }
    }



}
