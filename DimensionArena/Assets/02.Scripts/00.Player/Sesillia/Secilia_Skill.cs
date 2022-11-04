using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Secilia_Skill : Player_Skill
    {
        [SerializeField] private SphereCollider SpawnDetector;
        private Atk_FixedCircle cricle;
        private Parabola_Projectile projectile;


        protected override void Start()
        {
            base.Start();
           
            cricle = rangeComponent as Atk_FixedCircle;

            if (cricle == null)
                Destroy(this);
            
        }

        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            FirstStep(magnitude);
        }

        private void FirstStep(float magnitude)
        {

            int layerMask = 1 << LayerMask.NameToLayer("Player");


            SortedList<float, Vector3> targetPos = new SortedList<float, Vector3>();
            RaycastHit[] hitInfo;
            hitInfo = Physics.SphereCastAll(transform.position, magnitude, Vector3.up, 0f, layerMask);

            for (int i = hitInfo.Length - 1; i >= 0; --i)
            {
                if (hitInfo[i].collider.name == gameObject.name)
                    continue;
                targetPos.Add(Vector3.Distance(hitInfo[i].transform.position, transform.position), hitInfo[i].transform.position + hitInfo[i].transform.forward);
            }

            
            foreach(var target in targetPos)
            {
                if(CheckFirstStepForTaget(target.Value, target.Value))
                    break;                    
            }

        }

        bool CheckFirstStepForTaget(Vector3 position, Vector3 forward)
        {
            //만약, 상대방의 정면에 아무런 Obstacle이 존재하지 않는다면 정면 -> 아니라면 45도씩 돌으며 순보가 가능한 위치를 파악한다.
            int leftCheck;
            for(int i = 0; i < 8; ++i)
            {
                leftCheck = i % 2 == 0 ? 1 : -1;
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, MaxRange);          
        }

        public override void AutoSkill()
        {
            // 자동 공격
        }
    }
}


