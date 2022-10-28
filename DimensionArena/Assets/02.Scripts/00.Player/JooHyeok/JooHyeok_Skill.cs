using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
        //PhotonNetwork.Instantiate("grenade", transform.position, parabola.transform.rotation);
        GameObject tempSkill = Instantiate(skillPrefab, transform.position, parabola.transform.rotation);
        projectile = tempSkill.GetComponent<Parabola_Projectile>();
        projectile.SetArcInfo(direction, parabola.distance, parabola.velocity, parabola.radianAngle);
    }
}
