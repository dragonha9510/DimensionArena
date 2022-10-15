using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Photon;

namespace PlayerSpace
{
    public abstract class Player : MonoBehaviourPunCallbacks
    {
        /*
        static Player ownerPlayer;
        public static Player OwnerPlayer => ownerPlayer;
        */


        /// =============================
        /// Direction Region
        /// =============================
        [SerializeField] private Transform directionLocation;
        [HideInInspector] public Vector3 direction;

        /// ============================= 
        /// =============================
        /// Component Type Region
        /// =============================
        [SerializeField] protected PlayerInfo info;
        public PlayerInfo Info { get { return info; } }

        protected Player_Atk attack;
        public Player_Atk Attack => attack;

        private Player_Movement movement;
        private Rigidbody rigid;
        private string nickName;
        public string NickName => nickName;
        public bool CanDirectionChange { get; set; }

        /// =============================

        protected virtual void Awake()
        {
            if (photonView.Owner != null)
            {
                nickName = photonView.Owner.NickName;
                gameObject.name = nickName;
            }
        }

        protected virtual void Start()
        {
            movement = new Player_Movement(this);
            CanDirectionChange = true;
            TryGetComponent<Rigidbody>(out rigid);

            if (rigid == null)
                Destroy(this.gameObject);

            if (photonView.IsMine)
                SetToOwnerPlayer();
        }

        private void Update()
        {
            if (direction.Equals(Vector3.zero))
                directionLocation.gameObject.SetActive(false);
            else
            {
                directionLocation.gameObject.SetActive(true);
                directionLocation.position = transform.position + (direction * 1.25f);
            }
        }
        private void LateUpdate()
        {
            movement.MoveDirection(rigid, transform, direction, info.Speed);
        }

        private void Destroy()
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        private void SetToOwnerPlayer()
        {
            TouchCanvas touchCanvas = GameObject.Find("TouchCanvas").
                GetComponent<TouchCanvas>();
            touchCanvas.player = this;
            info.EDisActivePlayer += touchCanvas.DisActiveTouch;


            JoyStick joyStick = GameObject.Find("MoveJoyStick").
                    GetComponent<JoyStick>();
            joyStick.player = this;


            SkillJoyStick skilljoyStick = GameObject.Find("SkillJoyStick").
                GetComponent<SkillJoyStick>();
            info.EskillAmountChanged += skilljoyStick.SkillSetFillAmount;
            skilljoyStick.player = this;

            AtkJoyStick atkjoyStick = GameObject.Find("AtkJoyStick").
                GetComponent<AtkJoyStick>();
            atkjoyStick.player = this;


            GameObject.Find("Target Camera").
                GetComponent<Prototype_TargetCamera>().target = this.transform;
        }

    }
}
