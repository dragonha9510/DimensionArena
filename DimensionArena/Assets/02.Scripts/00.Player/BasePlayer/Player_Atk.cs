using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace PlayerSpace
{
    public abstract class Player_Atk : MonoBehaviourPun
    {
        public event Action<float> eChangeMagazineCost = (param) => { };
        public event Action eCantAttack = () => { };

        protected Player owner;
        public Player Owner => owner;

        [SerializeField] protected PlayerAtkInfo atkInfo;
        public PlayerAtkInfo AtkInfo => atkInfo;

        [Header("Programmer Region")]
        [SerializeField]  private GameObject  atkRangeMesh;
        [SerializeField]  private GameObject  skillRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 attackDirection;

        private float rotationSpeed = 1080.0f;
        private RaycastHit atkRangeRay;

        //Attack중인지 확인
        protected bool isAttack;
        public bool IsAttack { get { return isAttack; } }

        protected virtual void Start()
        {
            InitalizeAtkInfo();

            if (atkRangeMesh == null)
                Instantiate(atkRangeMesh, transform);

            if (owner)
            {
                owner = GetComponent<Player>();
                if (owner.photonView.IsMine)
                    StartCoroutine(MagazineReloadCoroutine());
            }
            else
            {
                owner = GetComponent<Player>();
                StartCoroutine(MagazineReloadCoroutine());
            }

        }

        protected abstract void InitalizeAtkInfo();
        protected virtual void LateUpdate()
        {
            float distance = atkInfo.Range;

            Vector3 position = transform.position + new Vector3(0, 0.5f, 0) + direction.normalized;

            if (Physics.Raycast(position, direction.normalized, out atkRangeRay, atkInfo.Range))
            {
                Vector3 forLength = Vector3.zero;
                forLength = atkRangeRay.point - position;
                distance = forLength.magnitude;
            }

            atkRangeMesh.transform.localScale = new Vector3(0.5f, 1, distance);

            atkRangeMesh.transform.position = transform.position +
                                              direction.normalized * ((distance * 0.5f) + 1f)
                                              + new Vector3(0, 0.001f, 0);
            atkRangeMesh.transform.forward =
                    (transform.position - atkRangeMesh.transform.position).normalized;
        }


        protected abstract void Attack();
        protected abstract void Skill();


        public void StartAttack()
        {
            attackDirection = direction;
            attackDirection.Normalize();
            StartCoroutine(LookAttackDirection());
            Attack();
        }

        protected void WaitAttack()
        {
            eCantAttack();
        }

        IEnumerator LookAttackDirection()
        {
            Vector3 forward = Vector3.Slerp(transform.forward,
                attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

            while (!forward.AlmostEquals(attackDirection, float.Epsilon))
            {
                yield return null;
                forward = Vector3.Slerp(transform.forward,
                attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, attackDirection));

                transform.LookAt(transform.position + forward);
            }
        }

        IEnumerator MagazineReloadCoroutine()
        {
            while (true)
            {
             
                atkInfo.AddCost(Time.deltaTime * atkInfo.InverseReloadTime);
                eChangeMagazineCost(atkInfo.CurCost);
                yield return null;
            }
        }
    }


}


