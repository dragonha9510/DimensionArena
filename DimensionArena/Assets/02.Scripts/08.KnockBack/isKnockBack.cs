using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isKnockBack : MonoBehaviour
{
    [HideInInspector] public KnockBackInfo info;
    private float curSpeed;
    private float realSpeed;
    private float curDistance;
    private void Start()
    {
        
    }

    public void SetValue()
    {
        curDistance = 0;
        curSpeed = realSpeed = info.speed;
    }

    private void Update()
    {
        if (info.isOn == false)
            return;

        float speed = curSpeed * Time.deltaTime;
        curDistance += speed;

        transform.position += info.direction.normalized * speed;

        if (info.distance <= curDistance)
        { 
            info.isOn = false;
            this.enabled = false;
        }
        else
            curSpeed -= realSpeed * Time.deltaTime * curDistance;
    }


    public void CallMoveKnockBack(Vector3 pos,Vector3 direction, float speed, float knockbackDistance)
    {
        StartCoroutine(MoveKnockBack(pos,direction, speed, knockbackDistance));
    }

    private IEnumerator MoveKnockBack(Vector3 pos,Vector3 direction , float speed , float knockbackDistance)
    {
        while(true)
        {
            if (Vector3.Distance(pos, this.transform.position) > knockbackDistance)
                yield break;
            else
            {
                Debug.Log("움직이는중");
                this.transform.position = this.transform.position + direction * speed * Time.fixedDeltaTime;
                yield return null;
            }
        }
    }

}

