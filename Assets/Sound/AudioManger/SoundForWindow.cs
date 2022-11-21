using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundForWindow : EditorWindow
{
    public string SoundName;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    public AudioMixerGroup audioMixer;
    [HideInInspector]
    public AudioSource source;

    [MenuItem("Window/SoundWindow")]
    static void InitWindow()
    {
        SoundForWindow window = GetWindow<SoundForWindow>();
        window.titleContent = new GUIContent("SoundWindow");
        window.Show();
        
    }

}
