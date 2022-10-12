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

        // �ε� �ؾ��� �� ũ�� ���ϱ�.
        // if ) maxWidth   =   1000
        // ex ) �� ���� ��  =   1000 _ ���� 1 ���� curGauge �� sizeDelta.x += 1f;
        // if ) maxWidth   =   400
        // ex ) �� ���� ��  =   400  _ ���� 1 ���� curGauge �� sizeDelta.x += 2.5f;
        // if ) maxWidth   =   1000
        // ex ) ����  ũ��  =   1000 .. ���� ����
        // gaugeUnit = ( maxWidth / �� ũ�� );

        StartCoroutine(loadData(SceneChanger_Loading.Instance.LoadData));
    }

    private void Update()
    {
        curGauge.rectTransform.sizeDelta = new Vector2(curWidth, curGauge.rectTransform.sizeDelta.y);
    }

    IEnumerator loadData(string dataPath)
    {
        // �ε�� ������ ���߾� curWidth = gaugeUnit * �ε�� ��;

        // Test Code
        text.text = "�ε� ����";
        gaugeUnit = 10f;
        yield return new WaitForSeconds(1.5f);
        text.text = "�ε� 1";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "�ε� 2";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "�ε� 3";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);
        text.text = "�ε� 4";
        curWidth += gaugeUnit * 25f;
        yield return new WaitForSeconds(1.5f);

        if (Mathf.Abs(curGauge.rectTransform.sizeDelta.x - maxGauge.rectTransform.sizeDelta.x) <= float.Epsilon)
        {
            // �� ��ȯ
            SceneChanger_Loading
                .Instance.ChangeScene("LoadingTest1");
        }
    }
}
