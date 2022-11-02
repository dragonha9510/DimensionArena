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

        [SerializeField] private bool isRotation = true;

        protected Player owner;
        public Player Owner => owner;

        [SerializeField]  private GameObject skillRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 skillDirection;

        private float rotationSpeed = 1080.0f;

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
            StartCoroutine(LookAttackDirection(attackdirection, magnitude));
        }

        public abstract void ActSkill(Vector3 attackdirection, float magnitude);

        IEnumerator LookAttackDirection(Vector3 attackDirection, float magnitude)
        {
            if (isRotation)
            {
                Vector3 forward = Vector3.Slerp(transform.forward,
                    attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

                while (Vector3.Angle(attackDirection, transform.forward) >= 5)
                {
                    yield return null;
                    forward = Vector3.Slerp(transform.forward,
                    attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, attackDirection));

                    transform.LookAt(transform.position + forward);
                }
            }
            // 방향이 다 돌아가고 나서 공격 실행
            ActSkill(attackDirection, magnitude);
        }
    }
}