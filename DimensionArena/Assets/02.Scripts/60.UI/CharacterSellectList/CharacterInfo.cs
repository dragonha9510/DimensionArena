using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private Transform player;

    private void OnEnable()
    {
        txt.text = SelectedCharacter.Instance.nextCharacterName;
    }

    private void OnDisable()
    {
        player.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void BackSpace()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        SelectedCharacter.Instance.CharacterSelected(false);
    }

    public void CharacterSelected()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");

        SelectedCharacter.Instance.ChangeCharacterInfo();
    }
}
