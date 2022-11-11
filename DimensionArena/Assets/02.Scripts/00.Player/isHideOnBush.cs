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

    private bool isAppearMoment;
    private bool doubleCheck;

    public bool isHide { get { return isAppearMoment ? !isAppearMoment : exitCnt > 0; } }

    public void AppearForMoment(float time)
    {
        if(!photonView.IsMine)
            StartCoroutine(appearRenderCoroutine(time));
    }

    [PunRPC]
    private IEnumerator appearRenderCoroutine(float time)
    {
        for (int i = 0; i < AvartarRender.Length; ++i)
            AvartarRender[i].enabled = true;

        Additional.SetActive(true);

        isAppearMoment = true;

        yield return new WaitForSeconds(time);

        isAppearMoment = false;

        if (exitCnt <= 0)
        {
            yield break;
        }
        else
        {
            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = false;

            Additional.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine && other.CompareTag("HideBush"))
            doubleCheck = true;
    }

    private void Update()
    {
        if (Additional == null)
            return;
        
        if (photonView.IsMine)
            return;
        
        if(doubleCheck && Additional.activeInHierarchy)
        {
            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = false;

            Additional.SetActive(false);

            doubleCheck = false;

        }
        else if(!doubleCheck && !Additional.activeInHierarchy)
        {
            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = true;

            Additional.SetActive(true);
        }
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
