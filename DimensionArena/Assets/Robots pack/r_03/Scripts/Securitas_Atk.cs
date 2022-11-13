using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Securitas_Atk : Player_Atk
    {
        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private GameObject prefab_EnforceProjectile;
        [SerializeField] private AudioSource audioSource;

        [Header("Securitas Passive")]
        [SerializeField] private float passiveTime;
        private float curpassiveTime;
        private bool passiveReady;

        protected override void InitalizeAtkInfo()
        {
            atkInfo = new PlayerAtkInfo(10f, 3, 2.25f);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected void Update()
        {
            if(passiveReady)
                return;

            curpassiveTime += Time.deltaTime;

            if (curpassiveTime >= passiveTime)
            {
                curpassiveTime = 0;
                passiveReady = true;
            }
        }

        public override void Attack()
        {
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
                StartAttackCoroutine();
        }

        private void StartAttackCoroutine()
        {
            owner.CanDirectionChange = false;
            isAttack = true;

            if (!PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient, gameObject.name, tmpDirection, photonView.Controller);
            else
                StartCoroutine(SingleCreateProjectile(gameObject.name, tmpDirection));
        }

        private IEnumerator SingleCreateProjectile(string shooter, Vector3 shooterAttackDir)
        {
            GameObject projectile;
            Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);

            SubMagazine(shooter);
            SetAttackTrigger();


            if (passiveReady)
            {
                passiveReady = false;
                projectile = Instantiate(prefab_EnforceProjectile, shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
            }
            else
                projectile = Instantiate(prefab_Projectile, shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));



            projectile.GetComponent<Projectile>().ownerID = shooter;

            EndAttack( shooter);

            yield break;
        }

        [PunRPC]
        public void SetAttackTrigger()
        {
            AtkTrigger();
        }

        [PunRPC]
        private IEnumerator MasterCreateProjectile(string shooter, Vector3 shooterAttackDir,
                                                   Photon.Realtime.Player controller)
        {
            GameObject projectile;
            Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);

            photonView.RPC(nameof(SubMagazine), controller, shooter);
            photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);

            string prefabname;

            if (passiveReady)
            {
                prefabname = prefab_EnforceProjectile.name;
                passiveReady = false;
            }
            else
                prefabname = prefab_Projectile.name;

            projectile = PhotonNetwork.Instantiate(
                prefabname,
                shooterPosition.position + (Vector3.up * 0.5f),
                Quaternion.LookRotation(shooterAttackDir, Vector3.up));

            projectile.GetComponent<Projectile>().ownerID = shooter;

            photonView.RPC("EndAttack", controller, shooter);

            yield break;
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
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
                StartCoroutine(LookAttackAutoDirection(true, (autoAtk.targetPos - transform.position)));
        }

        [PunRPC]
        IEnumerator LookAttackAutoDirection(bool isServer, Vector3 autoDirection)
        {
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

            if (!PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
            {
                SubMagazine(owner.name);
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient, gameObject.name
                                                             , autoDirection
                                                             , photonView.Controller);
            }
            else
            {
                SubMagazine(owner.name);
                StartCoroutine(SingleCreateProjectile(gameObject.name, autoDirection));
            }
        }
    }
}
