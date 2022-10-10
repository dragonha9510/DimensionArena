using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SoundManager : MonoBehaviourPun
{
    public enum PLAYMODE
    {
        PLAYMODE_ONECE,
        PLAYMODE_END,
    }

    [SerializeField] private List<string> sceneNames;

    public static SoundManager Instance;

    [SerializeField]
    private AudioSource bgmPlayer;
    [SerializeField]
    private AudioSource sfxPlayer;

    private Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

    
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            LoadMusics();

            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }
    private void LoadMusics()
    {
        foreach (string str in sceneNames)
        {
            var audios = Resources.LoadAll<AudioClip>("Sounds/" + str);
            foreach (AudioClip audio in audios)
            {
                if (false == AudioClips.ContainsKey(audio.name))
                {
                    AudioClips.Add(audio.name, audio);
                }
            }
        }
        AudioClip clip = AudioClips["LobbyMusic"];
        bgmPlayer.clip = AudioClips["LobbyMusic"];
        bgmPlayer.Play();
        bgmPlayer.loop = true;
    }


    [PunRPC]
    private void PlaySFXOneShot(string audioClipName)
    {
        if (null == AudioClips[audioClipName])
            return;
        else
            sfxPlayer.clip = AudioClips[audioClipName];
        sfxPlayer.Play();
    }

    public void SettingSFXVolume(float value)
    {
        sfxPlayer.volume = value;
        Debug.Log(sfxPlayer.volume);
    }
    public void SettingMusicVolume(float value)
    {
        bgmPlayer.volume = value;
    }


    public void PlaySFXAllClient(string audioClipName)
    {
        photonView.RPC("PlaySFXOneShot", RpcTarget.All, audioClipName);
    }



    public void PlayBGM(string clipName)
    {
        bgmPlayer.clip = AudioClips[clipName];
        bgmPlayer.Play();
        bgmPlayer.loop = true;
    }


    public AudioClip GetClip(string clipName)
    {
        if (AudioClips.ContainsKey(clipName))
            return AudioClips[clipName];
        else
            return null;
    }


}
