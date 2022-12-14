using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudeEffect : MonoBehaviour
{
    [SerializeField]
    private float minScale = 0.1f;
    [SerializeField]
    private float maxScale = 0.6f;
    [SerializeField]
    private float correctionValue = 1.0f;
    [SerializeField]
    private float alliveTime = 5.0f;

    private float liveTime = 0.0f;

    public bool UnDead = false;

    WaitForSeconds waitforSeconds = new WaitForSeconds(1.05f);
    private bool upScale = false;

    public void StartEffect()
    {
        liveTime = alliveTime;
        float randScale = Random.Range(minScale, maxScale);
        this.transform.localScale = new Vector3(randScale, randScale, 1);
        StartCoroutine(ScaleUpdate());
    }

    IEnumerator ScaleUpdate()
    {
        while (true)
        {
            if (upScale)
                transform.DOScale(Vector3.one, 1f);
            
            else
                transform.DOScale(Vector3.one * 0.4f, 1f);

            yield return waitforSeconds;
            upScale = !upScale;
        }
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameEnd)
        {
            Destroy(this.gameObject);
            return;
        }
        liveTime -= Time.deltaTime;
        
        if (0 > liveTime && false == UnDead)
        {
            ObjectPool.Instance.ReturnObjectToPool(CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT, this.gameObject);
        }
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




}
