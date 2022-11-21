using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Stop("MusicMenu");
        FindObjectOfType<AudioManager>().Play("MainTheme");
        SceneManager.LoadScene("SoundScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
