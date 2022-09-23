using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScale : MonoBehaviour
{
    [SerializeField]
    private float minScale = 0.1f;
    [SerializeField]
    private float maxScale = 1.0f;
    [SerializeField]
    private float correctionValue = 0.01f;
    private float time = 0.0f;

    private bool upScale = false;
    private void SizeUp()
    {
        Debug.Log("늘이기");
        this.transform.localScale = new Vector3(this.transform.localScale.x + correctionValue, this.transform.localScale.y + correctionValue, 1);
        if (this.transform.localScale.x > maxScale)
            upScale = false;
    }
    private void SizeDown()
    {
        Debug.Log("줄이기");
        this.transform.localScale = new Vector3(this.transform.localScale.x - correctionValue, this.transform.localScale.y - correctionValue, 1);
        if (this.transform.localScale.x < minScale)
            upScale = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (upScale)
            SizeUp();
        else
            SizeDown();
    }
}
