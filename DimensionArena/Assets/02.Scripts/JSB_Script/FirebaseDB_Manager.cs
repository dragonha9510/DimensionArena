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
    // �̸����� ����ִ� ���� �ĺ��� ��ȣ�� ��ȯ�ϴ� �Լ�
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
                // �ε� ���
            }
            else if (task.IsFaulted)
            {
                // �ε� ����
            }
        });
        return returnNumber;
    }
    // �ڲ� �� �Լ��� ȣ���ϴ� �͵� �׷��ϱ� , ���� ������ ������� Ȯ���ϰ� ������,,,,
    // �̰� �� �Ŀ� �����غ���.
    private void GetDB_PlayerDatas()
    {
        // �ؾ�[�����������ʤ���;������;'�����ä�����'�Ĥ�9��,����'���� �� ����� ��Ž??
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
                        // ���� ���� == ���� �����ؾ���
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
                // �ε� ���
            }
            else if (task.IsFaulted)
            {
                // �ε� ����
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


        int serializeNum;
        do
        {
            infoText.text = "���� �ĺ� ��ȣ ���� ������";
            serializeNum = Random.Range(1, 10000);
            infoText.text = "�ĺ� ��ȣ �ߺ� Ȯ����";
        } while (SerializeNumberOverlapCheck(serializeNum));

        mySerializeNumber = serializeNum;

        infoText.text = "�÷��̾� ������ ������";
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
                infoText.text = "������ �ִ� ���� �� �ҷ����� ��. . . ";
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