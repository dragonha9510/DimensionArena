using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Player_Atk : MonoBehaviourPun
{
    protected Player owner;
    public Player Owner => owner;

    [Header("PlayerAttackInfo")]
    [SerializeField] protected float range;
    [SerializeField] protected int maxMagazine;
    [SerializeField] private float reloadTime;
    protected float curCost;
    protected float shotCost;
    protected float reverseReloadTime;
    
    public int MaxMagazine => maxMagazine;

    public event Action<float> eChangeMagazineCost = (param) => { };
    public event Action eCantAttack = () => { };

    [Header("Programmer Region")]
    [SerializeField]  private GameObject atkRangeMesh;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 attackDirection;

    private float rotationSpeed = 1080.0f;
    private RaycastHit atkRangeRay;

    //Attack중인지 확인
    protected bool isAttack;
    public bool IsAttack { get { return isAttack; } }

    protected virtual void Start()
    {
        if (atkRangeMesh == null)
            Instantiate(atkRangeMesh, transform);

        if(owner)
        {
            owner = GetComponent<Player>();
            if (owner.photonView.IsMine)
            {
                SetMagaazineInverseVaraible();
                StartCoroutine(MagazineReloadCoroutine());
            }
        }
#if UNITY_EDITOR
            SetMagaazineInverseVaraible();
            StartCoroutine(MagazineReloadCoroutine());
#endif
    }
    protected virtual void LateUpdate()
    {
        float distance = range;

        Vector3 position = transform.position + new Vector3(0, 0.5f, 0) + direction.normalized;

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, range))
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

    protected void WaitAttack()
    {
        eCantAttack();
    }
    public abstract void Attack();
    public abstract void Skill();
    public void StartAttack()
    {
        attackDirection = direction;
        attackDirection.Normalize();
        StartCoroutine(LookAttackDirection());
        Attack();
    }

    IEnumerator LookAttackDirection()
    {
        Vector3 forward = Vector3.Slerp(transform.forward,
            attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

        while (!forward.AlmostEquals(attackDirection,float.Epsilon))
        {
            yield return null;
            forward = Vector3.Slerp(transform.forward,
            attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, attackDirection));

            transform.LookAt(transform.position + forward);
        }
    }

    void SetMagaazineInverseVaraible()
    {
        shotCost = (float)(1.0f / maxMagazine);
        reverseReloadTime = 1 / (reloadTime * maxMagazine);
    }



    IEnumerator MagazineReloadCoroutine()
    {      
        while(true)
        {         
            curCost += Time.deltaTime * reverseReloadTime;
            curCost = Mathf.Min(curCost, 1.0f);
            eChangeMagazineCost(curCost);
            yield return null;
        }
    }  
}


