using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Photon;

namespace PlayerSpace
{
    public class InGamePlayerData
    {
        public InGamePlayerData()
        {
            damage = 0;
            liveTime = 0;
            kill = 0;
        }


        public void HitPoint(float dmg)
        {
            damage += dmg;
            
        }

        public void KillPoint()
        {
            kill++;
        }


        float damage;
        float liveTime;
        int kill;
    }



    public abstract class Player : MonoBehaviourPunCallbacks
    {

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

        protected Player_Skill skill;
        public Player_Skill Skill => skill;

        private Player_Movement movement;

        private Rigidbody rigid;

        private InGamePlayerData playerdata;
        public InGamePlayerData PlayerData => playerdata;
        /// =============================

        private string nickName;
        public string NickName => nickName;

        //나중에 고칠것
        private float reviveTime = 3.0f;
        public bool CanDirectionChange { get; set; }



        protected virtual void Awake()
        {
            if (PhotonNetwork.InRoom)
            {
                if (photonView.Owner != null)
                {
                    nickName = photonView.Owner.NickName;
                    gameObject.name = nickName;
                    playerdata = new InGamePlayerData();
                }
            }
            else
            {
                nickName = "Player";
                gameObject.name = "Player";
                playerdata = new InGamePlayerData();
            }
        }

        protected virtual void Start()
        {
            //Memory Register
            CanDirectionChange = true;
            attack = GetComponent<Player_Atk>();
            skill = GetComponent<Player_Skill>();
            movement = new Player_Movement(this);
            TryGetComponent<Rigidbody>(out rigid);

            if (rigid == null)
                Destroy(this.gameObject);

            if (PhotonNetwork.InRoom)
            {
                if (photonView.IsMine)
                    SetToOwnerPlayer();
            }
            else
                SetToOwnerPlayer();

            //Create Event
            EffectManager.Instance.CreateParticleEffectOnGameobject(this.transform, "Revive");

            //Add Event

            Info.EDisActivePlayer += DisActiveAnimation;
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

        private void SetToOwnerPlayer()
        {
            TouchCanvas touchCanvas = GameObject.Find("TouchCanvas").
                GetComponent<TouchCanvas>();
            touchCanvas.player = this;
            info.EDisActivePlayer += touchCanvas.DisActiveTouch;


            MoveJoyStick joyStick = GameObject.Find("MoveJoyStick").
                    GetComponent<MoveJoyStick>();
            joyStick.player = this;

            SkillJoyStick skilljoyStick = GameObject.Find("SkillJoyStick").
                GetComponent<SkillJoyStick>();
            info.EskillAmountChanged += skilljoyStick.SkillSetFillAmount;
            skilljoyStick.player = this;

            AtkJoyStick atkjoyStick = GameObject.Find("AtkJoyStick").
                GetComponent<AtkJoyStick>();
            atkjoyStick.player = this;

            SkillJoyStick skillJoyStick = GameObject.Find("SkillJoyStick").
                GetComponent<SkillJoyStick>();
            skilljoyStick.player = this;

            GameObject.Find("Target Camera").
                GetComponent<Prototype_TargetCamera>().target = this.transform;
        }

        private void DisActiveAnimation()
        {
            EffectManager.Instance.CreateParticleEffectOnGameobject(this.transform, "Dead");
            gameObject.SetActive(false);
        }

    }
}
