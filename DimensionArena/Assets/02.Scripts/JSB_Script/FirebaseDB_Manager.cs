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
    // �ڲ� �� �Լ��� ȣ���ϴ� �͵� �׷��ϱ� , ���� ������ ������� Ȯ���ϰ� ������,,,,
    // �̰� �� �Ŀ� �����غ���.
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
                    Debug.Log("������ ������");

                    string key = data.Key;
                    if (playerDatas.ContainsKey(key))
                    {
                        // ���� ���� == ���� �����ؾ���
                        playerDatas[key] = (PlayerData)data.Value;
                    }
                    else
                    {
                        PlayerData newData = new PlayerData(data);


                        playerDatas.Add(key, newData);

                    }
                }
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
        // �̰͵� ����ó�� �ؾ��ϴµ�;
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
            infoText.text = "Guest �̸� ������";
            newName = defaultName += Random.Range(1, 1000).ToString();
            infoText.text = "�ߺ� Ȯ����. . .";

        } while (NameOverlapCheck(newName));

        mySerializeNumber = SystemInfo.deviceUniqueIdentifier;

        infoText.text = "�÷��̾� ������ ������";
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
                infoText.text = "������ �ִ� ���� �� �ҷ����� ��. . . ";
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
