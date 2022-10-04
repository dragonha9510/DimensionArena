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
    [SerializeField] private Transform directionLocation;
    [HideInInspector] public Vector3 direction;
    
    /// ============================= 


    /// =============================
    /// Component Type Region
    /// =============================

    [SerializeField] private PlayerInfo info;
    public  PlayerInfo Info { get { return info; } }
    private Prototype_Movement movement;
    private Rigidbody rigid;

    PhotonView pc;

    /// =============================
    private void Awake()
    {
        pc = photonView;
        info = new PlayerInfo();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameObject.Find("MoveJoyStick").GetComponent<JoyStick>().player = this;
            GameObject.Find("Target Camera").GetComponent<Prototype_TargetCamera>().target = this.transform;
        }

        movement = new Prototype_Movement();

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
