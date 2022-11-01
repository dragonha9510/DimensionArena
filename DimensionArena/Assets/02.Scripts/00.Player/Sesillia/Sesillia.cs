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

            //Attack ���
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
                //��Ʋ���� �ƴҶ� ü���� Ű���.
                if(!isBattle)
                {
                    PlayerInfoManager.Instance.CurHpIncrease(gameObject.name, info.MaxHP * hpPercentRatio);
                }

                //ü���� ���� �����϶� ��ȣ�� ����
                if(Info.CurHP / Info.MaxHP < hpPercentShield)
                {
                    PlayerInfoManager.Instance.GetShield(gameObject.name, info.MaxHP * shieldPercent);
                }
         
            }
        }
    }
}

