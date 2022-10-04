using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BushObject : GroundObject
{
    [SerializeField] private BoxCollider myCollider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float bushAlpha;
    private Color oriColor;
    private Vector3 oriPos;

    private void Start()
    {
        oriPos = myCollider.transform.position;
        oriColor = _renderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Ditection") && other.gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            myCollider.transform.position = oriPos + new Vector3(0, 20, 0);

            _renderer.material.color = oriColor * new Color(1, 1, 1, bushAlpha); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player_Ditection") && other.gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            myCollider.transform.position = oriPos;

            _renderer.material.color = oriColor; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player_Enter");
    }
}