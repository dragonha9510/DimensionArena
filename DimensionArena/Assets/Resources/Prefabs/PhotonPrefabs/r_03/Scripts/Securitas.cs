using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PlayerSpace
{
    // Start is called before the first frame update
    public class Securitas : Player
    {

        protected override void Awake()
        {
            base.Awake();
            ////Player State ???
            //info = new PlayerInfo(NickName,
            //                      CharacterType.Joohyeok,
            //                      info.MaxHP, info.MaxSkillPoint, info.Speed);
            //Attack ???
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