using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaintainScale : MonoBehaviour
{
    private float width = 1920;
    private float height = 1118;
    private float curWidth;
    private float curHeight;
    [SerializeField] private bool maintainRatio;
    private Vector3 ratio;

    private void Start()
    {
        curWidth = width;
        curHeight = height;
        ratio = transform.localScale;
    }

    void Update()
    {
        if(!Mathf.Approximately(curWidth, Screen.width) || !Mathf.Approximately(curHeight, Screen.height))
        {
            transform.localScale = new Vector3(Screen.width / width, Screen.height / height, 1);
            curWidth = Screen.width;
            curHeight = Screen.height;

            if (maintainRatio)
            {
                float maxValue = transform.localScale.x < transform.localScale.y ? transform.localScale.x : transform.localScale.y;
                transform.localScale = new Vector3(ratio.x * maxValue, ratio.y * maxValue, ratio.z * maxValue);
            }
        }
    }
}
