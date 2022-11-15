using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class AttackEvent : BaseEvent
{
    public Player_Atk atk;

    public override bool CheckEventState()
    {
        if (atk.IsAttack)
            return true;

        return false;
    }

    public override void EventSuccesed()
    {
        base.EventSuccesed();
    }
}
