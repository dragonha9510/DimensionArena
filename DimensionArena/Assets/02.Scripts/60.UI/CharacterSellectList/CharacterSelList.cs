using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharacterSelList : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject SelectedRect;
    private List<GameObject> CharcterList = new List<GameObject>();
    void Start()
    {
        int idx = 0;

        Sprite[] fileinfos = Resources.LoadAll<Sprite>("CharacterIillustration/");

        foreach(var sprite in fileinfos)
        {
            //FileInfo File = (FileInfo)fileinfo;
            //if (File.FullName.Contains(".meta"))
            //    continue;
            
            GameObject temp = transform.GetChild(idx + 3).gameObject;
            temp.SetActive(true);

            string strTemp = sprite.name;

            //for (int i = 0; i <= sprite.name.Length; ++i)
            //{
            //    if (sprite.name[i] == '.')
            //        break;
            //    strTemp += sprite.name[i];
            //}

            GameObject imageTemp = new GameObject(strTemp);
            imageTemp.transform.SetParent(temp.transform);
            Image image = imageTemp.AddComponent<Image>();
            CharcterList.Add(imageTemp);


            image.sprite = sprite;
            imageTemp.AddComponent<SetSelectedName>().mySprite = image.sprite;

            if(strTemp == "JiJooHyeock")
                image.rectTransform.localScale = new Vector3(0.5f, 0.5f, 1);

            image.rectTransform.pivot = new Vector2(0.5f, 1);
            image.rectTransform.anchorMin = new Vector2(0.5f, 1);
            image.rectTransform.anchorMax = new Vector2(0.5f, 1);
            image.rectTransform.sizeDelta = new Vector3(image.sprite.rect.width, image.sprite.rect.height, 1);
            image.rectTransform.localPosition = Vector3.zero;
            imageTemp.transform.localPosition = Vector3.zero;
            image.rectTransform.anchoredPosition = Vector3.zero;
            ++idx;


        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var character in CharcterList)
        {
            if (SelectedCharacter.Instance.characterName.Equals(character.name))
            {
                SelectedRect.transform.SetParent(character.transform.parent);
                SelectedRect.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0,0);
                break;
            }
        }
    }
}
