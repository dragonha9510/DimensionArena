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
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        popUpObject.SetActive(true);
    }

    public void SetDisablePopUp()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        popUpObject.SetActive(false);
    }
}
