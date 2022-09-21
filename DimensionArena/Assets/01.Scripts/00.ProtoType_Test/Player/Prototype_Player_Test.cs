using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Player_Test : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;

    [HideInInspector] public Vector3 direction;
    
    private Prototype_Movement movement;
    private GameObject avatar;
    private Rigidbody rigid;

    private void Start()
    {
        movement = new Prototype_Movement();

        if (transform.childCount != 0)
            avatar = transform.GetChild(0).gameObject;

        TryGetComponent<Rigidbody>(out rigid);

        if (rigid == null)
            Destroy(this.gameObject);
    }

    private void Update()
    {
        movement.MoveDirection(transform, direction, speed);
    }
}
