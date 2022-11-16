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

 

        
        public bool CanDirectionChange { get; set; }

        
        private isHideOnBush bushRenderCheck;



        protected virtual void Awake()
        {
            if (PhotonNetwork.InRoom)
            {
                if (photonView.Owner != null)
                    gameObject.name = photonView.Owner.NickName;
            }
            else
            {
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
            Info.EDisActivePlayer += PlayerInfoManager.Instance.DiePlayer;
            Info.EDisActivePlayer += DisActiveAnimation;
            Info.EBattleStateOn += BattleStateProcess;
            bushRenderCheck = GetComponentInChildren<isHideOnBush>();
        }

        private Coroutine lastCoroutine = null;
        private void BattleStateProcess()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if(lastCoroutine != null)
                StopCoroutine(lastCoroutine);

            lastCoroutine = StartCoroutine(BattleStateProcessCoroutine());
        }

        private IEnumerator BattleStateProcessCoroutine()
        {
            bushRenderCheck.AppearForMoment(info.BattleOffTime);
            yield return new WaitForSeconds(info.BattleOffTime);
            info.BattleOff();
        }

        private void Update()
        {
            if (direction.magnitude >= 1.0f)
                direction.Normalize();

            if (direction.Equals(Vector3.zero))
                directionLocation.gameObject.SetActive(false);
            else
            {
                directionLocation.gameObject.SetActive(true);
                directionLocation.position = transform.position + (direction * 1.25f) + new Vector3(0, 0.1f, 0);
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

            if(touchCanvas.player == null)
                touchCanvas.player = this;
            info.EDisActivePlayer += touchCanvas.DisActiveTouch;


            MoveJoyStick joyStick = GameObject.Find("MoveJoyStick").
                    GetComponent<MoveJoyStick>();

            if(!joyStick.player)
                joyStick.player = this;

            SkillJoyStick skilljoyStick = GameObject.Find("SkillJoyStick").
                GetComponent<SkillJoyStick>();
            

            if(!skilljoyStick.player)
                skilljoyStick.player = this;
            info.EskillAmountChanged += skilljoyStick.SkillSetFillAmount;


            AtkJoyStick atkjoyStick = GameObject.Find("AtkJoyStick").
                GetComponent<AtkJoyStick>();

            if(!atkjoyStick.player)
                atkjoyStick.player = this;

            Transform target = GameObject.Find("Target Camera").
                GetComponent<Prototype_TargetCamera>().target;

            if (target.name.Equals("Dummy"))
            {
                GameObject.Find("Target Camera").
                GetComponent<Prototype_TargetCamera>().target = this.transform;
            }
            
        }

        public void DisActiveAnimation()
        {
            EffectManager.Instance.CreateParticleEffectOnGameobject(this.transform, "Dead");
            gameObject.SetActive(false);
        }

    }
}
