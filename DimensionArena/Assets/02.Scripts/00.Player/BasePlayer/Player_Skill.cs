using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    public abstract class Player_Skill : MonoBehaviourPun
    {
        // Animator
        protected Animator animator;

        [SerializeField] protected bool isRotation = true;

        [SerializeField] protected AutoAtk_Detection autoSkill;

        protected Player owner;
        public Player Owner => owner;

        [SerializeField]  private GameObject skillRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 skillDirection;

        protected float rotationSpeed = 1080.0f;

        protected Attack_Type type;
        public Attack_Type Type => type;
        protected Atk_Range rangeComponent;

        //player skill info region
        float damage;
        float velocity;
        [SerializeField] private float maxRange = 5.0f;
        public float MaxRange => maxRange;
        protected virtual void Start()
        {
            animator = GetComponentInChildren<Animator>();

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

        public virtual void SetSkillInfo() { }

        public void OnSkillMesh()
        {
            if (owner.Skill.direction.AlmostEquals(Vector3.zero, float.Epsilon))
                return;

            float distance = maxRange;
            rangeComponent.Calculate_Range(distance, direction);

            skillRangeMesh.gameObject.SetActive(true);
        }

        public void OffSkillMesh()
        {
            skillRangeMesh.gameObject.SetActive(false);
        }

        public void UseSkill(Vector3 attackdirection, float magnitude)
        {
            skillDirection = attackdirection;

            if (isRotation)
                StartCoroutine(LookAttackDirection(attackdirection, magnitude));
            else
                ActSkill(attackdirection, magnitude);
        }

        public abstract void ActSkill(Vector3 attackdirection, float magnitude);
        public abstract void AutoSkill();
        protected IEnumerator LookAttackDirection(Vector3 attackDirection, float magnitude)
        {
            SetSkillInfo();
            Vector3 forward = Vector3.Slerp(transform.forward,
                attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));
            
            while (Vector3.Angle(attackDirection, transform.forward) >= 5)
            {
                yield return null;
                forward = Vector3.Slerp(transform.forward,
                attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, attackDirection));
            
                transform.LookAt(transform.position + forward);
            }
         
            ActSkill(attackDirection, magnitude);
        }
    }
}