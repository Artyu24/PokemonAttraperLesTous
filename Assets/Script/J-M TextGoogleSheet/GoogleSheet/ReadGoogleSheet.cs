using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class ReadGoogleSheet : MonoBehaviour
{
    public string link;  //https://spreadsheets.google.com/feeds/list//od6/public/values?alt=json
    public Text outputArea;
    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private void Update()
    {

    }
    IEnumerator ObtainSheetData()
    {
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

            foreach (var item in o["values"][2])
            {
                var itemo = JSON.Parse(item.ToString());

                //foreach (var i in itemo)
                //    Debug.Log(JSON.Parse(i.ToString()));

                updateText = JSON.Parse(itemo[0].ToString());
            }
            outputArea.text = updateText;
        }
    }
}
