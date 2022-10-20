using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System;

using Photon.Pun;

using Random = UnityEngine.Random;

public class FirebaseDB_Manager : MonoBehaviourPun
{
    public static FirebaseDB_Manager Instance;

    DatabaseReference DB_reference;


    [SerializeField] Dictionary<string, PlayerData> playerDatas = new Dictionary<string, PlayerData>();
    
    private string defaultName = "Guest";
    private string mySerializeNumber = "";

    private bool isRefresh = false;
    private bool isRerfeshing = false;

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
    // 자꾸 이 함수를 호출하는 것도 그러니까 , 값이 변경이 됬는지를 확인하고 싶은데,,,,
    // 이건 추 후에 생각해보자.
    public void GetDB_PlayerDatas()
    {
        isRerfeshing = true;
        bool test = PhotonNetwork.IsConnected;

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
                        playerDatas[key] = (PlayerData)data.Value;
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
            isRerfeshing = false;
        });
    }
    private void Update()
    {
        {
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

    public void ChangeNickName(string name)
    {
        // 이것도 예외처리 해야하는데;
        PlayerData willchangeData = playerDatas[mySerializeNumber];

        willchangeData.playerName = name;
        string json = JsonUtility.ToJson(willchangeData);
        DB_reference.Child(mySerializeNumber.ToString()).SetRawJsonValueAsync(json);

    }
    private string RegisterNewPlayer(TextMeshProUGUI infoText)
    {
        string newName;
        do
        {
            infoText.text = "Guest 이름 생성중";
            newName = defaultName += Random.Range(1, 1000).ToString();
            infoText.text = "중복 확인중. . .";

        } while (NameOverlapCheck(newName));

        mySerializeNumber = SystemInfo.deviceUniqueIdentifier;

        infoText.text = "플레이어 데이터 생성중";
        PlayerData newData = new PlayerData(newName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child(mySerializeNumber).SetRawJsonValueAsync(json);


        return newName;
    }
    public string RegisterDataBase(TextMeshProUGUI infoText)
    {
        string name = "";
        foreach(string Key in playerDatas.Keys)
        {
            // 0 is not used
            if(Key == SystemInfo.deviceUniqueIdentifier)
            {
                infoText.text = "기존에 있던 정보 값 불러오는 중. . . ";
                name = playerDatas[Key].playerName;
                mySerializeNumber = Key;
                break;
            }
        }
        if(name == "")
        {
            name = RegisterNewPlayer(infoText);
        }
        return name;
    }
}
