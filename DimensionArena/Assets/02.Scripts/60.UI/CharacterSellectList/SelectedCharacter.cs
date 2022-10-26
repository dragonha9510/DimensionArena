using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacter : MonoBehaviour
{
    public static SelectedCharacter Instance;
    public string characterName;
    public string nextCharacterName;

    public Canvas characterSelect;
    public Canvas characterInfo;

    public Sprite characterSprite;
    public Sprite nextCharacterSprite;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.characterSelect = this.characterSelect;
            Instance.characterInfo = this.characterInfo;
            Destroy(this.gameObject); 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void CharacterSelected(bool isOn)
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        characterSelect.gameObject.SetActive(!isOn);
        characterInfo.gameObject.SetActive(isOn);
    }

    public void ChangeCharacterInfo()
    {
        characterSprite = nextCharacterSprite;
        characterName = nextCharacterName;
    }
}