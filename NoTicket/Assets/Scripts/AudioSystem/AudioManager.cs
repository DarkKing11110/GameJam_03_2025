using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public Sound[] music, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        playAudio("Theme");
        setMusicVolume(0.5f);
    }


    public void playAudio(string pName)
    {
        Sound musicSound = Array.Find(music, sound => sound.name == pName);
        // Play music
        if (musicSound == null)
        {
            Debug.Log("no music found!");
        }
        else
        {
            musicSource.clip = musicSound.clip;
            musicSource.Play();
        }
    }

    public void playSFX(string pName)
    {
        Sound sfxSound = Array.Find(sfxSounds, sound => sound.name == pName);
        // Play SFX
        if (sfxSound == null)
        {
            Debug.Log("no sfx found!");
        }
        else
        {
            sfxSource.clip = sfxSound.clip;
            sfxSource.Play();
        }
    }

    public void toggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void toggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void setMusicVolume(float pVolume)
    {
        musicSource.volume = pVolume;
    }

    public void setSFXVolume(float pVolume)
    {
        sfxSource.volume = pVolume;
    }
}
