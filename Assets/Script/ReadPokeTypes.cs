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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(ObtainSheetData(firstNumber, secondNumber));
    }
    public IEnumerator ObtainSheetData(int liste, int emplacement)
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
            string updateText = "";
            string json = www.downloadHandler.text;
            var o = JSON.Parse(json);

            updateText = JSON.Parse(o["values"][liste][emplacement].ToString());

            if (updateText != null)
            {
                multiplyer = float.Parse(updateText);
            }
        }
    }
}
