using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviourPun
{
    public enum PLAYMODE
    {
        PLAYMODE_ONECE,
        PLAYMODE_END,
    }

    [SerializeField] private List<string> sceneNames;

    public static SoundManager Instance;

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    private Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

    
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            bgmPlayer = GetComponent<AudioSource>();
            sfxPlayer = GetComponent<AudioSource>();

            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }
    public void LoadMusics()
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
    
    public void PlaySFXOneShot(string audioClipName)
    {
        if (null == AudioClips[audioClipName])
            return;
        else
            sfxPlayer.clip = AudioClips[audioClipName];
        sfxPlayer.Play();
    }

    public void PlaySFXAllClient(string audioClipName)
    {
        for(int i = 0; i < PlayerInfoManager.Instance.PlayerObjectArr.Length; ++i)
        {
            //PlayerInfoManager.Instance.PlayerObjectArr[i].name == photonView.ViewID
        }
        //PlayerInfoManager.Instance.PlayerObjectArr[]
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
