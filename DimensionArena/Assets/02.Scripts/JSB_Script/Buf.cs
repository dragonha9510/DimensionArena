using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf
{
    public string playerName;
    public float durationTime;
    public float amount;
    public ITEM itemType;
    public Buf(string ID,float time, float amount,ITEM type)
    {
        this.playerName = ID;
        this.durationTime = time;
        this.itemType = type;
        this.amount = amount;
    }
}
