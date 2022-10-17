using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
public class FirebaseDB_Manager : MonoBehaviour
{
    public static FirebaseDB_Manager Instance;

    DatabaseReference DB_reference;

    [SerializeField] private List<PlayerData> playerNames = new List<PlayerData>();

    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(this.gameObject);

        DB_reference = FirebaseDatabase.DefaultInstance.RootReference;

    }


    public void WritePlayerNameData(string userName)
    {
        PlayerData newData = new PlayerData(userName);
        string json = JsonUtility.ToJson(newData);
        DB_reference.Child("Player").Child(userName).SetRawJsonValueAsync(json);
    }

    public List<PlayerData> GetPlayerNameList()
    {
        DatabaseReference dtr = FirebaseDatabase.DefaultInstance.GetReference("Player");
        dtr.GetValueAsync().ContinueWith(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    playerNames.Add((PlayerData)data.Value);
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

        return playerNames;
    }

}
