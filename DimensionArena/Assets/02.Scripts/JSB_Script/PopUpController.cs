using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PopUpController : MonoBehaviour
{
    [SerializeField]
    GameObject popUpObject;
    [SerializeField]
    GameObject character;

    public void SetActivePopUp()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        popUpObject.SetActive(true);
        character.SetActive(false);
    }

    public void SetDisablePopUp()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        character.SetActive(true);
        popUpObject.SetActive(false);

    }
}
