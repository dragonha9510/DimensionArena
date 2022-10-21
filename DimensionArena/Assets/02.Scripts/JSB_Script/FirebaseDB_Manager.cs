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

    public string PlayerNickName { get { return playerNickName; } }

    private bool isRefreshing = false;

    private bool isInGame = false;
    public bool IsInGame { set { isInGame = value; } }

    private int refreshCount = 0;
    public int RefreshCount { get { return refreshCount; } }

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
    // �ڲ� �� �Լ��� ȣ���ϴ� �͵� �׷��ϱ� , ���� ������ ������� Ȯ���ϰ� ������,,,,
    // �̰� �� �Ŀ� �����غ���.
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
                    Debug.Log("������ ������");

                    string key = data.Key;
                    if (playerDatas.ContainsKey(key))
                    {
                        // ���� ���� == ���� �����ؾ���
                        playerDatas[key] = new PlayerData(data);
                    }
                    else
                    {
                        PlayerData newData = new PlayerData(data);
                        playerDatas.Add(key, newData);
                    }
                }
                ++refreshCount;
                Debug.Log("���� ��");
            }
            else if (task.IsCanceled)
            {
                // �ε� ���
            }
            else if (task.IsFaulted)
            {
                // �ε� ����
            }
            isRefreshing = false;
        });
    }
    private void Update()
    {
        // �켱�� �ӽ������� ���Ƴ��´�.
        if (isRefreshing || isInGame)
        {
            Debug.Log("������ �Լ� ȣ�� ����");
            return;
        }
        else
        {
            Debug.Log("���� �Լ� ȣ��");
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
        // �̰͵� ����ó�� �ؾ��ϴµ�;
        PlayerData willchangeData = playerDatas[mySerializeNumber];

        willchangeData.playerName = name;
        string json = JsonUtility.ToJson(willchangeData);
        DB_reference.Child(mySerializeNumber.ToString()).SetRawJsonValueAsync(json);

    }
    public void RegisterNewPlayer(string newName)
    {
        playerNickName = newName;
        mySerializeNumber = SystemInfo.deviceUniqueIdentifier;
        PlayerData newData = new PlayerData(newName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child(mySerializeNumber).SetRawJsonValueAsync(json);
    }
    // ���� ������ �����Ͱ� �ִٸ� true �� ��ȯ
    public bool RegisterDataBase()
    {
        
        bool dataAlreadyIn = false;
        foreach(string Key in playerDatas.Keys)
        {
            // 0 is not used
            if(Key == SystemInfo.deviceUniqueIdentifier)
            {
                dataAlreadyIn = true;
                playerNickName = playerDatas[Key].playerName;
                mySerializeNumber = Key;
                break;
            }
        }
        return dataAlreadyIn;
    }


}
