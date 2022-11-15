using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{

    [SerializeField] protected bool isBlockTouch;

    public virtual void EventSuccesed()
    {
        TutorialEvent.Instance.TouchBlockOnOff(isBlockTouch);
    }

    public abstract bool CheckEventState();
}
