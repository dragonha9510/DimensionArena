using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : BaseEvent
{
    bool isDetected = false;
    public override bool CheckEventState()
    {
        return isDetected;
    }

    public override void EventSuccesed()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isDetected = true;
    }
}
