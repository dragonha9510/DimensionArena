using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_CameraContorller : MonoBehaviour
{
    [SerializeField] private GameObject[] _camera;

    private int cameraNum = 1;
    private int nextCameraNum = 1;

    private void Start()
    {
        if (_camera == null)
            Destroy(this.gameObject);
    
        for (int i = 0; i < _camera.Length; ++i)
            _camera[i].SetActive(false);
        
        _camera[cameraNum].SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
            nextCameraNum = 0;
        if (Input.GetKey(KeyCode.Alpha2))
            nextCameraNum = 1;
        if (Input.GetKey(KeyCode.Alpha3))
            nextCameraNum = 2;

        if (cameraNum != nextCameraNum)
        {
            cameraNum = nextCameraNum;

            for (int i = 0; i < _camera.Length; ++i)
                _camera[i].SetActive(false);

            _camera[cameraNum].SetActive(true);
        }
    }

}
