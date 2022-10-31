using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class CSVDownloader : MonoBehaviour
{
    private const string k_googleSheetDocID = "1QAGetZagLebbvVikbkj5g_EMedwKmqMZJLn6wQmnmKA";
    private const string url = "https://docs.google.com/spreadsheets/d/" + k_googleSheetDocID + "/export?format=csv";

    internal static IEnumerator DownloadData(System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();

        string downloadData = null;
        using (UnityWebRequest webRequest = UnityWebRequest.Get( url ))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Download Error : " + webRequest.error);
                downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                string versionText = PlayerPrefs.GetString("LastDataDownloaded", null);
                Debug.Log("Using stale data version : " + versionText);
            }
            else
            {
                Debug.Log("Download success");
                Debug.Log("Data : " + webRequest.downloadHandler.text);

                string versionSection = webRequest.downloadHandler.text.Substring(0, 5);
                int equalsIndex = versionSection.IndexOf("=");
                UnityEngine.Assertions.Assert.IsFalse( equalsIndex == -1, "Could not find a '=' at the start of the CVS");

                string versionText = webRequest.downloadHandler.text.Substring(0, equalsIndex);
                Debug.Log("Downloaded data version: " + versionText);

                PlayerPrefs.SetString("LastDataDownloaded", webRequest.downloadHandler.text);
                PlayerPrefs.SetString("LastDownloadedVersion", versionText);

                downloadData = webRequest.downloadHandler.text.Substring(equalsIndex + 1);
            }
        }

        onCompleted(downloadData);
    }
}
