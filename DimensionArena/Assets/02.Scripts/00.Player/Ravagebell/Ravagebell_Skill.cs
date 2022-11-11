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
            // 애니메이션
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
        Vector3 location = direction;
        for (int i = 0; i < shotCnt; ++i)
        {
            yield return atkDelayWait;
            ShotDown(location, skillPoint);
            skillPoint = Quaternion.AngleAxis(120, Vector3.up) * skillPoint;
            yield return attackIntervalWait;
        }
    }

    private void ShotUp()
    {
        // 여기서 부터 코드 수정
        Destroy(Instantiate(muzzlePrefab, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation), 0.5f);
        projectile = Instantiate(skillPrefab, this.transform.position + (Vector3.up * 2f), Quaternion.LookRotation(Vector3.up));
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    private void ShotDown(Vector3 location, Vector3 direction)
    {
        projectile = Instantiate(skillPrefab,
            shotPosition + location + (Vector3.up * MaxRange) + direction,
            skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    [PunRPC]
    private void AnimationTrigger()
    {
        animator.SetTrigger("attack");
    }

    [PunRPC]
    private void AnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    [PunRPC]
    private IEnumerator MasterCreateSkill(Vector3 shooterAttackDir)
    {
        WaitForSeconds atkDelayWait = new WaitForSeconds(atkDelay);
        WaitForSeconds attackIntervalWait = new WaitForSeconds(attackInterval);

        Vector3 skillPoint = new Vector3(0, 0, 1f);

        for (int i = 0; i < shotCnt; ++i)
        {
            // 애니메이션
            photonView.RPC("AnimationTrigger", RpcTarget.All);
            photonView.RPC("AnimationSpeed", RpcTarget.All, 0.2f / atkDelay);
            
            yield return atkDelayWait;
          
            ShotUp_Server();
            photonView.RPC("AnimationSpeed", RpcTarget.All, 1f);

            if (i == 0)
                shotPosition = transform.position;
            yield return attackIntervalWait;
        }

        yield return new WaitForSeconds(dropDelay);

        skillPoint = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * skillPoint;
        for (int i = 0; i < shotCnt; ++i)
        {
            yield return atkDelayWait;
            ShotDown_Server(shooterAttackDir, skillPoint);
            skillPoint = Quaternion.AngleAxis(120, Vector3.up) * skillPoint;
            yield return attackIntervalWait;
        }
    }

    private void ShotUp_Server()
    {
        // 여기서 부터 코드 수정
        Destroy(PhotonNetwork.Instantiate(muzzlePrefab.name, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation), 0.5f);
        projectile = PhotonNetwork.Instantiate(skillPrefab.name, this.transform.position + (Vector3.up * 2f), Quaternion.LookRotation(Vector3.up));
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    private void ShotDown_Server(Vector3 location, Vector3 direction)
    {
        projectile = PhotonNetwork.Instantiate(skillPrefab.name,
            shotPosition + location + (Vector3.up * MaxRange) + direction,
            Quaternion.LookRotation(Vector3.down));
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    public override void ActSkill(Vector3 attackdirection, float magnitude)
    {
        //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
        if (!PhotonNetwork.OfflineMode)
        {
            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                      (skillDirection.normalized * skillDirection.magnitude * MaxRange));
        }
        else
        {
            StartCoroutine(SingleCreateSkill((skillDirection.normalized * skillDirection.magnitude * MaxRange)));
        }
    }

    public override void AutoSkill()
    {
        if (!PhotonNetwork.OfflineMode)
        {

            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                      autoSkill.targetPos - transform.position);
        }
        else
        {
            StartCoroutine(SingleCreateSkill(autoSkill.targetPos - transform.position));
        }
    }
}
