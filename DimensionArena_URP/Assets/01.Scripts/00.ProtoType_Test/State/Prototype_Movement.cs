using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Movement
{
    public void MoveDirection(Rigidbody rigid, Transform transform, Vector3 direction, float speed)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, direction, 720.0f * Time.deltaTime / Vector3.Angle(transform.forward, direction));

            transform.LookAt(transform.position + forward);
        }

        direction.y = 0;

        rigid.velocity = direction * speed;
        //transform. = transform.position + direction * speed * Time.deltaTime;
    }
}
