using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;

namespace PlayerSpace
{
    public class Secilia_Skill : Player_Skill
    {
        [SerializeField] private DetectArea targetDetect;
        [SerializeField] private GameObject skillParticle;
        private Atk_FixedCircle circle;
        Vector3 firstStepPos;
        bool isCanFirstStep;

        protected override void Start()
        {
            base.Start();

            circle = rangeComponent as Atk_FixedCircle;

            if (circle == null)
                Destroy(this);

            circle.transform.localScale = new Vector3(MaxRange * 0.5f, MaxRange * 0.5f, 1);
            targetDetect.GetComponent<SphereCollider>().radius = MaxRange * 0.5f;

            targetDetect.SetRadius(MaxRange);
        }

        private void LateUpdate() => CheckFirstStep();

        //Quaternion.LookRotation(direction)
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            if (!isCanFirstStep || !targetDetect.IsTargetDetect)
                return;

            photonView.RPC(nameof(ActSkillforAllclient), RpcTarget.All, firstStepPos, this.transform.position);
        }

        [PunRPC]
        private void ActSkillforAllclient(Vector3 targetPos, Vector3 originPos)
        {
           
            GameObject particle = Instantiate(skillParticle, originPos, Quaternion.identity);
            particle.transform.LookAt(targetPos, Vector3.up);
            Destroy(particle, 0.6f);

            targetPos.y = 0;

            //FirstStep
            transform.position = targetPos;
            transform.LookAt(targetPos, Vector3.up);

        }
    
        private void CheckFirstStep()
        {
            //Find
            if (!targetDetect.Target)
                return;

            RaycastHit[] hitInfo;

            for (int i = 0; i < 8; ++i)
            {
                firstStepPos = targetDetect.TargetPos[i];
                hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0f, targetDetect.CollisionLayer);
                if (hitInfo.Length < 1)
                {
                    isCanFirstStep = true;
                    return;
                }
            }

            isCanFirstStep = false;
        }

        public override void AutoSkill()
        {
            // 자동 공격
        }
    }
}


