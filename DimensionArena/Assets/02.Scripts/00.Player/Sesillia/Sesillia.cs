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

            //Attack ���
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

                //��Ʋ���� �ƴҶ� ü���� Ű���.
                if (!Info.IsBattle && !Info.MaxHP.Equals(info.CurHP))
                {
                    if (healTime >= healTickTime)
                    {
                        Debug.Log(info.MaxHP * healPercent + "��ŭ ȸ��..");
                        PlayerInfoManager.Instance.CurHpIncrease(gameObject.name, info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }

                //ü���� ���� �����϶� ��ȣ�� ����
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        Debug.Log("�ǵ� �нú� Ȱ��ȭ!");
                        PlayerInfoManager.Instance.GetShield(gameObject.name, info.MaxHP * shieldPercent);
                        //and Create Particle
                        shieldtime = 0.0f;
                    }
                    else
                        Debug.Log("�ǵ� �нú� �ð� �����..");
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

                //��Ʋ���� �ƴҶ� ü���� Ű���.
                if (!Info.IsBattle && !Info.MaxHP.Equals(info.CurHP))
                {
                    if (healTime >= healTickTime)
                    {
                        Debug.Log(info.MaxHP * healPercent + "��ŭ ȸ��..");
                        info.Heal(info.MaxHP * healPercent);
                        healTime = 0;
                    }
                }

                //ü���� ���� �����϶� ��ȣ�� ����
                if (Info.CurHP / Info.MaxHP < shieldPassivePercent)
                {
                    if (shieldtime >= shieldPassiveCount)
                    {
                        Debug.Log("�ǵ� �нú� Ȱ��ȭ!");
                        info.GetShield(info.MaxHP * shieldPercent);
                        //and Create Particle
                        shieldtime = 0.0f;
                    }
                    else
                        Debug.Log("�ǵ� �нú� �ð� �����..");
                }
                yield return null;
            }
        }


    }
}
