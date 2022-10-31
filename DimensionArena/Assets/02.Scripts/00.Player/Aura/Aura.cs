using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Aura : Player
    {



        protected override void Awake()
        {
            base.Awake();

            //Player State 등록
            info = new PlayerInfo(NickName,
                                  CharacterType.Aura,
                                  100.0f, 100.0f, 2.5f);
            //Attack 등록
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

