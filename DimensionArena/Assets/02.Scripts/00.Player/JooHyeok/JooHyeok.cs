using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace PlayerSpace
{
    public class JooHyeok : Player
    {
        protected override void Awake()
        {
            base.Awake();

            //Player State ���
            info = new PlayerInfo(NickName,
                                  UNITTYPE.Joohyeok,
                                  100.0f, 100.0f, 2.3f);
            //Attack ���
            attack = GetComponent<JooHyeok_Atk>();
            if (!attack)
                attack = gameObject.AddComponent<JooHyeok_Atk>();
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}



