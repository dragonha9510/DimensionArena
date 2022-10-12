using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCanvas : MonoBehaviour
{
    public Player player;

    public void DisActiveTouch()
    {
        gameObject.SetActive(false);
    }
}
