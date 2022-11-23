using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FranckoManager : MonoBehaviour {

    public string NameInput { get; set; }
    public Text txtGameName;

    public void Awake()
    {
        SaveSystem.InitSaveData();
    }

    public void OnSave()
    {
        SaveSystem.SaveGameData(NameInput);
    }

    public void OnLoad()
    {
        SaveData.GameData gameData = SaveSystem.LoadGameData();
        txtGameName.text = gameData.gameName;
    }
}
