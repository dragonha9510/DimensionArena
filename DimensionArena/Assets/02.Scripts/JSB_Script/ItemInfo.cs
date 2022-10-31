using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public string itemNumber;
    public string item_ID;
    public float achieveRange;
    public float liveTime;

    public bool attackNesting;

    public float attackIncrement;
    public float speedAmount;
    public float healthAmount;
    public float shieldAmount;
    public float skillRecovery;
}
