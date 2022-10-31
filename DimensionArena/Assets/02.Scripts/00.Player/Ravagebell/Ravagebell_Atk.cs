using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Ravagebell_Atk : Player_Atk
    {
        [Header("RavagebellAttackInfo")]
        [SerializeField] private float projectileSpeed = 8.0f;
        [SerializeField] private float atkDelay;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Muzzle;
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private AudioSource audioSource;

        protected override void InitalizeAtkInfo()
        {

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


            projectile = PhotonNetwork.Instantiate("projectile_ravagebell", shooterPosition.position + (Vector3.up * 0.5f), shooterPosition.rotation);
            projectile.GetComponent<Projectile>().AttackToDirection(Vector3.up, AtkInfo.Range, projectileSpeed);
            projectile.GetComponent<Projectile>().ownerID = shooter;
            yield return new WaitForSeconds(atkDelay);

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
            if (gameObject.name == name)
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

            yield return new WaitForSeconds(atkDelay);

            Instantiate(prefab_Muzzle, this.transform.position + shooterAttackDir + (Vector3.up * 0.5f), playerRotation);
            projectile = Instantiate(prefab_Projectile, this.transform.position + shooterAttackDir + (Vector3.up * 0.5f), playerRotation);
            projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, AtkInfo.Range, projectileSpeed);
            projectile.GetComponent<Projectile>().ownerID = ownerName;


            isAttack = false;
            owner.CanDirectionChange = true;
        }
    }
}