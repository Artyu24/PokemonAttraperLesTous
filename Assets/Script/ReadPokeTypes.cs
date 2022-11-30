using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;

public class ReadPokeTypes : MonoBehaviour
{
    public float multiplyer;
    //public Text outputArea;
    public static ReadPokeTypes instance;
    [SerializeField] private int firstNumber;
    [SerializeField] private int secondNumber;
    string json;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }
    public IEnumerator ObtainSheetData()
    {
        string link = "https://sheets.googleapis.com/v4/spreadsheets/11HeIxoXTcmEcJ2bSRSySPyQMftcyPENFiX6AVqqaDWY/values/Feuille%202?key=AIzaSyBLAdauLnxGZsp9wHva5rStJJZzq6cdUls";
        UnityWebRequest www = UnityWebRequest.Get(link);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError || www.timeout > 2)
        {
            Debug.Log("Error" + www.error);
        }
        else
        {
            json = www.downloadHandler.text;
        }
    }

    public float Test(int liste, int emplacement)
    {
        string updateText = "";
        var o = JSON.Parse(json);
        updateText = JSON.Parse(o["values"][liste][emplacement].ToString());

        if (updateText != null)
        {
            multiplyer = float.Parse(updateText);
            return multiplyer;
        }
        return -1;
    }
}
