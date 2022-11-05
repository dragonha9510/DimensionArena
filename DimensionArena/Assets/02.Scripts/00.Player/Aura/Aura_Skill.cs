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
        private AuraSkillDetector ditector;

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
            Debug.Log("애니메이션 돌리기");
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
                Quaternion skillRot = FOV.transform.rotation;
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < 3; ++i)
                {
                    GameObject tempSkill1 = Instantiate(skillPrefab, transform.position + transform.forward * 0.2f, skillRot);
                    tempSkill1.GetComponent<AuraSkillProjectile>().StartAttack(projectileSpeed, projectileRange);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                }
                List<GameObject> objList = ditector.WillTakeDamageObj;

                foreach(GameObject obj in objList)
                {
                    if(obj.tag == "Player")
                    {
                        Vector3 tmp = (obj.transform.position - this.transform.position).normalized;
                        obj.GetComponent<Rigidbody>().velocity = tmp * 1000.0f;
                    }
                }

            }
        }

        public override void AutoSkill()
        {
            return;
        }


    }
}
