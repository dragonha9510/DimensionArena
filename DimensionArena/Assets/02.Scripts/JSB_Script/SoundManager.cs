using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
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
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        foreach (string str in sceneNames)
        {
            var audios = Resources.LoadAll<AudioClip>("Sounds/"+str);
            foreach(AudioClip audio in audios)
            {
                if (false == AudioClips.ContainsKey(audio.name))
                {
                    AudioClips.Add(audio.name, audio);
                }
            }
        }
    }
    private void ChangeBGM(string clipName)
    {
    }
    private void PlayMusic()
    {
    }
    public AudioClip GetClip(string clipName)
    {
        if (AudioClips.ContainsKey(clipName))
            return AudioClips[clipName];
        else
            return null;
    }


}
