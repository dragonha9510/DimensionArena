using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedZone_Missile : MonoBehaviour
{
    [SerializeField] private GameObject boundaryPrefab;
    [SerializeField] private GameObject outterboundaryPrefab;
    private GameObject boundary;
    private GameObject outterBoundary;
    private DecalProjector proj;
    private float posY;

    private void Start()
    {
        posY = transform.position.y;

        boundary = Instantiate(boundaryPrefab);
        boundary.transform.position = new Vector3(transform.position.x, boundaryPrefab.transform.position.y, transform.position.z);

        outterBoundary = Instantiate(outterboundaryPrefab);
        outterBoundary.transform.position = new Vector3(transform.position.x, outterboundaryPrefab.transform.position.y, transform.position.z);

        proj = boundary.GetComponent<DecalProjector>();

        proj.size = new Vector3(0, 0, 2.5f);
    }

    private void Update()
    {
        float sizeScale = (posY - transform.position.y) / posY;
        proj.size = new Vector3(sizeScale, sizeScale, 2.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != gameObject.name)
        { 
            Destroy(this.gameObject);
            Destroy(boundary);
            Destroy(outterBoundary);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != gameObject.name)
        {
            Destroy(this.gameObject);
            Destroy(boundary);
            Destroy(outterBoundary);
        }
    }
}
