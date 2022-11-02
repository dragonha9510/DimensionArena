using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
using PlayerSpace;

public class Ravagebell_Skill : Player_Skill
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private float projectileSpeed = 8f;

    [SerializeField] private float shotCnt = 3;
    [SerializeField] private float dropDelay = 1f;
    [SerializeField] private float attackInterval = 0.3f;
    [SerializeField] private float atkDelay = 0.2f;

    private Vector3 shotPosition;

    private Atk_Circle circle;
    private GameObject projectile;

    protected override void Start()
    {
        base.Start();

        circle = rangeComponent as Atk_Circle;

        if (circle == null)
            Destroy(this);
    }

    private IEnumerator SingleCreateSkill(Vector3 direction)
    {
        WaitForSeconds atkDelayWait = new WaitForSeconds(atkDelay);
        WaitForSeconds attackIntervalWait = new WaitForSeconds(attackInterval);

        Vector3 skillPoint = new Vector3(0, 0, 1f);

        for (int i = 0; i < shotCnt; ++i)
        {
            // �ִϸ��̼�
            animator.SetTrigger("attack");
            animator.speed = 0.2f / atkDelay;

            yield return atkDelayWait;

            ShotUp();
            animator.speed = 1;

            if(i == 0)
                shotPosition = transform.position;
            yield return attackIntervalWait;
        }

        yield return new WaitForSeconds(dropDelay);

        skillPoint = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * skillPoint;
        for (int i = 0; i < shotCnt; ++i)
        {
            yield return atkDelayWait;
            ShotDown(skillPoint);
            skillPoint = Quaternion.AngleAxis(120, Vector3.up) * skillPoint;
            yield return attackIntervalWait;
        }
    }

    private void ShotUp()
    {
        // ���⼭ ���� �ڵ� ����
        Destroy(Instantiate(muzzlePrefab, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation), 0.5f);
        projectile = Instantiate(skillPrefab, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.up, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    private void ShotDown(Vector3 direction)
    {
        projectile = Instantiate(skillPrefab,
            shotPosition + (skillDirection.normalized * skillDirection.magnitude * MaxRange) + (Vector3.up * MaxRange) + direction,
            skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.down, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    [PunRPC]
    private IEnumerator MasterCreateSkill(Vector3 shooterAttackDir)
    {
        WaitForSeconds atkDelayWait = new WaitForSeconds(atkDelay);
        WaitForSeconds attackIntervalWait = new WaitForSeconds(attackInterval);

        Vector3 skillPoint = new Vector3(0, 0, 1f);

        for (int i = 0; i < shotCnt; ++i)
        {
            // �ִϸ��̼�
            animator.SetTrigger("attack");
            animator.speed = 0.2f / atkDelay;
            
            yield return atkDelayWait;
          
            ShotUp_Server();
            animator.speed = 1;

            if (i == 0)
                shotPosition = transform.position;
            yield return attackIntervalWait;
        }

        yield return new WaitForSeconds(dropDelay);

        skillPoint = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * skillPoint;
        for (int i = 0; i < shotCnt; ++i)
        {
            yield return atkDelayWait;
            ShotDown_Server(skillPoint);
            skillPoint = Quaternion.AngleAxis(120, Vector3.up) * skillPoint;
            yield return attackIntervalWait;
        }
    }

    private void ShotUp_Server()
    {
        // ���⼭ ���� �ڵ� ����
        Destroy(PhotonNetwork.Instantiate(muzzlePrefab.name, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation), 0.5f);
        projectile = PhotonNetwork.Instantiate(skillPrefab.name, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.up, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    private void ShotDown_Server(Vector3 direction)
    {
        projectile = PhotonNetwork.Instantiate(skillPrefab.name,
            shotPosition + (skillDirection.normalized * skillDirection.magnitude * MaxRange) + (Vector3.up * MaxRange) + direction,
            skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.down, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    public override void ActSkill(Vector3 attackdirection, float magnitude)
    {
        //Parabola rotation, distance velocity radianAngle�� ����ȭ���� �ʴ´�. �̸� �������ְ� ������ �Ķ���͸� ���� �� ����ұ�?
        if (!PhotonNetwork.OfflineMode)
        {

            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                      direction);
        }
        else
        {
            StartCoroutine(SingleCreateSkill(direction));
        }
    }
}
