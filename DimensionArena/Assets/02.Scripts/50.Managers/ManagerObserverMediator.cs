using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerObserverMediator : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
