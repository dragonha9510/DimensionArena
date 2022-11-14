using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagerSpace;

namespace PlayerSpace
{
    public class Aura : Player
    {
       
        [Header("AuraPessive")]
        [SerializeField]
        private float speedNesting = 0.1f;
        [SerializeField]
        private int maxNestingCount = 5;
        private int nestingCount = 0;
        [SerializeField]
        private float nestingTimingDistance = 20.0f;
        [SerializeField]
        private int maxParticle = 20;
        [SerializeField]
        private int particleMagnification = 3;
        private ParticleSystem passiveParticle;

        ParticleSystem.MainModule main;

        private float moveDistance = 0f;
        private Vector3 prevPos = new Vector3();
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            passiveParticle = GetComponentInChildren<ParticleSystem>();
            StartCoroutine(PassiveStartSetting());

            main = passiveParticle.main;
            main.maxParticles = maxParticle;
        }
        
        private void ParticleAdd()
        {
            main.maxParticles = maxParticle * particleMagnification;
        }

        IEnumerator PassiveStartSetting()
        {
            while(true)
            {
                if(GameManager.Instance.IsSpawnEnd)
                {
                    StartCoroutine(nameof(SettingOriginalPos));
                    yield break;
                }
                yield return null;
            }
        }
        
        IEnumerator SettingOriginalPos()
        {
            prevPos = this.transform.position;
            while (true)
            {
                if (true == info.IsBattle)
                {
                    PlayerInfoManager.Instance.SpeedDecrease(this.name, speedNesting * nestingCount);
                    nestingCount = 0;
                    moveDistance = 0;
                    main.maxParticles = maxParticle;
                }
                else if (false == info.IsBattle && prevPos != this.transform.position && nestingCount < maxNestingCount)
                {
                    moveDistance += Vector3.Distance(prevPos, transform.position);
                    prevPos = this.transform.position;
                    if (nestingTimingDistance <= moveDistance)
                    {
                        PlayerInfoManager.Instance.SpeedIncrease(this.name, speedNesting);
                        moveDistance = 0f;
                        ++nestingCount;
                        ParticleAdd();
                    }

                }
                yield return null;

            }
        }
    }

}
