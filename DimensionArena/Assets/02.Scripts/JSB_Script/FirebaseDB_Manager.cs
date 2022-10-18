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
    // 이름값을 들고있는 고유 식별자 번호를 반환하는 함수
    private int FindSerializeNumber(string playerName)
    {
        int returnNumber = 0;

        DB_reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    if (data.Child("playerName").ToString() == playerName)
                    {
                        Int32.TryParse(data.Key, out returnNumber);
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
        return returnNumber;
    }
    // 자꾸 이 함수를 호출하는 것도 그러니까 , 값이 변경이 됬는지를 확인하고 싶은데,,,,
    // 이건 추 후에 생각해보자.
    private void GetDB_PlayerDatas()
    {
        // 왠앨[ㅁ나레ㅐㅁ너ㅏㄷ;ㅐㄱ러;'ㅁ내ㅓㄷㄱㅎ'ㅔㅁ9댜,ㄱ헤'여기 왜 제대로 안탐??
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
        GetDB_PlayerDatas();

        foreach (int key in playerDatas.Keys)
        {
            if (key == number)
                return true;
        }
        return false;
    }


    public void ChangeNickName(string name)
    {
        GetDB_PlayerDatas();


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


        int serializeNum;
        do
        {
            infoText.text = "고유 식별 번호 랜덤 생성중";
            serializeNum = Random.Range(1, 10000);
            infoText.text = "식별 번호 중복 확인중";
        } while (SerializeNumberOverlapCheck(serializeNum));

        mySerializeNumber = serializeNum;

        infoText.text = "플레이어 데이터 생성중";
        PlayerData newData = new PlayerData(newName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child(serializeNum.ToString()).SetRawJsonValueAsync(json);

        return newName;
    }
    public string RegisterDataBase(TextMeshProUGUI infoText)
    {
        GetDB_PlayerDatas();
        GetDB_PlayerDatas();

        string name = "";
        foreach(int data in playerDatas.Keys)
        {
            // 0 is not used
            if(playerDatas[data].deviceIdentifier == SystemInfo.deviceUniqueIdentifier)
            {
                infoText.text = "기존에 있던 정보 값 불러오는 중. . . ";
                mySerializeNumber = FindSerializeNumber(playerDatas[data].playerName);
                name = playerDatas[data].playerName;
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
