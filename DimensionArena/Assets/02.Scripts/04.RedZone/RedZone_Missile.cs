using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedZone_Missile : MonoBehaviour
{
    //[SerializeField] private GameObject boundaryPrefab;
    //[SerializeField] private GameObject outterboundaryPrefab;
    //private GameObject boundary;
    //private GameObject outterBoundary;
    //private DecalProjector proj;
    private float posY;


    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private Vector2 missileScale;

    protected virtual void Start()
    {
        //posY = transform.position.y;

        //boundary = Instantiate(boundaryPrefab);
        //boundary.transform.position = new Vector3(transform.position.x, boundaryPrefab.transform.position.y, transform.position.z);
        //boundary.GetComponent<DecalProjector>().size = new Vector3(missileScale.x, missileScale.y, 2.5f);

        //outterBoundary = Instantiate(outterboundaryPrefab);
        //outterBoundary.transform.position = new Vector3(transform.position.x, outterboundaryPrefab.transform.position.y, transform.position.z);
        //boundary.GetComponent<DecalProjector>().size = new Vector3(missileScale.x, missileScale.y, 2.5f);


        //proj = boundary.GetComponent<DecalProjector>();

        //proj.size = new Vector3(0, 0, 2.5f);
    }

    protected virtual void Update()
    {
        //float sizeScale = ((posY - transform.position.y) / posY) * missileScale.x;
        //proj.size = new Vector3(sizeScale, sizeScale, 2.5f);

        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
            Instantiate(destroyEffect, new Vector3(transform.position.x, 0, transform.position.z), destroyEffect.transform.rotation);
            //Destroy(boundary);
            //Destroy(outterBoundary);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != gameObject.name)
        { 
            Destroy(this.gameObject);
            Instantiate(destroyEffect, new Vector3(transform.position.x, 0, transform.position.z), destroyEffect.transform.rotation);
            //Destroy(boundary);
            //Destroy(outterBoundary);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.name != gameObject.name)
        {
            Destroy(this.gameObject);
            Instantiate(destroyEffect, new Vector3(transform.position.x, 0, transform.position.z), destroyEffect.transform.rotation);

            //Destroy(boundary);
            //Destroy(outterBoundary);
        }
    }
}
