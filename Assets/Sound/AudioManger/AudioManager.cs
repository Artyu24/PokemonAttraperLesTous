using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer audioMixer;
    private readonly string volumeMain = "MainVolume"; 
    private readonly string volumeMusic = "MusicVolume"; 
    private readonly string volumeSFX = "SFXVolume"; 
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat(volumeMain , 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
