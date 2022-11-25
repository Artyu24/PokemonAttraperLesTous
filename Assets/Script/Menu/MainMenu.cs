using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject baseMenu;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject newGameMenu;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private Text saveNameText;

    private Button buttonLoad;
    private Image imageLoad;


    private void Awake()
    {
        buttonLoad = loadButton.GetComponent<Button>();
        imageLoad = loadButton.GetComponent<Image>();
    }

    public void BaseMenu()
    {
        if (baseMenu != null)
        {
            baseMenu.SetActive(true);
            saveMenu.SetActive(false);
            newGameMenu.SetActive(false);
        }
    }

    public void SaveMenu()
    {
        if (saveMenu != null)
        {
            saveMenu.SetActive(true);
            baseMenu.SetActive(false);
            string name = SaveSystemManager.Instance.GetNameSave();

            if (name == "New_Game")
            {
                saveNameText.text = "";
                imageLoad.color = new Color(255, 0, 0);
                buttonLoad.enabled = false;
            }
            else
            {
                saveNameText.text = name;
                buttonLoad.enabled = true;
                imageLoad.color = new Color(255, 255, 255);
            }
        }
    }

    public void CreateNewGameMenu()
    {
        if (newGameMenu != null)
        {
            newGameMenu.SetActive(true);
            saveMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
