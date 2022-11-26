using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    // Start is called before the first frame update
    public class Securitas_Atk : Player_Atk
    {
        [Header("Prefab")]
        [SerializeField] private GameObject prefab_Projectile;
        [SerializeField] private GameObject prefab_EnforceProjectile;

        [Header("Securitas Passive")]
        [SerializeField] private float passiveTime;
        private float curpassiveTime;
        private bool passiveReady;
        [SerializeField] private GameObject passiveObject;

        protected override void InitalizeAtkInfo()
        {
            atkInfo = new PlayerAtkInfo(10.0f, 3, 2.25f);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected void Update()
        {
            if (passiveReady)
                return;

            curpassiveTime += Time.deltaTime;

            if (!passiveObject)
                return;
            
            if (curpassiveTime >= passiveTime)
            {
                curpassiveTime = 0;
                passiveReady = true;
                photonView.RPC(nameof(PassiveActive), RpcTarget.All, true);
                //passiveObject.enabled = (true);
                SoundManager.Instance.PlaySFXOneShot("snd_char_securitas_overclock");
            }
            else
                photonView.RPC(nameof(PassiveActive), RpcTarget.All, false);
        }

        [PunRPC]
        private void PassiveActive(bool onoff)
        {
            passiveObject.SetActive(onoff);
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
            atkInfo.SubCost(atkInfo.ShotCost);


            if (!PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient, gameObject.name, tmpDirection, photonView.Controller);
            else
                StartCoroutine(SingleCreateProjectile(gameObject.name, tmpDirection));
        }

        private IEnumerator SingleCreateProjectile(string shooter, Vector3 shooterAttackDir)
        {
            GameObject projectile;
            Transform shooterPosition = PlayerInfoManager.Instance.getPlayerTransform(shooter);

            animator.speed = 3f;
            SetAttackTrigger();

            if (passiveReady)
            {
                passiveReady = false;
                projectile = Instantiate(prefab_EnforceProjectile, shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));
            }
            else
                projectile = Instantiate(prefab_Projectile, shooterPosition.position + (Vector3.up * 0.5f), Quaternion.LookRotation(shooterAttackDir, Vector3.up));

            projectile.GetComponent<Projectile>().ownerID = shooter;

            yield return new WaitForSeconds(0.2f);
            animator.speed = 1f;
            EndAttack(shooter);

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


            atkInfo.SubCost(atkInfo.ShotCost);


            if (!PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
                photonView.RPC(nameof(MasterCreateProjectile), RpcTarget.MasterClient, gameObject.name
                                                             , autoDirection
                                                             , photonView.Controller);
            else
                StartCoroutine(SingleCreateProjectile(gameObject.name, autoDirection));
        }
    }
}