using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Sesillia_Skill : Player_Skill
    {
        [SerializeField] private GameObject skillPrefab;
        private Atk_Parabola parabola;
        private Parabola_Projectile projectile;

        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
        }

        protected override void Start()
        {
            base.Start();

            parabola = rangeComponent as Atk_Parabola;

            if (parabola == null)

                Destroy(this);
        }
    }
}
