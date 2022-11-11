using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class isHideOnBush : MonoBehaviourPun
{
    [SerializeField] private Renderer[] AvartarRender;
    [SerializeField] private GameObject Additional;
    [SerializeField] private DetectionRange detection;

    private int exitCnt;
    private int sizeCnt;

    public bool isHide { get { return exitCnt > 0; } }

    public void AppearForMoment(float time)
    {
        StartCoroutine(appearRenderCoroutine(time));
    }

    [PunRPC]
    private IEnumerator appearRenderCoroutine(float time)
    {
        for (int i = 0; i < AvartarRender.Length; ++i)
            AvartarRender[i].enabled = true;

        Additional.SetActive(true);

        yield return new WaitForSeconds(time);

        if (exitCnt <= 0)
            yield return null;

        for (int i = 0; i < AvartarRender.Length; ++i)
            AvartarRender[i].enabled = false;

        Additional.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        //exitCnt = 0;
        //if (!photonView.IsMine && other.CompareTag("HideBush"))
        //    ++exitCnt;
    }

    private void Update()
    {
        //if (Additional == null )
        //    return;

        //if (photonView.IsMine)
        //    return;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.gameObject.CompareTag("Player_Body"))
            return;

        if (!photonView.IsMine && other.CompareTag("HideBush"))
        {
            ++exitCnt;

            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = false;

            Additional.SetActive(false);
        }
        else if (photonView.IsMine && other.CompareTag("Bush"))
        {
            if (detection == null)
                return;

            ++sizeCnt;
            detection.SetRangeMax(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!this.gameObject.CompareTag("Player_Body"))
            return;

        if (!photonView.IsMine && other.CompareTag("HideBush"))
        {
            --exitCnt;

            if (exitCnt > 0)
                return;

            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = true;

            Additional.SetActive(true);
        }
        else if (photonView.IsMine && other.CompareTag("Bush"))
        {
            if (detection == null)
                return;

            --sizeCnt;

            if (sizeCnt > 0)
                return;

            detection.SetRangeMin(); 
        }
    }
}
