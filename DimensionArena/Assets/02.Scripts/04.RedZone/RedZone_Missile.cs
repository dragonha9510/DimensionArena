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
    protected bool effectOn;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private Vector2 missileScale;
    [SerializeField] private float Damage;

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

        if(transform.position.y < -5f)
        {
            Destroy(this.gameObject);
            if (destroyEffect)
                Instantiate(destroyEffect, new Vector3(transform.position.x, 0, transform.position.z), destroyEffect.transform.rotation);
            //Destroy(boundary);
            //Destroy(outterBoundary);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "ParentGround"
            || collision.gameObject.tag == "Parent_Obstacle")
        {
            Destroy(this.gameObject);

            if (destroyEffect)
                Instantiate(destroyEffect, transform.position/*new Vector3(transform.position.x, 0, transform.position.z)*/, destroyEffect.transform.rotation);

            //Destroy(boundary);
            //Destroy(outterBoundary);
        }
        else if (collision.gameObject.tag == "Water_Plane")
            Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        effectOn = false;

        if (other.CompareTag("Player") || other.CompareTag("ParentGround")
            || other.CompareTag("ParentObstacle"))
        {
            Destroy(this.gameObject);
            effectOn = true;
        }
        else if (other.CompareTag("Water_Plane"))
            Destroy(this.gameObject);
        else if (other.CompareTag("Bush") || other.CompareTag("Item_Box"))
            Destroy(other.gameObject);

        if (effectOn)
        {
            GetComponent<KnockBackObject>().KnockBackStartDamage("RedZone", Damage);

            if (destroyEffect)
                Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
        }
    }
}
