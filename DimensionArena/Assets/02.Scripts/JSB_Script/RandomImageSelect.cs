using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RandomImageSelect : MonoBehaviour
{
    [SerializeField]
    List<Sprite> randBackground;
    [SerializeField]
    Image backgroundImage;

    private void Start()
    {
        backgroundImage.sprite = randBackground[Random.Range(0, randBackground.Count)]; 
    }
}
