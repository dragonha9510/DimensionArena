using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEvent : BaseEvent
{
    [SerializeField] TriggerEvent[] triggers;
    [SerializeField] ProjectileEvent[] projectiles;
    public override bool CheckEventState()
    {
        for (int i = 0; i < triggers.Length; ++i)
            if (triggers[i].IsDetected)
                return true;

        return false;
    }

}
