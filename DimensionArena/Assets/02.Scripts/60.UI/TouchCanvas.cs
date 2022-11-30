using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
using UnityEngine.UI;

public class TouchCanvas : MonoBehaviour
{
    public Player player;
    [SerializeField] Image touchScreen;
    [SerializeField] Image attackScreen;

    private void Start()
    {
        float width = Screen.width;
        float padding = (width * 0.25f); //1320
        float height = Screen.height;
        touchScreen.rectTransform.anchoredPosition = new Vector2(-(padding), 0);
        attackScreen.rectTransform.anchoredPosition = new Vector2(padding, 0);
        touchScreen.rectTransform.sizeDelta = new Vector2(width * 0.48f, height);
        attackScreen.rectTransform.sizeDelta = new Vector2(width * 0.48f, height);

    }


    public void DisActiveTouch()
    {
        gameObject.SetActive(false);
    }
}
