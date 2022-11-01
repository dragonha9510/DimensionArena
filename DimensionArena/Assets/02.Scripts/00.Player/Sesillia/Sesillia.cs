using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Sesillia : Player
    {
        [SerializeField] private float hpPercentRatio;
        [SerializeField] private float hpPercentShield;
        [SerializeField] private float shieldPercent;
        bool isBattle;
        
        protected override void Awake()
        {
            base.Awake();

            //Attack 등록
            attack = GetComponent<Sesillia_Atk>();

            if (!attack)
                attack = gameObject.AddComponent<Sesillia_Atk>();
        }


        protected override void Start()
        {
            base.Start();
        }

        
        IEnumerator StartPassive()
        {
            while(true)
            {
                //배틀중이 아닐때 체력을 키운다.
                if(!isBattle)
                {
                    PlayerInfoManager.Instance.CurHpIncrease(gameObject.name, info.MaxHP * hpPercentRatio);
                }

                //체력이 일정 이하일때 보호막 생성
                if(Info.CurHP / Info.MaxHP < hpPercentShield)
                {
                    PlayerInfoManager.Instance.GetShield(gameObject.name, info.MaxHP * shieldPercent);
                }
         
            }
        }
    }
}

