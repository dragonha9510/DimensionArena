using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;

using System;

using Random = UnityEngine.Random;

public class FirebaseDB_Manager : MonoBehaviour
{
    public static FirebaseDB_Manager Instance;

    DatabaseReference DB_reference;


    [SerializeField] Dictionary<int, PlayerData> playerDatas = new Dictionary<int, PlayerData>();
    
    private string defaultName = "Guest";
    private int mySerializeNumber = 0;


    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(this.gameObject);

        DB_reference = FirebaseDatabase.DefaultInstance.GetReference("SerializeNumber");

    }

    private void GetDB_PlayerDatas()
    {
        DB_reference = FirebaseDatabase.DefaultInstance.GetReference("SerializeNumber");

        DB_reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    int key;
                    Int32.TryParse(data.Key, out key);
                    if(playerDatas.ContainsKey(key))
                    {
                        // 값이 있음 == 값을 갱신해야함
                        playerDatas[key] = (PlayerData)data.Value;
                    }
                    else
                    {
                        PlayerData newData = new PlayerData(data);


                        playerDatas.Add(key,newData);
                    }
                }
            }
            else if (task.IsCanceled)
            {
                // 로드 취소
            }
            else if (task.IsFaulted)
            {
                // 로드 실패
            }
        });
    }


    public bool NameOverlapCheck(string name)
    {
        GetDB_PlayerDatas();
        foreach (int key in playerDatas.Keys)
        {
            if (playerDatas[key].playerName == name)
                return true;
        }
        return false;
    }

    private bool SerializeNumberOverlapCheck(int number)
    {
        // 이 함수는 어차피 네임 체크 후 들어감으로 고유 번호에서는 굳이 갱신을 하지 않는다.
        // 추 후 재사용성을 고려하면 해야하지만 , 해당 함수는 오직 딱 한번 실행 된다.
        foreach (int key in playerDatas.Keys)
        {
            if (key == number)
                return true;
        }
        return false;
    }

    public void ChangeNickName(string name)
    {
        // 이 함수는 어차피 네임 체크 후 들어감으로 고유 번호에서는 굳이 갱신을 하지 않는다.
        // 추 후 재사용성을 고려하면 해야하지만 , 해당 함수는 오직 딱 한번 실행 된다.

        // 이것도 예외처리 해야하는데;
        PlayerData willchangeData = playerDatas[mySerializeNumber];

        willchangeData.playerName = name;
        string json = JsonUtility.ToJson(willchangeData);
        DB_reference.Child(mySerializeNumber.ToString()).SetRawJsonValueAsync(json);

    }

    public string RegisterJustOnce(TextMeshProUGUI infoText)
    {
        string newName;
        do
        {
            infoText.text = "Guest 이름 생성중";
            newName = defaultName += Random.Range(1, 1000).ToString();
            infoText.text = "중복 확인중. . .";

        } while (NameOverlapCheck(newName));


        int serializeNum;

        do
        {
            infoText.text = "고유 식별 번호 랜덤 생성중";
            serializeNum = Random.Range(1, 10000);
            infoText.text = "식별 번호 중복 확인중";
        }while(SerializeNumberOverlapCheck(serializeNum));

        mySerializeNumber = serializeNum;

        infoText.text = "플레이어 데이터 생성중";
        PlayerData newData = new PlayerData(newName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child(serializeNum.ToString()).SetRawJsonValueAsync(json);

        return newName;
    }


}
