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
        
        public Player_Movement Movement { get { return movement; } }
        private Rigidbody rigid;

        /// =============================

        private string nickName;
        public string NickName => nickName;


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
                nickName = "Player" + gameObject.name;
                gameObject.name = "Player" + gameObject.name;
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
            Info.EBattleStateOn += BattleStateProcess;
        }

        private Coroutine lastCoroutine = null;
        private void BattleStateProcess()
        {
            if(lastCoroutine != null)
                StopCoroutine(lastCoroutine);

            lastCoroutine = StartCoroutine(BattleStateProcessCoroutine());
        }

        private IEnumerator BattleStateProcessCoroutine()
        {
            Debug.Log("배틀 상태 ON");
            yield return new WaitForSeconds(info.BattleOffTime);
            info.BattleOff();
            Debug.Log("배틀 상태 OFF");
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



    }
}
