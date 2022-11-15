using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    [SerializeField] protected int eventIdx;
    public int EventIdx => eventIdx;
    [SerializeField] private string temp;

    public virtual string EventSuccesed()
    {
        return temp;
    }
    public abstract bool CheckEventState();
}
