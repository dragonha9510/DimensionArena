using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    [SerializeField] protected bool isEventOn = false;
    public bool IsEventOn => isEventOn;

    public abstract void EventSuccesed();

    public abstract bool CheckEventState();
}
