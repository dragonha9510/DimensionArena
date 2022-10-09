using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public abstract class Player : MonoBehaviourPun
{
    /// =============================
    /// Direction Region
    /// =============================
    [SerializeField]  private Transform directionLocation;
    [HideInInspector] public  Vector3 direction;
    
    /// ============================= 


    /// =============================
    /// Component Type Region
    /// =============================

    [SerializeField] protected PlayerInfo info;
    public  PlayerInfo  Info { get { return info; } }


    protected  Player_Atk   attack;
    private Player_Movement movement;
    private Rigidbody rigid;


    private string nickName;
    public string NickName => nickName;

    public bool CanDirectionChange { get; set; }
    
    /// =============================
    
    protected virtual void Awake()
    {   
        nickName = photonView.Owner.NickName;
        gameObject.name = nickName;
    }

    protected virtual void Start()
    {
        CanDirectionChange = true;

        if (photonView.IsMine)
        {
            GameObject.Find("MoveJoyStick").
                GetComponent<JoyStick>().player = this;
            GameObject.Find("Target Camera").
                GetComponent<Prototype_TargetCamera>().target = this.transform;
        }

        movement = new Player_Movement(this);

        TryGetComponent<Rigidbody>(out rigid);

        if (rigid == null)
            Destroy(this.gameObject);

        Info.EDestroyPlayer += Destroy;
    }


    private void Update()
    {
        if (direction.Equals(Vector3.zero))
        {
            directionLocation.gameObject.SetActive(false);
        }
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
        if (!photonView.IsMine)
            return;
        PhotonNetwork.Destroy(this.gameObject);
    }





}
