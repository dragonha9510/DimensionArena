using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedEvent : BaseEvent
{
    [SerializeField] EnemyDamaged[] enemy;
    public override bool CheckEventState()
    {
        for(int i = 0; i < enemy.Length; ++i)
        {
            if (enemy[i].IsTrigger)
                return true;
        }
        return false;
    }
}
