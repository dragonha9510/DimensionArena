using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    static public OptionManager Instance;

    [SerializeField]
    GameObject optionCanvas;


    private void Awake()
    {
        if (null == Instance)
        {
            Instance = new OptionManager();
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
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
