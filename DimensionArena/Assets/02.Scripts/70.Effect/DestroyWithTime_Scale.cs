using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime_Scale : DestroyWithTime
{
    public float delayTime = 1f;
    private float maxTime;

    Vector3 oriScale;

    private void Start()
    {
        maxTime = base.deathTime + delayTime;
        oriScale = transform.localScale;
    }

    // Update is called once per frame
    override protected void Update()
    {
        Debug.Log(transform.localScale);

        curTime += Time.deltaTime;

        if (curTime >= deathTime)
        {
            transform.localScale = oriScale * ((delayTime - (curTime - deathTime)) / delayTime);

            if (curTime >= maxTime)
                Destroy(this.gameObject);
        }
    }
}
