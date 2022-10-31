using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
using PlayerSpace;

public class Ravagebell_Skill : Player_Skill
{
    [SerializeField] private GameObject skillPrefab;
    private Atk_Circle circle;
    private GameObject projectile;

    protected override void Start()
    {
        base.Start();

        circle = rangeComponent as Atk_Circle;

        if (circle == null)
            Destroy(this);
    }



    public override void UseSkill(Vector3 direction, float magnitude)
    {
        //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
        if (!PhotonNetwork.OfflineMode)
        {

            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient,
                                                      direction);
        }
        else
        {
            projectile = Instantiate(skillPrefab, transform.position, skillPrefab.transform.rotation);

            // 여기서 부터 코드 수정
            projectile.GetComponent<Projectile>().AttackToDirection(Vector3.up, MaxRange, 5f);
        }

    }

    [PunRPC]
    private void MasterCreateSkill(Vector3 direction)
    {
        //GameObject tempSkill = PhotonNetwork.Instantiate("projectile_ravagebell", transform.position, rotation);
    }
}
