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

        for (int i = 0; i < shotCnt; ++i)
        {
            // 애니메이션
            animator.SetTrigger("attack");
            yield return atkDelayWait;
            ShotUp();
            yield return attackIntervalWait;
        }

        yield return new WaitForSeconds(dropDelay);

        for (int i = 0; i < shotCnt; ++i)
        {
            yield return atkDelayWait;
            ShotDown(direction);
            yield return attackIntervalWait;
        }
    }

    private void ShotUp()
    {
        // 여기서 부터 코드 수정
        Destroy(Instantiate(muzzlePrefab, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation), 0.5f);
        projectile = Instantiate(skillPrefab, this.transform.position + (Vector3.up * 2f), skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.up, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    private void ShotDown(Vector3 direction)
    {
        projectile = Instantiate(skillPrefab,
            this.transform.position + (skillDirection.normalized * skillDirection.magnitude * MaxRange) + (Vector3.up * MaxRange),
            skillPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().AttackToDirection(Vector3.down, MaxRange, projectileSpeed);
        projectile.GetComponent<Projectile>().ownerID = gameObject.name;
    }

    [PunRPC]
    private IEnumerator MasterCreateSkill(Vector3 shooterAttackDir)
    {
        return null;
    }

    public override void ActSkill(Vector3 attackdirection, float magnitude)
    {
        //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
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
