using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class isHideOnBush : MonoBehaviourPun
{
    [SerializeField] private Renderer[] AvartarRender;
    [SerializeField] private GameObject Additional;

    private int exitCnt;

    private void OnTriggerEnter(Collider other)
    {
        if (!this.gameObject.CompareTag("Player"))
            return;

        if (!photonView.IsMine && other.CompareTag("HideBush"))
        {
            ++exitCnt;

            //if(other.gameObject.GetComponent<MeshRenderer>().material.color.a < 1)
            //{
            //    for (int i = 0; i < AvartarRender.Length; ++i)
            //        AvartarRender[i].enabled = true;

            //    Additional.SetActive(true);
            //    return;
            //}

            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = false;

            Additional.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!this.gameObject.CompareTag("Player"))
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
    }
}
