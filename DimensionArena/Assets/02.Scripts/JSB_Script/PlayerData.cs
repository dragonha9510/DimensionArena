using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
public class PlayerData
{
    public string playerName = "";
    public int[] winCount = new int[(int)MODE.MODE_TRAINING] { 0, 0, 0, 0 };
    public int[] loseCount = new int[(int)MODE.MODE_TRAINING] { 0, 0, 0, 0 };
    public int killCount = 0;
    public int deathCount = 0;
    public int[] liveTime = new int[(int)MODE.MODE_TRAINING] { 0, 0, 0, 0 };
    public int totalPlayTime = 0;
    public string playCharacter = "";

    public PlayerData(string Name)
    {
        playerName = Name;
        playCharacter = Define.JOOHYEOK;
    }
    // 꼭 굳이 이렇게 생성자를 만들어야 할까.... IDictionary 로 대부분 그냥 단순 자식의 값만 받아오고...
    // 전체를 알아서 불러 올 수는 없는걸까
    public PlayerData(DataSnapshot data)
    {
        playerName = data.Child("playerName").Value.ToString();
        
        int i = 0;
        DataSnapshot winCountData = data.Child("winCount");
        foreach (DataSnapshot numberChild in winCountData.Children)
        {
            Int32.TryParse(numberChild.Value.ToString(), out winCount[i++]);
        }

        i = 0;
        DataSnapshot loseCountData = data.Child("loseCount");
        foreach (DataSnapshot numberChild in loseCountData.Children)
        {
            Int32.TryParse(numberChild.Value.ToString(), out loseCount[i++]);
        }


        Int32.TryParse(data.Child("killCount").Value.ToString(), out killCount);

        Int32.TryParse(data.Child("deathCount").Value.ToString(), out deathCount);

        i = 0;
        DataSnapshot liveTimeData = data.Child("liveTime");
        foreach (DataSnapshot numberChild in liveTimeData.Children)
        {
            Int32.TryParse(numberChild.Value.ToString(), out liveTime[i++]);
        }

        Int32.TryParse(data.Child("totalPlayTime").Value.ToString(), out totalPlayTime);

        // 임시 예외처리
        //return;
        playCharacter = data.Child("playCharacter").Value.ToString();

    }
}
