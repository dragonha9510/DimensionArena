using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
using PlayerSpace;

public class JooHyeok_Skill : Player_Skill
{
    [SerializeField] private GameObject skillPrefab;
    private Atk_Parabola parabola;
    private Parabola_Projectile projectile;

    /// 
    private Quaternion parabolaRotation;
    private float parabolaDistance;
    private float parabolaVelocity;
    private float parabolaMaxYPos;

    protected override void Start()
    {
        base.Start();

        parabola = rangeComponent as Atk_Parabola;

        if (parabola == null)
            Destroy(this);
    }

    [PunRPC]
    private IEnumerator MasterCreateSkill(Vector3 direction, Quaternion rotation, float dist, float velocity, float ypos)
    {
        owner.CanDirectionChange = false;

        GameObject tempSkill;
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC(nameof(SkillTriger), RpcTarget.All);
            tempSkill = PhotonNetwork.Instantiate("grenade", transform.position, rotation);
        }
        else
        {
            SkillTriger();
            tempSkill = Instantiate(skillPrefab, transform.position, rotation);
        }

        yield return new WaitForSeconds(0.25f);

        projectile = tempSkill.GetComponent<Parabola_Projectile>();
        projectile.SetArcInfo(direction, dist, velocity, ypos);
        projectile.ownerID = gameObject.name;

        yield return new WaitForSeconds(0.25f);

        owner.CanDirectionChange = true;
    }

    [PunRPC]
    private void SkillTriger()
    {
        animator.SetTrigger("skill");
    }

    public override void SetSkillInfo()
    {
        parabolaRotation = parabola.transform.rotation;
        parabolaDistance = parabola.distance;
        parabolaVelocity = parabola.velocity;
        parabolaMaxYPos = parabola.maxYpos;
    }

    public override void ActSkill(Vector3 attackdirection, float magnitude)
    {        
        //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                      attackdirection,
                                                      parabolaRotation,
                                                      parabolaDistance,
                                                      parabolaVelocity,
                                                      parabolaMaxYPos);
        }
        else
        {
            StartCoroutine(MasterCreateSkill( attackdirection,parabolaRotation,parabolaDistance,parabolaVelocity,parabolaMaxYPos));

            //animator.SetTrigger("skill");
            //GameObject tempSkill = Instantiate(skillPrefab, transform.position, parabolaRotation);
            //projectile = tempSkill.GetComponent<Parabola_Projectile>();
            //projectile.SetArcInfo(attackdirection, parabolaDistance, parabolaVelocity, parabolaMaxYPos);
            //projectile.ownerID = gameObject.name;
        }
    }

    public override void AutoSkill()
    {
        // 자동 공격
        skillDirection = autoSkill.targetPos - transform.position;
        float magnitude = skillDirection.magnitude;

        skillDirection.Normalize();

        parabola.Calculate_Range(magnitude, skillDirection);

        if (isRotation)
            StartCoroutine(LookAutoAttackDirection(skillDirection, magnitude));
        else
            ActSkill(skillDirection, magnitude);
    }

    protected IEnumerator LookAutoAttackDirection(Vector3 attackDirection, float magnitude)
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
