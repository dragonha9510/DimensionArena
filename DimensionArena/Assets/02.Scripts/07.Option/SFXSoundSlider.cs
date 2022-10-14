using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SFXSoundSlider : MonoBehaviour
{

    [SerializeField] Slider slider;

    
    public void ValueChanged()
    {
        SoundManager.Instance.SettingSFXVolume(slider.value);
    }
}
