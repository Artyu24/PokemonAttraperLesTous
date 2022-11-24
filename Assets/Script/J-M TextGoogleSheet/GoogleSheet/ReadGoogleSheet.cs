using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class ReadGoogleSheet : MonoBehaviour
{
    public Text outputArea;
    [SerializeField] private int firstNumber;
    [SerializeField] private int secondNumber;
    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private void Update()
    {

    }
    IEnumerator ObtainSheetData()
    {
        string link = "https://sheets.googleapis.com/v4/spreadsheets/11HeIxoXTcmEcJ2bSRSySPyQMftcyPENFiX6AVqqaDWY/values/Feuille%201?key=AIzaSyBLAdauLnxGZsp9wHva5rStJJZzq6cdUls";
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
            updateText = JSON.Parse(o["values"][firstNumber][secondNumber].ToString());
            outputArea.text = updateText;
        }
    }
}
