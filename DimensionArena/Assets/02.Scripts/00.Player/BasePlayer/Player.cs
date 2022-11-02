using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using ManagerSpace;
using Photon;

namespace PlayerSpace
{

    public abstract class Player : MonoBehaviourPunCallbacks
    {
        Animator animator;

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

        /// =============================

        private string nickName;
        public string NickName => nickName;

        private bool isInMangeticField = false;
        // 뭣같은 자기장 알고리즘 때문에 생긴 변수
        private int collisionMagneticCount = 0;


        //나중에 고칠것
        public bool CanDirectionChange { get; set; }



        protected virtual void Awake()
        {
            if (PhotonNetwork.InRoom)
            {
                if (photonView.Owner != null)
                {
                    nickName = photonView.Owner.NickName;
                    gameObject.name = nickName;
                }
            }
            else
            {
                nickName = "Player";
                gameObject.name = "Player";
            }

            info.ID = gameObject.name;
        }

        protected virtual void Start()
        {
            // Animation
            animator = GetComponentInChildren<Animator>();

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

                EffectManager.Instance.CreateParticleEffectOnGameobject(this.transform, "Revive");
            }
            else
                SetToOwnerPlayer();

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
            animator.SetFloat("speed", direction.magnitude);
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


        public void Win()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            if(photonView.IsMine)
            {
                InGameUIManager.Instance.WinUIOn();
                IngameDataManager.Instance.OwnerData.SetRank();
                gameObject.SetActive(false);
            }
        }



        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (other.gameObject.tag == "MagneticField")
            {
                --collisionMagneticCount;
                if (0 == collisionMagneticCount)
                {
                    Debug.Log("자기장 멈춰");
                    isInMangeticField = false;
                }
            }
        }

    }
}
