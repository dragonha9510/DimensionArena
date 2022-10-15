using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
public class TouchCanvas : MonoBehaviour
{
    public Player player;

    public void DisActiveTouch()
    {
        gameObject.SetActive(false);
    }
}
