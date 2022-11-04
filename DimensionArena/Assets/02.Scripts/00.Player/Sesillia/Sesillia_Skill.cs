using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Sesillia_Skill : Player_Skill
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
                if(CheckFirstStepForTaget(target.Value))
                    break;                    
            }

        }

        bool CheckFirstStepForTaget(Vector3 position)
        {
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


