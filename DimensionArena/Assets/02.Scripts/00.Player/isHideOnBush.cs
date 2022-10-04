using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class isHideOnBush : MonoBehaviourPun
{
    [SerializeField] private Renderer[] AvartarRender;
    [SerializeField] private GameObject Additional;
    [SerializeField] private DitectionRange ditection;

    private int exitCnt;
    private int sizeCnt;

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
            if (ditection == null)
                return;

            ++sizeCnt;
            ditection.SetRangeMax(); 
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
            if (ditection == null)
                return;

            --sizeCnt;

            if (sizeCnt > 0)
                return;

            ditection.SetRangeMin(); 
        }
    }
}
