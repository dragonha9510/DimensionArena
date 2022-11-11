using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class JooHyeok_Atk : Player_Atk
    {
        [Header("JooHyeokAttackInfo")]
        [SerializeField] private int projectileCount = 3;
        [SerializeField] private float projectileSpeed = 8.0f;
        [SerializeField] private float burst_delay = 0.1f;
        [SerializeField] private float attack_delay = 0.25f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;

        private int passiveCnt = 0;
        private const int attackCnt = 2;
        private const int passiveAttackCnt = 3;

        protected override void InitalizeAtkInfo()
        {
            atkInfo = new PlayerAtkInfo(6.0f, 3, 2.25f);
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
                ++passiveCnt;
                StartAttackCoroutine();
            }
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

            int atkCnt = 2;

            if(passiveCnt == 3)
            {
                atkCnt = 3;
                passiveCnt = 0;
            }


            WaitForSeconds burstDelay = new WaitForSeconds(burst_delay);
            WaitForSeconds attackDelay = new WaitForSeconds(attack_delay);

            

            for (int i = 0; i < atkCnt; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
                    projectile = PhotonNetwork.Instantiate("projectile", shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
                    projectile.GetComponent<Projectile>().ownerID = shooter;
                    yield return burstDelay;
                }
                yield return attackDelay;
            }

            photonView.RPC("EndAttack", controller, shooter);
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
            if(gameObject.name == name)
            {
                owner.CanDirectionChange = true;
                isAttack = false;
            }
        }


        private IEnumerator AttackCoroutineSingle(string shooter, Quaternion playerRotation, Vector3 shooterAttackDir, string ownerName)
        {
            isAttack = true;

            atkInfo.SubCost(atkInfo.ShotCost);

            GameObject projectile;

            int atkCnt = attackCnt;

            if (passiveCnt == passiveAttackCnt)
            {
                atkCnt = passiveAttackCnt;
                passiveCnt = 0;
            }

            for (int i = 0; i < atkCnt; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    // JSB
                    projectile = Instantiate(prefab_Projectile, this.transform.position + shooterAttackDir + (Vector3.up * 0.5f), this.transform.rotation);
                    projectile.GetComponent<Projectile>().ownerID = ownerName;
                    SetAttackTrigger();
                    //
                    yield return new WaitForSeconds(burst_delay);
                }
                yield return new WaitForSeconds(attack_delay);
            }

            isAttack = false;
            owner.CanDirectionChange = true;
        }

        public override void AutoAttack()
        {
            // 자동 공격 루틴 추가
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
            {
                ++passiveCnt;
                if (PhotonNetwork.InRoom)
                    photonView.RPC(nameof(LookAttackAutoDirection), RpcTarget.All, true);
                else
                    StartCoroutine(LookAttackAutoDirection(false));
            }
        }

        [PunRPC]
        IEnumerator LookAttackAutoDirection(bool isServer)
        {
            if (isRotation)
            {
                Vector3 autoDirection = (autoAtk.targetPos - transform.position);
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
                photonView.RPC(nameof(CreateAutoProjectile), RpcTarget.MasterClient, true, this.transform.position, autoAtk.targetPos);
            else
                StartCoroutine(CreateAutoProjectile(false, this.transform.position, autoAtk.targetPos));
        }

        [PunRPC]
        public IEnumerator CreateAutoProjectile(bool isServer, Vector3 ownerPos, Vector3 targetPos)
        {
            owner.CanDirectionChange = false;
            isAttack = true;

            photonView.RPC(nameof(SubMagazine), photonView.Controller, owner.name);

            GameObject projectile;

            int atkCnt = attackCnt;

            if (passiveCnt == passiveAttackCnt)
            {
                atkCnt = passiveAttackCnt;
                passiveCnt = 0;
            }

            Vector3 autoDirection = (targetPos - ownerPos);
            autoDirection.Normalize();

            for (int i = 0; i < atkCnt; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    // JSB
                    if (isServer)
                    {
                        photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
                        projectile = PhotonNetwork.Instantiate(prefab_Projectile.name, ownerPos + autoDirection + (Vector3.up * 0.5f), transform.rotation);
                    }
                    else
                    {
                        SetAttackTrigger();
                        projectile = Instantiate(prefab_Projectile, this.transform.position + autoDirection + (Vector3.up * 0.5f), transform.rotation);
                    }

                    projectile.GetComponent<Projectile>().ownerID = this.gameObject.name;
                    //
                    yield return new WaitForSeconds(burst_delay);
                }
                yield return new WaitForSeconds(attack_delay);
            }

            isAttack = false;
            owner.CanDirectionChange = true;
        }
    }
}
