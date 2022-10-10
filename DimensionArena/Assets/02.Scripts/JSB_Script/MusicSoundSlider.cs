using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicSoundSlider : MonoBehaviour
{

    [SerializeField] Slider slider;


    public void ValueChanged()
    {
        SoundManager.Instance.SettingMusicVolume(slider.value);
    }
}
