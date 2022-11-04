using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvartar_Info : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] avartar;

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

        for(int i = 0; i < avartar.Length; ++i)
        {
            if (avartar[i].name == SelectedCharacter.Instance.characterName)
                avartar[i].SetActive(isChange = true);
            else
                avartar[i].SetActive(false);
        }

        if (!isChange)
            avartar[avartar.Length - 1].SetActive(true);
    }
}
