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
        }

        private void LateUpdate()
        {
            CheckFirstStep();
        }

        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            if (isCanFirstStep && targetDetect.IsTargetDetect)
            {
                GameObject particle = Instantiate(skillParticle, transform.position, Quaternion.identity);
                Destroy(particle, 0.6f);
                firstStepPos.y = 0;
                transform.position = firstStepPos;
                particle = Instantiate(skillParticle, transform.position, Quaternion.identity);
                Destroy(particle, 0.6f);
                Vector3 tempPos = targetDetect.Target.position;
                tempPos.y = 0;
                transform.LookAt(tempPos, Vector3.up);
            }
        }

        private void CheckFirstStep()
        {
            isCanFirstStep = false;

            int layerMask = (1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("GroundObject_Brick"));
            RaycastHit[] hitInfo;

            //Find
            if (!targetDetect.Target)
                return;

            Transform target = targetDetect.Target;

            for (int i = 0; i < 8; ++i)
            {
                switch (i)
                {
                    case 0:
                        firstStepPos = target.position + target.forward;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 1:
                        firstStepPos = target.position + (target.forward + target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 2:
                        firstStepPos = target.position + (target.forward + -target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 3:
                        firstStepPos = target.position + target.right;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 4:
                        firstStepPos = target.position - target.right;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 5:
                        firstStepPos = target.position + (-target.forward + target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 6:
                        firstStepPos = target.position + (-target.forward + -target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    case 7:
                        firstStepPos = target.position + -target.forward;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                    default:
                        firstStepPos = target.position + target.forward;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, 0, layerMask);
                        break;
                }
                //오브젝트가 해당 공간에 있을 경우,
                if (hitInfo.Length < 1)
                {                   
                    isCanFirstStep = true;
                    return;                  
                }
            }

            isCanFirstStep = false;
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(firstStepPos, 0.5f);
        }

        public override void AutoSkill()
        {
            // 자동 공격
        }
    }
}


