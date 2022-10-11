using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainProfileButton : MonoBehaviour
{
    [SerializeField]
    GameObject profileObject;

    public void SetActiveProfileUI()
    {
        profileObject.SetActive(true);
    }
}
