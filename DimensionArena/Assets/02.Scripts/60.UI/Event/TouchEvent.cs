using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEvent : BaseEvent
{

    public override bool CheckEventState()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        return false;
    }

    public override void EventSuccesed()
    {
        TutorialEvent.Instance.TouchBlockOnOff(isBlockTouch);
    }
}
