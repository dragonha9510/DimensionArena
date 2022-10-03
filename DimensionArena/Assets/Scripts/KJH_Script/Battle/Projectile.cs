using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackObject
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Vector3 originPos;

    //basic Range 10
    float range = 10.0f;

    private void Awake()
    {
        originPos = transform.position;
    }

    private void LateUpdate()
    {
        if(range < (this.transform.position - originPos).magnitude)
        {
            Destroy(this.gameObject);
        }
    }

    public void AttackToDirection(Vector3 dir, float range, float speed)
    {
        this.range = range;
        rigid.velocity = dir * speed;
    }
}