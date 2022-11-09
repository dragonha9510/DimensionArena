using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Aura_Skill : Player_Skill
    {
        [SerializeField] private GameObject skillPrefab;
        [SerializeField]
        private float projectileSpeed = 10.0f;
        [SerializeField]
        private float projectileRange= 10.0f;
        [SerializeField]
        private FieldOfView FOV;
        [SerializeField]
        private Animator auraAnimator;
        [SerializeField]
        private int rayCount = 3;
        [SerializeField]
        private float skillDamage = 10.0f;
        [SerializeField]
        private float skillAnimationSpeed = 2f;

        private List<GameObject> hitedObj = new List<GameObject>();

        protected override void Start()
        {
            base.Start();
            auraAnimator.SetFloat("SkillSpeed", skillAnimationSpeed);

        }
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            projectileRange = FOV.ViewRadius;
            auraAnimator.SetBool("SkillUse", true);
            SetMovePrevSkill();
        }

        private void SetMovePrevSkill()
        {
            owner.Info.SpeedDown(0.9f);
            owner.CanDirectionChange = false;
        }
        private void SetMoveAfterSkill()
        {
            Debug.Log("애니메이션 돌리기");
            auraAnimator.SetBool("SkillUse", false);
            owner.Info.SpeedUp(0.9f);
            owner.CanDirectionChange = true;
        }

        [PunRPC]
        private void CreateSkillProjectile(string prefabName,Vector3 trans,Quaternion rot)
        {
            GameObject skill = PhotonNetwork.Instantiate(prefabName, trans, rot);
            skill.GetComponent<Projectile>().AttackToDirection(projectileRange, projectileSpeed);
        }

        private void MakeSkillProjectile()
        {
            if (PhotonNetwork.InRoom)
            {
                Quaternion skillRot = FOV.transform.rotation;
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < 3; ++i)
                {
                    photonView.RPC(nameof(CreateSkillProjectile), RpcTarget.MasterClient, skillPrefab.name, transform.position + transform.forward * 0.2f, skillRot);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                }

                if (!PhotonNetwork.IsMasterClient)
                    return;
                Ray ray = new Ray();
                RaycastHit rayHit;
                ray.origin = FOV.transform.position;

                ray.direction = FOV.transform.forward;
                ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;

                float correctionAngle = FOV.viewAngle / (rayCount - 1);


                for (int i = 0; i < rayCount; ++i)
                {
                    if (true == Physics.Raycast(ray, out rayHit, FOV.ViewRadius) && rayHit.transform.gameObject != owner && false == hitedObj.Contains(rayHit.transform.gameObject))
                    {
                        hitedObj.Add(rayHit.transform.gameObject);
                        Debug.Log("부딫힘");
                        //rayHit.transform.gameObject.transform.position = owner.transform.position + ((rayHit.transform.gameObject.transform.position - owner.transform.position).normalized * FOV.ViewRadius);
                        // 맞는 애가 넉백이 없을 시에 ( 플레이어가 아닐 때 ) 는 넉백 작동 안함.
                        if(null != rayHit.transform.gameObject.GetComponent<isKnockBack>())
                        {
                            rayHit.transform.gameObject.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position, (rayHit.transform.gameObject.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                            PlayerInfoManager.Instance.CurHpDecrease(rayHit.transform.gameObject.name, skillDamage);
                        }
                    }
                    ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
                }
                hitedObj.Clear();
            }
            else
            {
                // Projectile 생성
                Quaternion skillRot = FOV.transform.rotation;
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < 3; ++i)
                {
                    GameObject tempSkill1 = Instantiate(skillPrefab, transform.position + transform.forward * 0.2f, skillRot);
                    tempSkill1.GetComponent<Projectile>().AttackToDirection(projectileRange, projectileSpeed);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                }

                Ray ray = new Ray();
                RaycastHit rayHit;
                ray.origin = FOV.transform.position;

                ray.direction = FOV.transform.forward;
                ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;

                float correctionAngle = FOV.viewAngle / (rayCount - 1);


                for (int i = 0; i < rayCount; ++i)
                {
                    if (true == Physics.Raycast(ray, out rayHit,FOV.ViewRadius) && rayHit.transform.gameObject != owner && false == hitedObj.Contains(rayHit.transform.gameObject))
                    {
                        hitedObj.Add(rayHit.transform.gameObject);
                        Debug.Log("부딫힘");
                        //rayHit.transform.gameObject.transform.position = owner.transform.position + ((rayHit.transform.gameObject.transform.position - owner.transform.position).normalized * FOV.ViewRadius);
                        
                        rayHit.transform.gameObject.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position,(rayHit.transform.gameObject.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                        rayHit.transform.gameObject.GetComponent<PlayerInfo>().Damaged(skillDamage);
                    }
                    ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
                }
                hitedObj.Clear();
            }
        }

        private void OnDrawGizmos()
        {
            Ray ray = new Ray();
            ray.origin = FOV.transform.position;

            ray.direction = FOV.transform.forward;
            ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;

            float correctionAngle = FOV.viewAngle / (rayCount - 1);

            for (int i = 0; i < rayCount; ++i)
            {
                Gizmos.DrawRay(ray);
                /*if (true == Physics.Raycast(ray, out rayHit))
                {
                    Debug.Log("부딫힘");
                }*/
                ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
            }
        }

        public override void AutoSkill()
        {
            return;
        }


    }
}
