using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject baseMenu;

    public void SaveMenu()
    {
        if (saveMenu != null)
        {
            saveMenu.SetActive(true);
            baseMenu.SetActive(false);
        }
    }

    public void BaseMenu()
    {
        if (baseMenu != null)
        {
            baseMenu.SetActive(true);
            saveMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
