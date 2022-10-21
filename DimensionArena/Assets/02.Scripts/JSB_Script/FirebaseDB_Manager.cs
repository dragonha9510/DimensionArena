using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;
using Photon.Pun;

using Random = UnityEngine.Random;

public class FirebaseDB_Manager : MonoBehaviourPun
{
    public static FirebaseDB_Manager Instance;

    DatabaseReference DB_reference;


    [SerializeField] Dictionary<string, PlayerData> playerDatas = new Dictionary<string, PlayerData>();
    
    private string mySerializeNumber = "";

    private string playerNickName = "";

    public string PlayerNickName { get { return playerNickName; } set { playerNickName = value; } }

    private bool isRefreshing = false;

    private bool isInGame = false;
    public bool IsInGame { set { isInGame = value; } }

    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        DB_reference = FirebaseDatabase.DefaultInstance.GetReference("SerializeNumber");

    }
    // 자꾸 이 함수를 호출하는 것도 그러니까 , 값이 변경이 됬는지를 확인하고 싶은데,,,,
    // 이건 추 후에 생각해보자.
    public void GetDB_PlayerDatas()
    {
        isRefreshing = true;

        FirebaseDatabase.DefaultInstance.GetReference("SerializeNumber").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    Debug.Log("데이터 갱신중");

                    string key = data.Key;
                    if (playerDatas.ContainsKey(key))
                    {
                        // 값이 있음 == 값을 갱신해야함
                        playerDatas[key] = new PlayerData(data);
                    }
                    else
                    {
                        PlayerData newData = new PlayerData(data);
                        playerDatas.Add(key, newData);
                    }
                }
                Debug.Log("갱신 끝");
            }
            else if (task.IsCanceled)
            {
                // 로드 취소
            }
            else if (task.IsFaulted)
            {
                // 로드 실패
            }
            isRefreshing = false;
        });
    }
    private void Update()
    {
        // 우선은 임시적으로 막아놓는다.
        if (isRefreshing || isInGame)
        {
            Debug.Log("갱신중 함수 호출 안함");
            return;
        }
        else
        {
            Debug.Log("갱신 함수 호출");
            GetDB_PlayerDatas();
        }
    }

    public bool NameOverlapCheck(string name)
    {
        foreach (string key in playerDatas.Keys)
        {
            if (playerDatas[key].playerName == name)
                return true;
        }
        return false;
    }

    public void ReWriteData(string name)
    {
        // 이것도 예외처리 해야하는데;
        PlayerData willchangeData = playerDatas[mySerializeNumber];

        willchangeData.playerName = name;
        string json = JsonUtility.ToJson(willchangeData);
        DB_reference.Child(mySerializeNumber.ToString()).SetRawJsonValueAsync(json);

    }
    private string RegisterNewPlayer(string newName)
    {
        mySerializeNumber = SystemInfo.deviceUniqueIdentifier;
        PlayerData newData = new PlayerData(newName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child(mySerializeNumber).SetRawJsonValueAsync(json);

        return newName;
    }
    public string RegisterDataBase(string newName)
    {
        string name = "";
        foreach(string Key in playerDatas.Keys)
        {
            // 0 is not used
            if(Key == SystemInfo.deviceUniqueIdentifier)
            {
                name = playerDatas[Key].playerName;
                mySerializeNumber = Key;
                break;
            }
        }
        if(name == "")
        {
            name = RegisterNewPlayer(newName);
        }
        return name;
    }
}
