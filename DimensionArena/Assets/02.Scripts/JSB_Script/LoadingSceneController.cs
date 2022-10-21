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
        loadInfoText.text = "씬 불러오는 중...";
        StartCoroutine("LoadSceneProcess");
    }

    IEnumerator LoadSceneProcess()
    {
        //비 동기 방식 씬 불러오기 , 씬을 불러오는 중 다른 작업이 가능하다.
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
     

        // Fake 씬 로딩을 위한 설정 , 씬이 로딩되면 바로 넘어가지 않도록 하는 설정.
        // 추 후 에셋번들로 나누게 되면 리소스를 따로 관리하기 때문에 이런 설정을 하기도 한다.
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
                // Time Scale 에 영향을 받지 않는 델타타임.
                timer += Time.unscaledDeltaTime;
                if(howLongFake <= timer)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            loadInfoText.text = "불러오는중..." + (op.progress * 100f).ToString() + "%";
            loadSlider.value = op.progress;
        }
    }

}
