using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCanvas : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        player.Info.EDisActivePlayer += DisActiveTouch;
    }

    private void DisActiveTouch()
    {
        gameObject.SetActive(false);
    }
}
