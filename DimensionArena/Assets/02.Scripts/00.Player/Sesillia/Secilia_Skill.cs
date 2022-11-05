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
        private Atk_FixedCircle cricle;
        private Parabola_Projectile projectile;
        Vector3 firstStepPos;
        bool isCanFirstStep;
        protected override void Start()
        {
            base.Start();

            cricle = rangeComponent as Atk_FixedCircle;

            if (cricle == null)
                Destroy(this);

        }

        private void LateUpdate()
        {
            CheckFirstStep();

        }

        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            if (isCanFirstStep)
            {
                firstStepPos.y = 0;
                transform.position = firstStepPos;
            }
        }

        private void CheckFirstStep()
        {
            int layerMask = 1 << LayerMask.NameToLayer("Player");
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
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 1:
                        firstStepPos = target.position + (target.forward + target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 2:
                        firstStepPos = target.position + (target.forward + -target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 3:
                        firstStepPos = target.position + target.right;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 4:
                        firstStepPos = target.position - target.right;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 5:
                        firstStepPos = target.position + (-target.forward + target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 6:
                        firstStepPos = target.position + (-target.forward + -target.right).normalized;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    case 7:
                        firstStepPos = target.position + -target.forward;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                    default:
                        firstStepPos = target.position + target.forward;
                        hitInfo = Physics.SphereCastAll(firstStepPos, 0.49f, Vector3.up, layerMask);
                        break;
                }
                Debug.Log(hitInfo.Length);
                //오브젝트가 해당 공간에 있을 경우,
                if (hitInfo.Length < 2)
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


