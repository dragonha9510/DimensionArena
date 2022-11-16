using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private Transform player;

    [SerializeField] private Image[] SkillImage;

    [SerializeField] private Sprite[] AuraSkill;
    [SerializeField] private Sprite[] JiJooHyeockSkill;
    [SerializeField] private Sprite[] SecuritasSkill;
    [SerializeField] private Sprite[] RavagebellSkill;
    [SerializeField] private Sprite[] SeciliaSkill;

    private void OnEnable()
    {
        txt.text = SelectedCharacter.Instance.nextCharacterName;
        Sprite[] tempSprite;
        switch (txt.text)
        {
            case "Aura":
                tempSprite = AuraSkill;
                break;
            case "JiJooHyeock":
                tempSprite = JiJooHyeockSkill;
                break;
            case "Securitas":
                tempSprite = SecuritasSkill;
                break;
            case "Ravagebell":
                tempSprite = RavagebellSkill;
                break;
            case "Secilia":
                tempSprite = SeciliaSkill;
                break;
            default:
                tempSprite = null;
                break;
        }

        if (tempSprite == null)
            return;

        for(int i = 0; i < SkillImage.Length; ++i)
        {
            SkillImage[i].sprite = tempSprite[i];
        }
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
