using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Secilia : Player
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

        [SerializeField] ParticleSystem shieldpassiveEffect;
        [SerializeField] ParticleSystem healpassiveEffect;
        [SerializeField] float effectPlayTime;
        ParticleSystem.MainModule mainParticle;
        protected override void Awake()
        {
            base.Awake();

            //Attack 등록
            attack = GetComponent<Secilia_Atk>();

            if (!attack)
                attack = gameObject.AddComponent<Secilia_Atk>();

            ParticleSystem.MainModule particle = shieldpassiveEffect.main;
            particle.startLifetime = effectPlayTime;

            for (int i = 0; i < shieldpassiveEffect.gameObject.transform.childCount; ++i)
            {
                particle = shieldpassiveEffect.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().main;
                particle.startLifetime = effectPlayTime;
            }
         
        }


        protected override void Start()
        {
            base.Start();

            if(photonView.IsMine)
            { 
                if (PhotonNetwork.InRoom)
                    StartCoroutine(StartPassiveFromOnline());
                else
                    StartCoroutine(StartPassiveFromOff());
            }
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
                        if (!healpassiveEffect.isPlaying)
                            healpassiveEffect.Play();

                        PlayerInfoManager.Instance.CurHpIncrease(gameObject.name, info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }
                else
                {
                    if (healpassiveEffect.isPlaying)
                        healpassiveEffect.Stop();
                }

                //체력이 일정 이하일때 보호막 생성
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        shieldpassiveEffect.Play();
                        PlayerInfoManager.Instance.GetShield(gameObject.name, info.MaxHP * shieldPercent);
                        shieldtime = 0.0f;
                        yield return new WaitForSeconds(effectPlayTime);
                        shieldpassiveEffect.Stop();
                    }
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
                        if(!healpassiveEffect.isPlaying)
                            healpassiveEffect.Play();
                        info.Heal(info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }
                else
                {
                    if (healpassiveEffect.isPlaying)
                        healpassiveEffect.Stop();
                }
                //체력이 일정 이하일때 보호막 생성
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        if (!shieldpassiveEffect.isPlaying)
                            shieldpassiveEffect.Play();
                        info.GetShield(info.MaxHP * shieldPercent);
                        //and Create Particle
                        shieldtime = 0.0f;
                        yield return new WaitForSeconds(effectPlayTime);
                        shieldpassiveEffect.Stop();
                    }
                }
                yield return null;
            }
        }


    }
}
