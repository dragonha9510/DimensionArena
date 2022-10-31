using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    public class Sesillia : Player
    {
        protected override void Awake()
        {
            base.Awake();

            //Attack µî·Ï
            attack = GetComponent<Sesillia_Atk>();

            if (!attack)
                attack = gameObject.AddComponent<Sesillia_Atk>();
        }


        protected override void Start()
        {
            base.Start();
        }
    }
}

