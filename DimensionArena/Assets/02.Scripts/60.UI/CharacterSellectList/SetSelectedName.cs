using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedName : MonoBehaviour, IPointerClickHandler
{
    public Sprite mySprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectedCharacter.Instance.nextCharacterName = this.gameObject.name;
        SelectedCharacter.Instance.nextCharacterSprite = this.mySprite;
        SelectedCharacter.Instance.CharacterSelected(true);
    }
}
