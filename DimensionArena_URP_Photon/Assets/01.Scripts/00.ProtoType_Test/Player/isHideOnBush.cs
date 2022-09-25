using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class isHideOnBush : MonoBehaviourPun
{
    [SerializeField] private Renderer[] AvartarRender;
    [SerializeField] private GameObject Additional;

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine && other.CompareTag("Bush"))
        {
            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = false;

            Additional.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine && other.CompareTag("Bush"))
        {
            for (int i = 0; i < AvartarRender.Length; ++i)
                AvartarRender[i].enabled = true;

            Additional.SetActive(true);
        }
    }
}
