using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject rotateImage;

    [SerializeField]
    private float rotateSpeed = 2f;
    private void FixedUpdate()
    {
        rotateImage.transform.Rotate(Vector3.forward * rotateSpeed);
    }
}
