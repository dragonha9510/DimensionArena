using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
using PlayerSpace;

public class Securitas_Skill : Player_Skill
{
    [SerializeField] private GameObject skillPrefab;

    [SerializeField] private Transform avartarJump;
    [SerializeField] private float bombDistance;
    private Atk_Parabola parabola;
    private Parabola_Projectile projectile;
    private CapsuleCollider myCollider;

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

        myCollider = GetComponent<CapsuleCollider>();
    }

    [PunRPC]
    private IEnumerator MasterCreateSkill(Vector3 direction, Quaternion rotation, float dist, float velocity, float ypos)
    {
        owner.CanDirectionChange = false;

        /*/ 4 방향
        GameObject[] tempSkill = new GameObject[4];
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC(nameof(SkillTriger), RpcTarget.All);
            for(int i = 0; i < 4; ++i)
                tempSkill[i] = PhotonNetwork.Instantiate(skillPrefab.name, transform.position, rotation);
        }
        else
        {
            SkillTriger();
            for (int i = 0; i < 4; ++i)
                tempSkill[i] = Instantiate(skillPrefab, transform.position, rotation);
        }

        Vector3 bombDirection = Vector3.forward;

        for (int i = 0; i < 4; ++i)
        {
            projectile = tempSkill[i].GetComponent<Parabola_Projectile>();
            projectile.SetArcInfo(bombDirection, bombDistance, 0f, 1f);
            bombDirection = Quaternion.Euler(0, 90, 0) * bombDirection;
            projectile.ownerID = gameObject.name;
        }
        //*/

        //*/ 제자리
        GameObject tempSkill;
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC(nameof(SkillTriger), RpcTarget.All);
            tempSkill = PhotonNetwork.Instantiate(skillPrefab.name, transform.position, rotation);
        }
        else
        {
            SkillTriger();
            tempSkill = Instantiate(skillPrefab, transform.position, rotation);
        }

        projectile = tempSkill.GetComponent<Parabola_Projectile>();
        projectile.SetArcInfo(Vector3.back, 0.01f, 0f, 1f);
        projectile.ownerID = gameObject.name;
        //*/
        float t = 0;

        myCollider.enabled = false;
        direction.Normalize();
        while (true)
        {
            Vector3 tempPos = parabola.GetArcPosition(t);

            transform.position += new Vector3(direction.x, 0, direction.z) * dist * Time.deltaTime;
            avartarJump.position = new Vector3(avartarJump.position.x, tempPos.y, avartarJump.position.z);
            t += Time.deltaTime;

            yield return null;

            if (t >= 0.5f && tempPos.y < 1)
                myCollider.enabled = true;

            if (t >= 1)
            {
                transform.position += new Vector3(direction.x, 0, direction.z) * dist * (1 - t);
                t = 1;
                tempPos = parabola.GetArcPosition(t);
                avartarJump.position = new Vector3(avartarJump.position.x, tempPos.y, avartarJump.position.z);
                break;
            }
        }

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
        owner.CanDirectionChange = false;

        if (!PhotonNetwork.OfflineMode)
        {
            photonView.RPC(nameof(skillServer), RpcTarget.MasterClient,
                                                      attackdirection,
                                                      magnitude);
        }
        else
        {
            skillServer(attackdirection, magnitude);
        }
    }

    [PunRPC]
    private void skillServer(Vector3 attackdirection, float magnitude)
    {
        StartCoroutine(MasterCreateSkill(attackdirection, parabolaRotation, parabolaDistance, parabolaVelocity, parabolaMaxYPos));
    }

    public override void AutoSkill()
    {
        // 자동 공격
        skillDirection = transform.position + Vector3.forward * 0.001f;
        skillDirection.y = 0;
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