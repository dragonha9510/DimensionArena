using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    public class Player_Skill : MonoBehaviourPun
    {
        protected Player owner;
        public Player Owner => owner;

        [Header("Programmer Region")]
        [SerializeField] private GameObject skillRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 skillDirection;
        [SerializeField] SkillJoyStick joystick;


        private Attack_Type type;
        public Attack_Type Type => type;
        private Atk_Range rangeComponent;

        //player skill info region
        float damage;
        float velocity;
        float maxRange = 10.0f;

        void Start()
        {
            maxRange = 10.0f;

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

            if (photonView.IsMine)
                joystick = GameObject.Find("SkillJoyStick").GetComponent<SkillJoyStick>();
        }

        protected virtual void LateUpdate()
        {
            float distance = maxRange;
            rangeComponent.Calculate_Range(distance, direction);
        }    //protected abstract void InitializeSkillInfo(float damage, float velocity);
    
        
    }
}