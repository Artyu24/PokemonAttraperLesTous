using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReadGoogleSheet : MonoBehaviour
{

    //public Text outputArea;
    //private void Start()
    //{
    //    StartCoroutine(ObtainSheetData());
    //}

    //private void Update()
    //{
        
    //}

    //IEnumerator ObtainSheetData()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/11HeIxoXTcmEcJ2bSRSySPyQMftcyPENFiX6AVqqaDWY/values/Feuille1?key=AIzaSyBLAdauLnxGZsp9wHva5rStJJZzq6cdUls");
    //    yield return www.SendWebRequest();
    //    if(www.isNetworkError || www.isHttpError || www.timeout>2)
    //    {
    //        Debug.Log("Error" + www.error);
    //    }
    //    else
    //    {
    //        string updateText = "";
    //        string json = www.downloadHandler.text;
    //        var o = JSON.Parse(json);

    //        foreach(var item in o["feed"]["entry"])
    //        {
    //            var itemo = JSON.Parse(item.ToString());

    //            updateText = itemo[0]["gsx$text"]["$t"] ;
    //        }
    //        outputArea.text = updateText;
    //    }
    //}
}
