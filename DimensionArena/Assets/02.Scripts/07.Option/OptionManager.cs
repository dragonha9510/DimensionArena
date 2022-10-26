using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{

    [SerializeField]
    private GameObject optionCanvas;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider effectSlider;
    [SerializeField]
    private Button joyStickFixOn;
    [SerializeField]
    private Button joyStickFixOff;
    [SerializeField]
    private Button CreditBtn;

    // Test Will TrashCode
    private void OnEnable()
    {
        joyStickFixOff.gameObject.SetActive(true);
        joyStickFixOn.gameObject.SetActive(false);
    }

    public void JoyStickFixOn()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        joyStickFixOff.gameObject.SetActive(false);
        joyStickFixOn.gameObject.SetActive(true);
    }
    public void JoyStickFixOff()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        joyStickFixOff.gameObject.SetActive(true);
        joyStickFixOn.gameObject.SetActive(false);
    }
    public void MusicSetting()
    {
        SoundManager.Instance.SettingMusicVolume(musicSlider.value);
    }
    public void EffectSoundSetting()
    {
        SoundManager.Instance.SettingSFXVolume(effectSlider.value);
    }

    public void CanvasOff()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        optionCanvas.SetActive(false);
    }

    public void OptionCanvasOn()
    {
        optionCanvas.SetActive(true);
    }

    public void OptionCanvasOff()
    {
        optionCanvas.SetActive(false);
    }
}
