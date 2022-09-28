using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CloudeEffect : MonoBehaviour
{
    [SerializeField]
    private float minScale = 0.1f;
    [SerializeField]
    private float maxScale = 1.0f;
    [SerializeField]
    private float correctionValue = 0.01f;
    [SerializeField]
    private float alliveTime = 5.0f;


    WaitForSeconds waitforSeconds = new WaitForSeconds(0.01f);
    private bool upScale = false;

    private void Start()
    {
        float randScale = Random.Range(minScale, maxScale);
        this.transform.localScale = new Vector3(randScale, randScale, 1);
        StartCoroutine("ScaleUpdate");
    }
    private void SizeUp()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x + correctionValue, this.transform.localScale.y + correctionValue, 1);
        if (this.transform.localScale.x > maxScale)
            upScale = false;
    }
    private void SizeDown()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x - correctionValue, this.transform.localScale.y - correctionValue, 1);
        if (this.transform.localScale.x < minScale)
            upScale = true;
    }
    
    IEnumerator ScaleUpdate()
    {
        while(true)
        {
            if (upScale)
                SizeUp();
            else
                SizeDown();
            yield return waitforSeconds;
        }
    }
    private void FixedUpdate()
    {
        alliveTime -= Time.deltaTime;
        if (0 > alliveTime)
            Destroy(this.gameObject);
    }
}
