using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger_Loading
{
    // 로딩 -> 메인
    // 큐 -> 로딩 -> 인게임
    private static SceneChanger_Loading instance;
    public static SceneChanger_Loading Instance
    {
        get
        {
            if (instance == null)
                instance = new SceneChanger_Loading();

            return instance;
        }
    }

    public string curScene;
    public string nextScene;

    private string loaddata; // string which for load data path
    public string LoadData { get { return loaddata; } private set { loaddata = value; } } 

    private void SetCurScene()
    {
        curScene = nextScene;
    }

    public void ChangeSceneWithLoad(string next, string datapath)
    {
        LoadData = datapath;
        nextScene = next;
        SceneManager.LoadScene("LoadingScene");
    }

    public void ChangeScene(string next = null)
    {
        if (string.IsNullOrEmpty(next))
            next = nextScene;
        else
            nextScene = next;

        SceneManager.LoadScene(next);
    }
}
