using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Aura : Player
    {
        [SerializeField]
        private float auraSpeed = 2.5f;
        [SerializeField]
        private float speedMaxMagnification = 1.1f;
        [SerializeField]
        private int nestingCount = 5;
        [SerializeField]
        private float netingTiming = 0f;

        private float maxSpeed = 0f;
        [Header("AuraPessive")]
        [SerializeField]
        private float speedNesting = 0f;

        private float moveTime = 0f;

        protected override void Awake()
        {
            base.Awake();
            auraSpeed = info.Speed;
            maxSpeed = auraSpeed * maxSpeed;
            speedNesting = (maxSpeed - auraSpeed) / nestingCount;

        }
        protected override void Start()
        {
            base.Start();
        }
    }

}
