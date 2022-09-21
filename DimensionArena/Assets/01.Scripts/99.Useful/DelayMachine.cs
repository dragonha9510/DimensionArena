using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayMachine
{
    private float maxDelay = 0.0f;
    private float curTime = 0.0f;
    private bool isOn = false;

    public float CurTime { get { return curTime; } private set { } }
    public float MaxTime { get { return maxDelay; } private set { } }


    /// <summary>
    /// Delay Start with "Float" delay Time
    /// if curTime pass delay this component will be ready ( check with IsReady() )
    /// </summary>
    /// <param name="delay"> delay is max time </param>
    public void DelayStart(float delay)
    {
        isOn = true;
        maxDelay = delay;
    }

    /// <summary>
    /// Have to call this func for recycle delay
    /// ( if need new max delay call DelayEnd & DelayStart )
    /// </summary>
    public void DelayReset()
    {
        curTime = 0.0f;
    }

    /// <summary>
    /// Delay Shut Down
    /// </summary>
    public void DelayEnd()
    {
        isOn = false;
        maxDelay = 0;
        curTime = 0;
    }

    /// <summary>
    /// Should be call in Update or LateUpdate
    /// ( Have to call DelayStart first )
    /// </summary>
    /// <returns>if Ready return true</returns>
    public bool IsReady()
    {
        if (!isOn)
            return false;

        curTime += Time.deltaTime;

        if (curTime >= maxDelay)
        {
            return true;
        }

        return false;
    }
}
