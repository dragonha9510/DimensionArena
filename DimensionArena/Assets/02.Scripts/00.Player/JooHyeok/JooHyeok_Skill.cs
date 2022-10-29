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

    protected override void Start()
    {
        base.Start();

        parabola = rangeComponent as Atk_Parabola;

        if (parabola == null)
            Destroy(this);
    }


    
    public override void UseSkill(Vector3 direction, float magnitude)
    {
        //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
        if(!PhotonNetwork.OfflineMode)
        {

            photonView.RPC(nameof(MasterCreateSkill), RpcTarget.MasterClient, 
                                                      direction, 
                                                      parabola.transform.rotation,
                                                      parabola.distance,
                                                      parabola.velocity,
                                                      parabola.maxYpos);
        }
        else
        {
            GameObject tempSkill = Instantiate(skillPrefab, transform.position, parabola.transform.rotation);
            projectile = tempSkill.GetComponent<Parabola_Projectile>();
            projectile.SetArcInfo(direction, parabola.distance, parabola.velocity, parabola.maxYpos);
        }

    }

    [PunRPC]
    private void MasterCreateSkill(Vector3 direction, Quaternion rotation, float dist, float velocity, float ypos)
    {
        GameObject tempSkill = PhotonNetwork.Instantiate("grenade", transform.position, rotation);
        projectile = tempSkill.GetComponent<Parabola_Projectile>();
        projectile.SetArcInfo(direction, dist, velocity, ypos);
    }
}
