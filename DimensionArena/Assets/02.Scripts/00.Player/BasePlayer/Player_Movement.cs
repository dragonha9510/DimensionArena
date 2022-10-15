using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class Player_Movement
{
    Player owner;

    public Player_Movement(Player owner)
    {
        this.owner = owner;
    }

    public void MoveDirection(Rigidbody rigid, Transform transform, Vector3 direction, float speed)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, direction, 720.0f * Time.deltaTime / Vector3.Angle(transform.forward, direction));  
            
            if(owner.CanDirectionChange)
                transform.LookAt(transform.position + forward);
        }

        direction.y = 0;

        rigid.velocity = direction * speed;
        //transform. = transform.position + direction * speed * Time.deltaTime;
    }
}
