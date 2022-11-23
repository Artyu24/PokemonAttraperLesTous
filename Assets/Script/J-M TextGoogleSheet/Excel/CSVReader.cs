using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]

    public class Player 
    {
        public string name;

        public int id;

        public string dialogue;     
    }
    [System.Serializable]

    public class PlayerList
    {
        public Player[] player;
    }
    public PlayerList myPlayerList = new PlayerList();


    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] {"," , "/n"}, StringSplitOptions.None); 
    }

}
