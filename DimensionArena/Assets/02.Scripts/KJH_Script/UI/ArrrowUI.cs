using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrrowUI : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float interval = 1;
    [SerializeField, Range(0, 0.5f)] private float minSpeed = 0.1f;

    private float max = 3;
    private float min = 2;
    private bool down;

    void Start()
    {
        max = min + interval;
    }

    void Update()
    {
        float positionY = Mathf.Abs(max - transform.position.y);
        transform.position =
            transform.position +
            new Vector3(0, (positionY <= minSpeed ? minSpeed : positionY) * Time.deltaTime * (down ? -1 : 1), 0);

        Debug.Log(positionY);

        if (transform.position.y >= max)
            down = true; 
        if (transform.position.y <= min)
            down = false;
    }
}
