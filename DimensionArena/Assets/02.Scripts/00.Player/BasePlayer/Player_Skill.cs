using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    public abstract class Player_Skill : MonoBehaviourPun
    {
        protected Player owner;
        public Player Owner => owner;

        [SerializeField]  private GameObject skillRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 skillDirection;
    

        private Attack_Type type;
        public Attack_Type Type => type;
        private Atk_Range rangeComponent;

        //player skill info region
        float damage;
        float velocity;
        float maxRange = 5.0f;
        public float MaxRange => maxRange;
        protected virtual void Start()
        {
            //юс╫ц
            maxRange = 5.0f;

            if (skillRangeMesh == null)
            {
                GameObject temp = Instantiate(skillRangeMesh, transform);
                rangeComponent = temp.GetComponent<Atk_Range>();
                type = Player_Atk.SetType(rangeComponent);
            }
            else
            {
                rangeComponent = skillRangeMesh.GetComponent<Atk_Range>();
                type = Player_Atk.SetType(rangeComponent);
            }

            if (!owner)
                owner = GetComponent<Player>();
        }

        protected virtual void LateUpdate()
        {
            if (owner.Skill.direction.AlmostEquals(Vector3.zero, float.Epsilon))
                return;

            float distance = maxRange;
            rangeComponent.Calculate_Range(distance, direction);
        }    
        

        public void OnSkillMesh()
        {
            skillRangeMesh.gameObject.SetActive(true);
        }

        public void OffSkillMesh()
        {
            skillRangeMesh.gameObject.SetActive(false);
        }

        public abstract void UseSkill(Vector3 direction, float magnitude);


        //protected abstract void InitializeSkillInfo(float damage, float velocity);
    
        
    }
}