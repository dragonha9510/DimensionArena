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
        private ParticleSystem passiveParticle;
        [SerializeField]
        private float originParticleRotateSpeed = 1;
        [SerializeField]
        private float particleSpeedMagnification = 0.5f;

        private ParticleSystem.MainModule main;


        [SerializeField]
        private List<int> praticleCount = new List<int>();

        // 속도:
        // a 시작 속도 직렬화
        // b 0.2 직렬화
        // a += b
        // 입자수 :
        // [직렬화]

        private float moveDistance = 0f;
        private Vector3 prevPos = new Vector3();
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            passiveParticle = GetComponentInChildren<ParticleSystem>();
            base.Start();
            StartCoroutine(PassiveStartSetting());

            main = passiveParticle.main;
            main.maxParticles = praticleCount[nestingCount];

            main.simulationSpeed = originParticleRotateSpeed;
        }


        private void ParticleAdd()
        {
            main.maxParticles = praticleCount[nestingCount];
            main.simulationSpeed += particleSpeedMagnification;
        }

        IEnumerator PassiveStartSetting()
        {
            while (true)
            {
                if (GameManager.Instance.IsSpawnEnd)
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
                    Debug.Log("패시브 초기화");
                    PlayerInfoManager.Instance.SpeedDecrease(this.name, speedNesting * nestingCount);
                    nestingCount = 0;
                    moveDistance = 0;
                    
                    main.simulationSpeed = 0;
                    main.simulationSpeed = originParticleRotateSpeed;
                    main.maxParticles = praticleCount[nestingCount];
                }
                else if (false == info.IsBattle && prevPos != this.transform.position && nestingCount < maxNestingCount - 1)
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
