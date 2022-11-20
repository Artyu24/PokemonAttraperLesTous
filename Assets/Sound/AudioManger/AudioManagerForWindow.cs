//using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine;
//using UnityEngine.Audio;
//using Unity.VisualScripting;

//public class AudioManagerForWindow : MonoBehaviour
//{
//    public SoundForWindow[] sounds;

//    public static AudioManagerForWindow instance;
//    private void Start()
//    {
//        FindObjectOfType<AudioManagerForWindow>().Play("MainTheme");

//    }
//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }
//        DontDestroyOnLoad(gameObject);
//        foreach (SoundForWindow s in sounds)
//        {
//            s.source = gameObject.AddComponent<AudioSource>();
//            s.source.clip = s.clip;

//            s.source.volume = s.volume;
//            s.source.pitch = s.pitch;
//            s.source.loop = s.loop;
//            s.source.outputAudioMixerGroup = s.audioMixer;
//        }
//    }

//    public void Play(string name)
//    {
//        SoundForWindow s = Array.Find(sounds, sound => sound.name == name);
//        if (s == null)
//        {
//            Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
//            return;
//        }
//        s.source.Play();
//    }
//}