using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Ravagebell : Player
    {
        protected override void Awake()
        {
            base.Awake();

            //Player State 등록
            //info = new PlayerInfo(NickName,
            //                      CharacterType.Ravagebell,
            //                      info.MaxHP, info.MaxSkillPoint, info.Speed);
            //Attack 등록
            attack = GetComponent<Ravagebell_Atk>();
            if (!attack)
                attack = gameObject.AddComponent<Ravagebell_Atk>();
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
