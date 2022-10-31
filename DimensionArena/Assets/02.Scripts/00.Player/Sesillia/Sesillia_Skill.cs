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

        protected override void Start()
        {
            base.Start();

            parabola = rangeComponent as Atk_Parabola;

            if (parabola == null)

                Destroy(this);
        }

        public override void UseSkill(Vector3 direction, float magnitude)
        {
            
        }

    }
}
