using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image curGauge;
    [SerializeField] private Image maxGauge;
    private float maxWidth;
    private float curWidth;
    private float gaugeUnit;

    private void Start()
    {
        maxWidth = maxGauge.rectTransform.sizeDelta.x;
        curGauge.rectTransform.sizeDelta = new Vector2(0, curGauge.rectTransform.sizeDelta.y);

        // 로드 해야할 총 크기 구하기.
        // if ) maxWidth   =   1000
        // ex ) 총 파일 수  =   1000 _ 파일 1 개당 curGauge 의 sizeDelta.x += 1f;
        // if ) maxWidth   =   400
        // ex ) 총 파일 수  =   400  _ 파일 1 개당 curGauge 의 sizeDelta.x += 2.5f;
        // if ) maxWidth   =   1000
        // ex ) 파일  크기  =   1000 .. 위와 동일
        // gaugeUnit = ( maxWidth / 총 크기 );

        StartCoroutine(loadData(SceneChanger_Loading.Instance.LoadData));
    }

    private void Update()
    {
        curGauge.rectTransform.sizeDelta = new Vector2(curWidth, curGauge.rectTransform.sizeDelta.y);
    }

    IEnumerator loadData(string dataPath)
    {
        // 로드된 단위에 맞추어 curWidth = gaugeUnit * 로드된 양;

        // Test Code
        text.text = "로딩 시작";
        gaugeUnit = 10f;
        yield return new WaitForSeconds(1.5f);
        text.text = "로딩 1";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "로딩 2";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "로딩 3";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "로딩 4";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);

        if (Mathf.Abs(curGauge.rectTransform.sizeDelta.x - maxGauge.rectTransform.sizeDelta.x) <= float.Epsilon)
        {
            // 씬 전환
            SceneChanger_Loading
                .Instance.ChangeScene("LoadingTest1");
        }
    }
}
