using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace PlayerSpace
{
    public enum Attack_Type
    {
        RECT,
        PARABOLA,
        CIRCLE,
        SECTORFORM,
        FIXEDCIRCLE,
        Attack_Type_End
    }

    public abstract class Player_Atk : MonoBehaviourPun
    {
        protected Animator animator;

        [SerializeField] protected bool isRotation = true;

        [SerializeField] protected AutoAtk_Detection autoAtk;

        public event Action<float> eChangeMagazineCost = (param) => { };
        public event Action eCantAttack = () => { };

        protected Player owner;
        public Player Owner => owner;

        [SerializeField] protected PlayerAtkInfo atkInfo;
        public PlayerAtkInfo AtkInfo => atkInfo;

        [Header("Programmer Region")]
        [SerializeField] private GameObject atkRangeMesh;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 attackDirection;

        protected float curdistance;
        private Atk_Range rangeComponent;

        private Attack_Type type;
        public  Attack_Type Type => type;

        protected float rotationSpeed = 1240.0f;

        protected bool isAttack;
        public bool IsAttack { get { return isAttack; } }

        protected abstract void InitalizeAtkInfo();
        protected virtual void Start()
        {
            animator = GetComponentInChildren<Animator>();

            InitalizeAtkInfo();

            if (atkRangeMesh == null)
            {
                GameObject temp = Instantiate(atkRangeMesh, transform);
                rangeComponent = temp.GetComponent<Atk_Range>();
                type = SetType(rangeComponent);
            }
            else
            {
                rangeComponent = atkRangeMesh.GetComponent<Atk_Range>();
                type = SetType(rangeComponent);
            }

            if (owner)
            {
                if (owner.photonView.IsMine)
                    StartCoroutine(MagazineReloadCoroutine());
            }
            else
            {
                owner = GetComponent<Player>();
                StartCoroutine(MagazineReloadCoroutine());
            }

            atkRangeMesh.SetActive(false);
        }

        public void Calculate()
        {
            float distance = atkInfo.Range;

            rangeComponent.Calculate_Range(distance, direction);
        }

        public virtual void AtkRangeMeshOnOff(bool temp)
        {
            atkRangeMesh.SetActive(temp);
        }

        [PunRPC]
        protected virtual void AtkTrigger()
        {
            animator.SetTrigger("attack");
            owner.Info.BattleOn();
        }

        public abstract void Attack();
        public abstract void AutoAttack();
       
        protected void WaitAttack()
        {
            eCantAttack();
        }

        public void StartAttack()
        {
            if (isAttack)
                return;
            if(false != owner.CanDirectionChange)
                attackDirection = direction;
            curdistance = direction.magnitude;
            attackDirection.Normalize();
            StartCoroutine(LookAttackDirection());  
        }

        IEnumerator LookAttackDirection()
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

            Attack();
        }

        IEnumerator MagazineReloadCoroutine()
        {
            while (true)
            {
                if(!atkInfo.CurCost.AlmostEquals(1.0f, float.Epsilon))
                {
                    atkInfo.AddCost(Time.deltaTime * atkInfo.InverseReloadTime);
                    eChangeMagazineCost(atkInfo.CurCost);
                }
                yield return null;   
            }
        }

        public static Attack_Type SetType(Atk_Range range)
        {
            if(range as Atk_Rect)
            {
                return Attack_Type.RECT;
            }
            else if(range as Atk_Parabola)
            {
                return Attack_Type.PARABOLA;
            }
            else if(range as Atk_Circle)
            {
                return Attack_Type.CIRCLE;
            }
            else if(range as Atk_FixedCircle)
            {
                return Attack_Type.FIXEDCIRCLE;
            }
            else if (range as FieldOfView)
            {
                return Attack_Type.SECTORFORM;
            }
            else
            {
                Debug.LogError("�߸��� Ÿ���� ������ �Ҵ��Ͽ����ϴ�.");
                return Attack_Type.Attack_Type_End;
            }
        }

    }
}


