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
            atkInfo.SubCost(atkInfo.ShotCost);

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

            //float offset;
            photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
            for (int i = 0; i < 3; ++i)
            {
                // offset = (i % 2 == 0) ? 0.3f : -0.3f;
                //projectile = PhotonNetwork.Instantiate("SA_Projectile", shooterPosition.position + (Vector3.right * offset) + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
                projectile = PhotonNetwork.Instantiate("SA_Projectile", shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
                projectile.GetComponent<Projectile>().ownerID = shooter;

                yield return attackDelay;
            }
            photonView.RPC("EndAttack", controller, shooter);
        }

        private IEnumerator AttackCoroutineSingle(string shooter, Quaternion playerRotation, Vector3 shooterAttackDir, string ownerName)
        {
            GameObject projectile;
            WaitForSeconds attackDelay = new WaitForSeconds(attack_delay);
    
            AtkTrigger();
            for (int i = 0; i < 3; ++i)
            {
                projectile = PhotonNetwork.Instantiate("SA_Projectile", transform.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
                projectile.GetComponent<Projectile>().ownerID = shooter;

                yield return attackDelay;
            }

            isAttack = false;
            owner.CanDirectionChange = true;
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



        /// ======================================
        /// 
        /// AutoAtk Region
        /// 
        /// ======================================


        public override void AutoAttack()
        {
            // 자동 공격 루틴 추가
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
                StartCoroutine(LookAttackAutoDirection(true, (autoAtk.targetPos - transform.position)));
        }

        [PunRPC]
        IEnumerator LookAttackAutoDirection(bool isServer, Vector3 autoDirection)
        {

            atkInfo.SubCost(atkInfo.ShotCost);

            if (isRotation)
            {
                autoDirection.Normalize();

                Vector3 forward = Vector3.Slerp(transform.forward,
                    autoDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

                while (Vector3.Angle(autoDirection, transform.forward) >= 5)
                {
                    yield return null;
                    forward = Vector3.Slerp(transform.forward,
                    autoDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, autoDirection));

                    transform.LookAt(transform.position + forward);
                }
            }

            if (PhotonNetwork.InRoom)
            {          
                photonView.RPC(nameof(CreateAutoProjectile), RpcTarget.MasterClient, true, autoDirection);
            }
            else
                StartCoroutine(CreateAutoProjectile(false, autoDirection));
        }

        [PunRPC]
        public IEnumerator CreateAutoProjectile(bool isServer, Vector3 autoDirection)
        {
            owner.CanDirectionChange = false;
            isAttack = true;
            photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);

            GameObject projectile;
            autoDirection.Normalize();


            // Server
            if (isServer)
            {
                photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
                for (int i = 0; i < 3; ++i)
                {
                    projectile = PhotonNetwork.Instantiate("SA_Projectile", transform.position + (Vector3.up * 0.5f), Quaternion.LookRotation(autoDirection, Vector3.up));
                    projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;
                    yield return new WaitForSeconds(attack_delay);
                }
            }
            else
            {
                SetAttackTrigger();
                for (int i = 0; i < 3; ++i)
                {
                    projectile = Instantiate(prefab_Projectile, transform.position + (Vector3.up * 0.5f), Quaternion.LookRotation(autoDirection, Vector3.up));
                    projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;
                    yield return new WaitForSeconds(attack_delay);
                }
            }
          

            isAttack = false;
            owner.CanDirectionChange = true;
        }

        protected override void InitalizeAtkInfo()
        {
        }
    }



}
