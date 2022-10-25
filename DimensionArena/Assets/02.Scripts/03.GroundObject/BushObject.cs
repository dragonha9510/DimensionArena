using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BushObject : GroundObject
{
    private BoxCollider myCollider;
    private MeshRenderer _renderer;
    [SerializeField] private Material opaque;
    [SerializeField] private Material transparent;
    [SerializeField] private float bushAlpha;
    private Color oriColor;
    private Vector3 oriPos;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        myCollider = transform.GetChild(0).GetComponent<BoxCollider>();

        oriPos = myCollider.transform.position;
        //oriColor = _renderer.material.color;
        oriColor = _renderer.material.GetColor("_BaseColor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Ditection"))
        {
            PhotonView temp = other.gameObject.GetComponentInParent<PhotonView>();

            if (temp != null && temp.IsMine == false)
                return;

            myCollider.transform.position = oriPos + new Vector3(0, 20, 0);

            //_renderer.material.color = oriColor * new Color(1, 1, 1, bushAlpha);
            _renderer.material = transparent;
            _renderer.material.SetColor("_BaseColor", oriColor * new Color(1, 1, 1, bushAlpha));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player_Ditection"))
        {
            PhotonView temp = other.gameObject.GetComponentInParent<PhotonView>();

            if (temp != null && temp.IsMine == false)
                return;

            myCollider.transform.position = oriPos;

            //_renderer.material.color = oriColor; 
            _renderer.material.SetColor("_BaseColor", oriColor);
            _renderer.material = opaque;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player_Enter");
    }
}
