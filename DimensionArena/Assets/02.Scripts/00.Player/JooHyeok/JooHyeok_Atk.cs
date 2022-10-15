using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
            atkInfo = new PlayerAtkInfo(6.0f, 3, 1.5f);
        }
        protected override void Start()
        {
            //향후, 바뀌는게 없다면 Player Start, LateUpdate를 private로 변환
            base.Start();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
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

            if (PhotonNetwork.InRoom)
            {
                photonView.RPC("AttackCoroutine", RpcTarget.MasterClient
                                                    , PhotonNetwork.NickName
                                                    , attackDirection
                                                    , atkInfo.Range
                                                    , projectileSpeed
                                                    , gameObject.name);

            }
            else
                StartCoroutine(AttackCoroutineSingle(null
                                                    , attackDirection
                                                    , atkInfo.Range
                                                    , projectileSpeed
                                                    , gameObject.name));

        }

        public override void Skill()
        {
            //Skill구현
        }

        [PunRPC]
        private IEnumerator AttackCoroutine(string shooter, Vector3 shooterAttackDir,
            float range, float speed, string ownerName)
        {
            isAttack = true;

            //한발 쏠때마다 ShotCost가 빼짐
            atkInfo.SubCost(atkInfo.ShotCost);

            GameObject projectile;
            Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);


            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {

                    projectile = PhotonNetwork.Instantiate("projectile", shooterPosition.position + shooterAttackDir, Quaternion.identity);
                    projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, range, speed);
                    projectile.GetComponent<Projectile>().ownerID = ownerName;
                    yield return new WaitForSeconds(burst_delay);
                }
                yield return new WaitForSeconds(attack_delay);
            }

            isAttack = false;
            owner.CanDirectionChange = true;

        }

        private IEnumerator AttackCoroutineSingle(string shooter, Vector3 shooterAttackDir,
            float range, float speed, string ownerName)
        {
            isAttack = true;

            atkInfo.SubCost(atkInfo.ShotCost);

            GameObject projectile;

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < projectileCount; ++j)
                {
                    // JSB
                    projectile = Instantiate(prefab_Projectile, this.transform.position + shooterAttackDir, Quaternion.identity);
                    projectile.GetComponent<Projectile>().AttackToDirection(shooterAttackDir, range, speed);
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