using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime_Height : DestroyWithTime
{
    public float delayTime = 1f;
    public float hegiht = 1f;
    private float maxTime;

    Vector3 oriPosition;

    private void Start()
    {
        maxTime = deathTime + delayTime;
        oriPosition = transform.position;
    }

    // Update is called once per frame
    override protected void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= deathTime)
        {
            Vector3 tempPos = transform.position;
            tempPos.y = oriPosition.y - ((curTime - deathTime) / delayTime) * hegiht;
            transform.position = tempPos;

            if (curTime >= maxTime)
                Destroy(this.gameObject);
        }
    }
}