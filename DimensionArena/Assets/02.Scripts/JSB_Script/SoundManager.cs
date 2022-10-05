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

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();



    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
       
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
        audioSource.clip = AudioClips["LobbyMusic"];
        audioSource.Play();
        audioSource.loop = true;
    }
    
    

    public void PlayMusic(string clipName)
    {
        audioSource.clip = AudioClips[clipName];
        audioSource.Play();
        audioSource.loop = true;
    }
    public AudioClip GetClip(string clipName)
    {
        if (AudioClips.ContainsKey(clipName))
            return AudioClips[clipName];
        else
            return null;
    }


}
