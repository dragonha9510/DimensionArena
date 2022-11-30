using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RectBox : MonoBehaviour
{
    [SerializeField] Image rectbox;
    [SerializeField] Image rectbox2;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        // ���� ���� ��� 9:16, �ݴ�� �ϰ� ������ 16:9�� �Է�.
        float scale_height = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (���� / ����)
        float scale_width = 1f / scale_height;

        if (scale_height < 1)
        {
            Debug.Log("���ΰ� �� ����");
            Debug.Log(scale_width);
            Debug.Log(scale_height);

            float ratio = (scale_width - 1.0f) * 0.5f; 
            rectbox.rectTransform.sizeDelta = new Vector2(Screen.width + 0.1f, ratio * Screen.height + 0.1f);
            rectbox2.rectTransform.sizeDelta = new Vector2(Screen.width + 0.1f, ratio * Screen.height + 0.1f);

            rectbox.rectTransform.anchoredPosition = new Vector2(0,(1080) * 0.5f + rectbox.rectTransform.sizeDelta.y * 0.5f );
            rectbox2.rectTransform.anchoredPosition = new Vector2(0,-1 * ((1080) * 0.5f + rectbox.rectTransform.sizeDelta.y * 0.5f));
        }
        else
        {
            Debug.Log("���ΰ� �� ŭ");
            Debug.Log(scale_width);
            Debug.Log(scale_height);
            float ratio = (scale_height - 1.0f) * 0.5f;
            rectbox.rectTransform.sizeDelta = new Vector2(ratio * Screen.width + 0.1f, Screen.height + 0.1f);
            rectbox2.rectTransform.sizeDelta = new Vector2(ratio * Screen.width + 0.1f, Screen.height + 0.1f);

            rectbox.rectTransform.anchoredPosition = new Vector2((Screen.width * scale_width) * 0.5f + rectbox.rectTransform.sizeDelta.x * 0.5f,  0);
            rectbox2.rectTransform.anchoredPosition = new Vector2(-1 * ((Screen.width * scale_width) * 0.5f + rectbox.rectTransform.sizeDelta.x * 0.5f), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
