using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip[] clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private Sound[] sounds;
    private Dictionary<string, AudioClip[]> soundDictionary = new Dictionary<string, AudioClip[]>();
    public AudioSource audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = gameObject.AddComponent<AudioSource>();

        // Initialize sounds from inspector
        foreach (Sound sound in sounds)
        {
                soundDictionary.Add(sound.name, sound.clip);
        }
    }

    public void PlaySound(string soundName, float volume = 1)
    {
        if(audioSource != null)
        {
            if (soundDictionary.ContainsKey(soundName))
            {
                AudioClip[] clips = soundDictionary[soundName];
                audioSource.PlayOneShot(clips[Random.Range(0,clips.Length)], volume);
            }
            else
            {
                Debug.LogWarning($"Sound {soundName} not found in the dictionary!");
            }
        }
       
    }

    public void AddSound(string soundName, AudioClip[] clip)
    {
        if (!soundDictionary.ContainsKey(soundName))
        {
            soundDictionary.Add(soundName, clip);
        }
    }
}
