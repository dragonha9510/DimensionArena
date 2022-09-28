using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgfTest : MonoBehaviour
{
    [SerializeField]
    private GameObject magField;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >= 1.0f)
        {
            Instantiate(magField);
            time = 0f;
        }
    }
}
