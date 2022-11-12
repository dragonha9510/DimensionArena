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
    private float curAlpha = 1;
    private Color oriColor;
    private Vector3 oriPos;

    private bool curGradation;
    private bool stopGradation;
    [SerializeField] private float gradationSpeed = 2.0f;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        myCollider = transform.GetChild(0).GetComponent<BoxCollider>();

        oriPos = myCollider.transform.position;
        //oriColor = _renderer.material.color;
        oriColor = _renderer.material.GetColor("_BaseColor");
    }

    private void Update()
    {
        if (curGradation == stopGradation)
            return;

        if(curGradation)
        {
            _renderer.material = transparent;
        }

        curAlpha += (curGradation ? -1 : 1) * gradationSpeed * Time.deltaTime;

        _renderer.material.SetColor("_BaseColor", oriColor * new Color(0.4f, 0.5f, 0.12f, curAlpha));

        if(curAlpha <= bushAlpha)
        {
            curAlpha = bushAlpha;
            stopGradation = curGradation;
        }
        else if(curAlpha >= 1)
        {
            curAlpha = 1;
            _renderer.material.SetColor("_BaseColor", oriColor);
            _renderer.material = opaque;
            stopGradation = curGradation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Detection"))
        {
            PhotonView temp = other.gameObject.GetComponentInParent<PhotonView>();

            if (temp != null && temp.IsMine == false)
                return;

            curGradation = true;
            stopGradation = false;

            myCollider.transform.position = oriPos + new Vector3(0, 20, 0);

            //_renderer.material.color = oriColor * new Color(1, 1, 1, bushAlpha);
            //_renderer.material = transparent;
            //_renderer.material.SetColor("_BaseColor", oriColor * new Color(1, 1, 1, bushAlpha));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player_Detection"))
        {
            PhotonView temp = other.gameObject.GetComponentInParent<PhotonView>();

            if (temp != null && temp.IsMine == false)
                return;

            curGradation = false;
            stopGradation = true;

            myCollider.transform.position = oriPos;

            //_renderer.material.SetColor("_BaseColor", oriColor);
            //_renderer.material = opaque;
        }
    }
}
