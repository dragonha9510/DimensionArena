using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
public class AllEnemyDieEvent : BaseEvent
{
    [SerializeField] Player[] enemy;
    public override bool CheckEventState()
    {
        for (int i = 0; i < enemy.Length; ++i)
            if (enemy[i].Info.CurHP > 0)
                return false;

        return true;
    }
}
