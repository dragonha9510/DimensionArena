using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Lobby_BtnFunction : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text subName;
    [SerializeField] private TMP_Text tmpLobbyState;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private TextMeshProUGUI nameText;
    public void Start()
    {
        // JSB
        SoundManager.Instance.PlayBGM("LobbyMusic");
        SoundManager.Instance.IsInLobby = true;
        LobbyManagerRenewal.Instance.Reconnect();
        nameText.text = FirebaseDB_Manager.Instance.PlayerNickName;
        //
        if (string.IsNullOrEmpty(SelectedCharacter.Instance.characterName))
        {
            SelectedCharacter.Instance.characterName = subName.text = "JiJooHyeock";
            SelectedCharacter.Instance.characterSprite = image.sprite = defaultSprite;
            image.rectTransform.sizeDelta = new Vector2(defaultSprite.rect.width * 0.5f, defaultSprite.rect.height * 0.5f);
            return;
        }

        subName.text = SelectedCharacter.Instance.characterName;
        image.sprite = SelectedCharacter.Instance.characterSprite;
        image.rectTransform.sizeDelta = new Vector2(SelectedCharacter.Instance.characterSprite.rect.width * 0.5f, SelectedCharacter.Instance.characterSprite.rect.height * 0.5f);
    }

    public void ToCharacterChange()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        SceneManager.LoadScene("CharacterSelect");
    }

    public void ToModeChanage()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        SceneManager.LoadScene("SelectMap");

    }

    public void Plus()
    {
        LobbyManagerRenewal.Instance.PlusLeastStartPlayer();
        tmpLobbyState.text = LobbyManagerRenewal.Instance.LeastStartPlayer.ToString();
    }
    public void Mins()
    {
        LobbyManagerRenewal.Instance.MinusLeastStartPlayer();

        tmpLobbyState.text = LobbyManagerRenewal.Instance.LeastStartPlayer.ToString();
    }
}
