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
        [SerializeField] private float burst_delay = 0.1f;
        [SerializeField] private float attack_delay = 0.25f;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        
        private int passiveCnt = 0;
        private const int attackCnt = 2;
        private const int passiveAttackCnt = 3;

        [SerializeField] private GameObject passiveEffect;

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
            atkInfo.SubCost(atkInfo.ShotCost);


            if (PhotonNetwork.InRoom)
                     
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient
                                                             , gameObject.name
                                                             , tmpDirection
                                                             , photonView.Controller
                                                    );
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

            if (passiveCnt == 2)
                passiveEffect.SetActive(true);
            else if (passiveCnt == 0)
                passiveEffect.SetActive(false);

            photonView.RPC("EndAttack", controller, shooter);
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
                    projectile = Instantiate(prefab_Projectile, this.transform.position + (Vector3.up * 0.5f), this.transform.rotation);
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
            // ???? ???? ???? ????
            if (atkInfo.CurCost < atkInfo.ShotCost)
                WaitAttack();
            else if (!isAttack)
            {
                ++passiveCnt;
                StartCoroutine(LookAttackAutoDirection(true, (autoAtk.targetPos - transform.position)));
            }
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


            atkInfo.SubCost(atkInfo.ShotCost);


            if (PhotonNetwork.InRoom)
                photonView.RPC(nameof(CreateAutoProjectile), RpcTarget.MasterClient, true, autoDirection);
            else
                StartCoroutine(CreateAutoProjectile(false, autoDirection));
        }

        [PunRPC]
        public IEnumerator CreateAutoProjectile(bool isServer, Vector3 autoDirection)
        {
            owner.CanDirectionChange = false;
            isAttack = true;

            //

            GameObject projectile;

            int atkCnt = attackCnt;

            if (passiveCnt == passiveAttackCnt)
            {
                atkCnt = passiveAttackCnt;
                passiveCnt = 0;
            }

            autoDirection.Normalize();

            for (int i = 0; i < atkCnt; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    // JSB
                    if (isServer)
                    {
                        photonView.RPC(nameof(SetAttackTrigger), RpcTarget.All);
                        projectile = PhotonNetwork.Instantiate(prefab_Projectile.name, this.transform.position + (Vector3.up * 0.5f), Quaternion.LookRotation(autoDirection, Vector3.up));
                    }
                    else
                    {
                        SetAttackTrigger();
                        projectile = Instantiate(prefab_Projectile, this.transform.position + (Vector3.up * 0.5f), Quaternion.LookRotation(autoDirection, Vector3.up));
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



        [PunRPC]
        public void SetAttackTrigger()
        {
            AtkTrigger();
        }
    }

}
