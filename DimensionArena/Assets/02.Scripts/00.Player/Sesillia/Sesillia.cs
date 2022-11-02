using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Sesillia : Player
    {
        [Header("Sesillia Passive Active Percent")]
        [SerializeField] private float shieldPassivePercent;

        [Header("Sesillia heal, shield Amount")]
        [SerializeField] private float healPercent;
        [SerializeField] private float shieldPercent;

        [Header("Sesillia Passive Delay Time")]
        [SerializeField] private float healTickTime;
        [SerializeField] private float shieldPassiveCount;
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

            if (PhotonNetwork.InRoom)
                StartCoroutine(StartPassiveFromOnline());
            else
                StartCoroutine(StartPassiveFromOff());

        }


        IEnumerator StartPassiveFromOnline()
        {
            float shieldtime = 0.0f;
            float healTime = 0.0f;

            while (true)
            {
                shieldtime += Time.deltaTime;
                healTime += Time.deltaTime;

                //배틀중이 아닐때 체력을 키운다.
                if (!Info.IsBattle && !Info.MaxHP.Equals(info.CurHP))
                {
                    if (healTime >= healTickTime)
                    {
                        Debug.Log(info.MaxHP * healPercent + "만큼 회복..");
                        PlayerInfoManager.Instance.CurHpIncrease(gameObject.name, info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }

                //체력이 일정 이하일때 보호막 생성
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        Debug.Log("실드 패시브 활성화!");
                        PlayerInfoManager.Instance.GetShield(gameObject.name, info.MaxHP * shieldPercent);
                        //and Create Particle
                        shieldtime = 0.0f;
                    }
                    else
                        Debug.Log("실드 패시브 시간 대기중..");
                }

                yield return null;
            }
        }

        IEnumerator StartPassiveFromOff()
        {
            float shieldtime = 0.0f;
            float healTime = 0.0f;

            while (true)
            {
                shieldtime += Time.deltaTime;
                healTime += Time.deltaTime;

                //배틀중이 아닐때 체력을 키운다.
                if (!Info.IsBattle && !Info.MaxHP.Equals(info.CurHP))
                {
                    if (healTime >= healTickTime)
                    {
                        Debug.Log(info.MaxHP * healPercent + "만큼 회복..");
                        info.Heal(info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }

                //체력이 일정 이하일때 보호막 생성
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        Debug.Log("실드 패시브 활성화!");
                        info.GetShield(info.MaxHP * shieldPercent);
                        //and Create Particle
                        shieldtime = 0.0f;
                    }
                    else
                        Debug.Log("실드 패시브 시간 대기중..");
                }
                yield return null;
            }
        }


    }
}
