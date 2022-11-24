using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance;

    public string NameInput { get; set; }

    [SerializeField] private string sceneToLoad;
    private Vector3 lastPosPlayer;
    public Vector3 LastPosPlayer => lastPosPlayer;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        SaveSystem.InitSaveData();
        DontDestroyOnLoad(this);
    }

    public void OnSave()
    {
        SaveSystem.SaveGameData(NameInput);
    }

    public void CreateNewGame()
    {
        if(NameInput == "" || NameInput == "New_Game")
            return;

        SaveSystem.SaveGameData(NameInput);
        LaunchGameScene();
    }

    public void OnLoad()
    {
        SaveData.GameData gameData = SaveSystem.LoadGameData();
        SaveData.SettingData settingData = SaveSystem.LoadSettingData();

        lastPosPlayer = new Vector3(settingData.posX, settingData.posY);
        LaunchGameScene();
    }

    public string GetNameSave()
    {
        SaveData.GameData gameData = SaveSystem.LoadGameData();
        return gameData.gameName;
    }

    private void LaunchGameScene()
    {
        FindObjectOfType<AudioManager>().Stop("MusicMenu");
        FindObjectOfType<AudioManager>().Play("MainTheme");
        SceneManager.LoadScene(sceneToLoad);
    }
}
