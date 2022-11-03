using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSVDownloader : MonoBehaviour
{
    private const string googleSheetDoc = "11HeIxoXTcmEcJ2bSRSySPyQMftcyPENFiX6AVqqaDWY";

    private const string url = "https://docs.google.com/spreadsheets/d/" + googleSheetDoc + "/export?format-csv";

    internal static IEnumerator DownloadData(System.Action<string> OnCompleted)
    {
        yield return new WaitForEndOfFrame();


        string downloadData = null;
        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log("Download Error : " + webRequest.error);
                downloadData = PlayerPrefs.GetString("LastDataDowloaded", null);
                string versionText = PlayerPrefs.GetString("LastDataDowloaded", null);
                Debug.Log("Using stale data version " + versionText);
            }
            else
            {
                Debug.Log("Download Success");
                Debug.Log("Data : " + webRequest.downloadHandler.text);

                string versionSection = webRequest.downloadHandler.text.Substring(0, 5);
                int equalIndex = versionSection.IndexOf('=');
                UnityEngine.Assertions.Assert.IsFalse(equalIndex == -1, "Could not find a '-' at the start of the CSV");
                string versionText = webRequest.downloadHandler.text.Substring(0, equalIndex);
                Debug.Log("Dowload Data version: " + versionText);
                PlayerPrefs.SetString("LastDataDownloaded", webRequest.downloadHandler.text);
                PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
                downloadData = webRequest.downloadHandler.text.Substring(equalIndex + 1);

            }
        }
        OnCompleted(downloadData);
    }
}
