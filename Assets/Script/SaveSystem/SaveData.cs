using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using static SaveData;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct GameData 
    {
        public string gameName;
        public int profilCountTotal;

        public GameData(string name, int profilCount)
        {
            string newName = string.IsNullOrEmpty(name) ? "New_Game" : name;

            gameName = newName;
            profilCountTotal = profilCount;
        }
    }
    public GameData MyGameData { get; private set; }


    [System.Serializable]
    public struct SettingData
    {
        //public bool fullScreenOn;
        //public int idResolution;

        //public int idLanguage;
        //public int idQuality;
    
        //public int musicVolume;
        //public int sfxVolume;

        //public SettingData(bool fullScreenOn, int idResolution, int idLanguage, int idQuality, int musicVolume, int sfxVolume)
        //{
        //    this.fullScreenOn = fullScreenOn;
        //    this.idResolution = idResolution;
        //    this.idLanguage = idLanguage;
        //    this.idQuality = idQuality;
        //    this.musicVolume = musicVolume;
        //    this.sfxVolume = sfxVolume;
        //}

        public float posX;
        public float posY;

        public SettingData(GameObject player)
        {
            if (player != null)
            {
                posX = player.transform.position.x;
                posY = player.transform.position.y;
            }
            else
            {
                posX = 0;
                posY = 0;
            }
        }
    }
    public SettingData MySettingData { get; private set; }

    public SaveData() 
    {
        MyGameData = new GameData();
        MySettingData = new SettingData();
    }

    public SaveData(GameData gameData, SettingData settingData)
    {
        MyGameData = gameData;
        MySettingData = settingData;
    }
}
