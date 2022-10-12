using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{

    [SerializeField]
    GameObject optionCanvas;



    public void CanvasOff()
    {
        optionCanvas.SetActive(false);
    }

    public void OptionCanvasOn()
    {
        optionCanvas.SetActive(true);
    }

    public void OptionCanvasOff()
    {
        optionCanvas.SetActive(false);
    }
}
