using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadingSceneController : MonoBehaviour
{

    static private LoadingSceneController instance;

    static public LoadingSceneController Instance { get { return instance; } }


    [SerializeField] TextMeshProUGUI loadInfoText;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(this.gameObject);
    }


    string nextScene;

    [SerializeField]
    Slider loadSlider;
    [SerializeField]
    float fakeTiming = 0.9f;
    [SerializeField]
    float howLongFake = 5.0f;



    public void LoadScene(string sceneName)
    {

        nextScene = sceneName;
        loadInfoText.text = "�� �ҷ����� ��...";
        StartCoroutine("LoadSceneProcess");
    }

    IEnumerator LoadSceneProcess()
    {
        //�� ���� ��� �� �ҷ����� , ���� �ҷ����� �� �ٸ� �۾��� �����ϴ�.
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
     

        // Fake �� �ε��� ���� ���� , ���� �ε��Ǹ� �ٷ� �Ѿ�� �ʵ��� �ϴ� ����.
        // �� �� ���¹���� ������ �Ǹ� ���ҽ��� ���� �����ϱ� ������ �̷� ������ �ϱ⵵ �Ѵ�.
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < fakeTiming)
            {
            }
            else
            {
                // Time Scale �� ������ ���� �ʴ� ��ŸŸ��.
                timer += Time.unscaledDeltaTime;
                if(howLongFake <= timer)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            loadInfoText.text = "�ҷ�������..." + (op.progress * 100f).ToString() + "%";
            loadSlider.value = op.progress;
        }
    }

}
