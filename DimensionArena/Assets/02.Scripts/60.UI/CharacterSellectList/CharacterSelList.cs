using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharacterSelList : MonoBehaviour
{
    private List<GameObject> listCharacterSlot = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo di = new DirectoryInfo("../DimensionArena/Assets/Resources/CharacterIillustration/");
        int idx = 0;

        foreach(FileInfo File in di.GetFiles())
        {
            if (File.FullName.Contains(".meta"))
                continue;
            
            GameObject temp = transform.GetChild(idx + 3).gameObject;
            temp.SetActive(true);

            string strTemp = "";

            for(int i = 0; i <= File.Name.Length; ++i)
            {
                if (File.Name[i] == '.')
                    break;
                strTemp += File.Name[i];
            }

            GameObject imageTemp = new GameObject(strTemp);
            imageTemp.transform.SetParent(temp.transform);
            Image image = imageTemp.AddComponent<Image>();

            image.sprite = Resources.Load<Sprite>("CharacterIillustration/" + strTemp);
            image.rectTransform.pivot = new Vector2(0.5f, 1);
            image.rectTransform.anchorMin = new Vector2(0.5f, 1);
            image.rectTransform.anchorMax = new Vector2(0.5f, 1);
            image.rectTransform.sizeDelta = new Vector3(image.sprite.rect.width, image.sprite.rect.height, 1);
            image.rectTransform.localPosition = Vector3.zero;
            imageTemp.transform.localPosition = Vector3.zero;
            image.rectTransform.anchoredPosition = Vector3.zero;
            
            listCharacterSlot.Add(temp);

            ++idx;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
