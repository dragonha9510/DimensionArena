using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Player_Test : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Transform directionLocation;

    [HideInInspector] public Vector3 direction;

    private Ray AtkRangeRay;
    
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
        if(direction.Equals(Vector3.zero))
        {
            directionLocation.gameObject.SetActive(false);
        }
        else
        {
            directionLocation.gameObject.SetActive(true);
            directionLocation.position = transform.position + (direction * 1.25f);
        }
        movement.MoveDirection(transform, direction, speed);
    }
}
