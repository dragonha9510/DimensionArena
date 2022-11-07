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
        private Animator animator;
        [SerializeField]
        private int rayCount = 3;
        [SerializeField]
        protected override void Start()
        {
            base.Start();
            
        }
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            projectileRange = FOV.ViewRadius;
            animator.SetBool("SkillUse", true);
            SetMovePrevSkill();
        }

        private void SetMovePrevSkill()
        {
            owner.Info.SpeedDown(0.9f);
            owner.CanDirectionChange = false;
        }
        private void SetMoveAfterSkill()
        {
            Debug.Log("局聪皋捞记 倒府扁");
            animator.SetBool("SkillUse", false);
            owner.Info.SpeedUp(10f);
            owner.CanDirectionChange = true;
        }
        private void MakeSkillProjectile()
        {
            if (PhotonNetwork.InRoom)
            {
                /*photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                          direction,
                                                          parabola.transform.rotation,
                                                          parabola.distance,
                                                          parabola.velocity,
                                                          parabola.maxYpos);*/
            }
            else
            {
                // Projectile 积己
                Quaternion skillRot = FOV.transform.rotation;
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < 3; ++i)
                {
                    GameObject tempSkill1 = Instantiate(skillPrefab, transform.position + transform.forward * 0.2f, skillRot);
                    tempSkill1.GetComponent<AuraSkillProjectile>().StartAttack(projectileSpeed, projectileRange);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                }

                Ray ray = new Ray();
                RaycastHit rayHit;
                ray.origin = FOV.transform.position;

                ray.direction = FOV.transform.forward;
                ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;

                float correctionAngle = FOV.viewAngle / (rayCount - 1);

                List<GameObject> hitedObj = new List<GameObject>();

                for (int i = 0; i < rayCount; ++i)
                {
                    if (true == Physics.Raycast(ray, out rayHit,FOV.ViewRadius) && rayHit.transform.gameObject != owner && false == hitedObj.Contains(rayHit.transform.gameObject))
                    {
                        hitedObj.Add(rayHit.transform.gameObject);
                        Debug.Log("何婬塞");
                        //rayHit.transform.gameObject.transform.position = owner.transform.position + ((rayHit.transform.gameObject.transform.position - owner.transform.position).normalized * FOV.ViewRadius);
                        
                        rayHit.transform.gameObject.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position,(rayHit.transform.gameObject.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                    }
                    ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
                }
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
                    Debug.Log("何婬塞");
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
