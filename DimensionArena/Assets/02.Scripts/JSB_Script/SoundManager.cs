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

    private static SoundManager instance;

    [SerializeField]
    private AudioSource bgmPlayer;
    [SerializeField]
    private AudioSource sfxPlayer;
    [SerializeField]
    private int inGameSoundCount = 6;
    private Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

    private bool isInLobby = false;
    public bool IsInLobby { set { isInLobby = value; } }

    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject soundMgr = GameObject.Find("SoundManager");//new GameObject("PlayerInfoManager");

                if (!soundMgr)
                {
                    soundMgr = new GameObject("SoundManager");
                    soundMgr.AddComponent<SoundManager>();
                }
                instance = soundMgr.GetComponent<SoundManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        LoadMusics();
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddPhotonView()
    {
        this.gameObject.AddComponent<PhotonView>();
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
        //AudioClip clip = AudioClips["LobbyMusic"];
        //bgmPlayer.clip = AudioClips["LobbyMusic"];
        //bgmPlayer.Play();
        //bgmPlayer.loop = true;
    }


    [PunRPC]
    public void PlaySFXOneShot(string audioClipName)
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

    public void PlayBGM(string clipName)
    {
        bgmPlayer.clip = AudioClips[clipName];
        bgmPlayer.Play();
        bgmPlayer.loop = true;
    }

    public void PlayRandomInGameSound()
    {
        int randomNumber = Random.Range(1, inGameSoundCount);
        bgmPlayer.clip = AudioClips["BattleMusic" + randomNumber.ToString()];
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