using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Player_Atk : MonoBehaviourPun
{
    protected Player owner;

    [SerializeField]  protected float range;
    [SerializeField]  private GameObject atkRangeMesh;

    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 attackDirection;

    private float rotationSpeed = 1080.0f;
    private RaycastHit atkRangeRay;

    protected bool isAttack;
    public bool IsAttack { get { return isAttack; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (atkRangeMesh == null)
            Instantiate(atkRangeMesh, transform);

        if (photonView.IsMine)
        {
            GameObject.Find("AtkJoyStick").GetComponent<AtkJoyStick>().player = this;
            owner = gameObject.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        Vector3 forLength = Vector3.zero;
        float distance = range;
        Vector3 position = transform.position + new Vector3(0, 0.5f, 0) + direction.normalized;

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, range))
        {
            forLength = atkRangeRay.point - position;
            distance = forLength.magnitude;
        }

        atkRangeMesh.transform.localScale = new Vector3(0.5f, 1, distance);
        atkRangeMesh.transform.position = transform.position + direction.normalized * ((distance * 0.5f) + 1f) + new Vector3(0, 0.001f, 0);
        atkRangeMesh.transform.forward = (transform.position - atkRangeMesh.transform.position).normalized;
    }

    public void StartAttack()
    {
        attackDirection = direction;
        attackDirection.Normalize();

        StartCoroutine(LookAttackDirection());
        //attack
        Attack();
    }



    public abstract void Attack();
    public abstract void Skill();

    IEnumerator LookAttackDirection()
    {
        Vector3 forward = Vector3.Slerp(transform.forward,
            attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

        while (!forward.Equals(attackDirection))
        {
            yield return null;
            forward = Vector3.Slerp(transform.forward,
            attackDirection, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, attackDirection));

            transform.LookAt(transform.position + forward);
        }
    }
}
