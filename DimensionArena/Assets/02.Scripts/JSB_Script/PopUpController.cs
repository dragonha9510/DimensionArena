using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PopUpController : MonoBehaviour
{
    [SerializeField]
    GameObject popUpObject;

    public void SetActivePopUp()
    {
        popUpObject.SetActive(true);
    }

    public void SetDisablePopUp()
    {
        popUpObject.SetActive(false);
    }
}
