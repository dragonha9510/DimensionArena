using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class SkillEvent : BaseEvent
{
    [SerializeField] Player player;

    public override bool CheckEventState()
    {
        return player.Info.CurSkillPoint.Equals(player.Info.MaxSkillPoint);
    }


}
