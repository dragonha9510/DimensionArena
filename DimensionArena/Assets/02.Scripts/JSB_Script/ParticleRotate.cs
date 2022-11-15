using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject nonstaticObject;
    // Start is called before the first frame update

    private void Update()
    {
        //this.transform.rotation = Quaternion.LookRotation(-transform.eulerAngles);
    }
}
