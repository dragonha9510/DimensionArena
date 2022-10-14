using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapPrefaps;
    [SerializeField]
    private GameObject[] playerPrefaps;

    private void Start()
    {
        mapPrefaps = Resources.LoadAll<GameObject>("Tool/Map");
        //playerPrefaps = Resources.LoadAll<GameObject>("")
        Instantiate(mapPrefaps[0]);
    }
}
