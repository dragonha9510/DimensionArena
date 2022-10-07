using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
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

    [SerializeField] private PlayerInfo info;
    public  PlayerInfo  Info { get { return info; } }
    public  Player_Atk  attack;
    private Player_Movement movement;
    private Rigidbody rigid;


    private int photon_id;
    public bool CanDirectionChange { get; set; }
    
    /// =============================
    
    private void Awake()
    {
        photon_id = photonView.ViewID;
        gameObject.name = photon_id.ToString();
        info = new PlayerInfo(gameObject.name);
    }

    private void Start()
    {
        CanDirectionChange = true;

        if (photonView.IsMine)
        {
            GameObject.Find("MoveJoyStick").GetComponent<JoyStick>().player = this;
            GameObject.Find("Target Camera").GetComponent<Prototype_TargetCamera>().target = this.transform;
        }

        movement = new Player_Movement(this);

        TryGetComponent<Rigidbody>(out rigid);

        if (rigid == null)
            Destroy(this.gameObject);
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


}
