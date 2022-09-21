using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Test_Cube : MonoBehaviour
{
    [SerializeField] private GameObject prefab_cube;

    private void Start()
    {
        for(int i = 0; i < 100; ++i)
            Instantiate(prefab_cube, new Vector3(-49.5f + (float)(i), 0.5f, -49.5f), Quaternion.identity);
    }
}
