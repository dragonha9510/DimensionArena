using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string playerName = "";
    public int[] winCount = new int[(int)MODE.MODE_TRAINING] { 0, 0, 0, 0 };
    public int[] loseCount = new int[(int)MODE.MODE_TRAINING] { 0, 0, 0, 0 };

    public PlayerData(string Name)
    {
        playerName = Name;
    }
}
