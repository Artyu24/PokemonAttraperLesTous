using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Unity.VisualScripting;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("MusicMenu");

    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixer;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<AudioManager>().Stop("MainTheme");

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            FindObjectOfType<AudioManager>().Play("MainTheme");

        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
            return;
        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
            return;
        }
        s.source.Stop();
    }
    public void StopFade(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
            return;
        }
        StartCoroutine(FadeOut(s));
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
            return;
        }
        s.source.Pause();
    }
    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
            return;
        }
        s.source.UnPause();

    }

    public IEnumerator FadeOut(Sound s)
    {
        s.source.DOFade(0f, 3f);
        if(s.source.volume <= 0f)
        {
            s.source.Stop();
        }
        yield return null;
    }
    public IEnumerator FadeIn(Sound s)
    {
        s.source.DOFade(0f, 3f);
        if (s.source.volume <= 0f)
        {
            s.source.Stop();
        }
        yield return null;
    }
}