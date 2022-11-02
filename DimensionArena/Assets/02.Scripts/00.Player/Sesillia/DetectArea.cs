using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    [SerializeField] private SphereCollider sphareCollision;

    List<Collision> collisions = new List<Collision>();
    bool isCantStep;

    private void Update()
    {
        Debug.Log(isCantStep);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCantStep = true;
    }


    private void OnCollisionExit(Collision collision)
    {
        isCantStep = false;
    }
  
}
