using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    public class Securitas : Player
    {
        protected override void Awake()
        {
            base.Awake();

            ////Player State 등록
            //info = new PlayerInfo(NickName,
            //                      CharacterType.Joohyeok,
            //                      info.MaxHP, info.MaxSkillPoint, info.Speed);
            //Attack 등록
            attack = GetComponent<Securitas_Atk>();
            if (!attack)
                attack = gameObject.AddComponent<Securitas_Atk>();
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
