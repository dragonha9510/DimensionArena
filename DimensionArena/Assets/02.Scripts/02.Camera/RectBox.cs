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
        if(Screen.width > Screen.height)
        {
            float scale_height = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
            float scale_width = 1f / scale_height;

            if (scale_height > 1.0f)
            {
                float paddingSize = (Screen.width - 1920) * 0.5f;
                float paddingPos = 1920 * 0.5f;
                rectbox.rectTransform.sizeDelta = new Vector2(paddingSize, Screen.height);
                rectbox.rectTransform.anchoredPosition = new Vector2(paddingPos + paddingSize * 0.5f, 0);
                rectbox2.rectTransform.sizeDelta = new Vector2(paddingSize, Screen.height);
                rectbox2.rectTransform.anchoredPosition = new Vector2(-1 * (paddingPos + paddingSize * 0.5f), 0);

            }
            else
            {
                float paddingSize = (Screen.height - 1080) * 0.5f;
                float paddingPos = 1080 * 0.5f;
                rectbox.rectTransform.sizeDelta = new Vector2(Screen.width, paddingSize);
                rectbox.rectTransform.anchoredPosition = new Vector2(0, paddingPos + paddingSize * 0.5f);
                rectbox2.rectTransform.sizeDelta = new Vector2(Screen.width, paddingSize);
                rectbox2.rectTransform.anchoredPosition = new Vector2(0, -1 * (paddingPos + paddingSize * 0.5f));

            }
        }
        else
        {
            /*
            float scale_height = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
            float scale_width = 1f / scale_height;

            if (scale_height > 1.0f)
            {
                float paddingSize = (Screen.width - 1920) * 0.5f;
                float paddingPos = 1920 * 0.5f;
                rectbox.rectTransform.sizeDelta = new Vector2(paddingSize, Screen.height);
                rectbox.rectTransform.anchoredPosition = new Vector2(paddingPos + paddingSize * 0.5f, 0);
                rectbox2.rectTransform.sizeDelta = new Vector2(paddingSize, Screen.height);
                rectbox2.rectTransform.anchoredPosition = new Vector2(-1 * (paddingPos + paddingSize * 0.5f), 0);

            }
            else
            {
                float paddingSize = (Screen.height - 1080) * 0.5f;
                float paddingPos = 1080 * 0.5f;
                rectbox.rectTransform.sizeDelta = new Vector2(Screen.width, paddingSize);
                rectbox.rectTransform.anchoredPosition = new Vector2(0, paddingPos + paddingSize * 0.5f);
                rectbox2.rectTransform.sizeDelta = new Vector2(Screen.width, paddingSize);
                rectbox2.rectTransform.anchoredPosition = new Vector2(0, -1 * (paddingPos + paddingSize * 0.5f));

            }
            */
        }
            
              
      
    }
}
