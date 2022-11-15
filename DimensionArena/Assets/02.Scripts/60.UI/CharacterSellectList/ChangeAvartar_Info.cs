using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvartar_Info : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] avartar;
    [SerializeField] private bool isMainLobby = false;
    [SerializeField] private Light lightAlignment;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetAvartar();
    }

    private void SetAvartar()
    {
        bool isChange = false;
        string tempName = isMainLobby ? SelectedCharacter.Instance.characterName : SelectedCharacter.Instance.nextCharacterName;

        for (int i = 0; i < avartar.Length; ++i)
        {
            if (avartar[i].name == tempName)
                avartar[i].SetActive(isChange = true);
            else
                avartar[i].SetActive(false);
        }

        if (!isChange)
            avartar[avartar.Length - 1].SetActive(true);

        if (lightAlignment == null)
            return;

  
        if (tempName.Equals("Secilia"))
            lightAlignment.intensity = 0.6f;
        else
            lightAlignment.intensity = 1.1f;
     

    }
}
