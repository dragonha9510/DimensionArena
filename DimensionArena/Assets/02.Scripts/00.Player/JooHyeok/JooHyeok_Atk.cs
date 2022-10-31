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
                StartAttackCoroutine();

            // Animation Temp
            AtkTrigger();
        }


        private void StartAttackCoroutine()
        {
            owner.CanDirectionChange = false;
            isAttack = true;

            if (PhotonNetwork.InRoom)
            {
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient
                                                             , gameObject.name
                                                             , attackDirection
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
            photonView.RPC(nameof(SubMagazine), controller, shooter);

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    projectile = PhotonNetwork.Instantiate("projectile", shooterPosition.position + (Vector3.up * 0.5f), shooterPosition.rotation);
                    projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, AtkInfo.Range, projectileSpeed);
                    projectile.GetComponent<Projectile>().ownerID = shooter;
                    yield return new WaitForSeconds(burst_delay);
                }
                yield return new WaitForSeconds(attack_delay);
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

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    // JSB
                    projectile = Instantiate(prefab_Projectile, this.transform.position + shooterAttackDir + (Vector3.up * 0.5f), playerRotation);
                    projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, AtkInfo.Range, projectileSpeed);
                    projectile.GetComponent<Projectile>().ownerID = ownerName;
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
