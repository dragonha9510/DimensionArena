using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime : MonoBehaviour
{
    protected float curTime;
    public float deathTime = 10;

    // Update is called once per frame
    virtual protected void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= deathTime)
            Destroy(this.gameObject);
    }
}
