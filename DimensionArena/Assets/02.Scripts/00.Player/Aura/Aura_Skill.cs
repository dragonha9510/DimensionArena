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

        protected override void Start()
        {
            base.Start();
        }
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            animator.SetBool("SkillUse", true);
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
                Debug.Log("스킬 생성");
                GameObject tempSkill = Instantiate(skillPrefab, transform.position, FOV.transform.rotation);
                tempSkill.GetComponent<AuraSkillProjectile>().StartAttack(projectileSpeed, projectileRange);
            }
            animator.SetBool("SkillUse", false);

        }

        public override void AutoSkill()
        {
            throw new System.NotImplementedException();
        }
    }
}
