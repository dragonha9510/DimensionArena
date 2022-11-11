using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class Player_Movement
{
    Player owner;

    private bool canMove = true;
    public bool CanMove { set { canMove = value; } }


    public Player_Movement(Player owner)
    {
        this.owner = owner;
    }

    public void MoveDirection(Rigidbody rigid, Transform transform, Vector3 direction, float speed)
    {
        if (!canMove)
            return;
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, direction, 720.0f * Time.deltaTime / Vector3.Angle(transform.forward, direction));  
            
            if(owner.CanDirectionChange)
                transform.LookAt(transform.position + forward);
        }

        direction = (direction.x.Equals(1.0f) || direction.z.Equals(1.0f)) ? new Vector3(direction.x * 0.99f + 0.03f, 0, direction.z * 0.99f + 0.03f) : direction;
        direction.y = 0;
        
        rigid.velocity = direction * speed;
        //transform. = transform.position + direction * speed * Time.deltaTime;
    }
}
