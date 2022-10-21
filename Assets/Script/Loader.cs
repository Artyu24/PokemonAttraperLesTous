using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Loader : MonoBehaviour
{
    private int progress = 0;
    private List<string> languages = new List<string>();

    void Initialize()
    {

    }

    public void Load()
    {
        StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (null == data)
        {
            Debug.Log("Was not able to download data or retrieve stale data");
        }
        else
        {
            StartCoroutine(ProcessData(data, AfterProcessData));
        }
    }

    private void AfterProcessData(string errorMessage)
    {
        if (null!= errorMessage)
        {
            Debug.Log("Was not able to process data : " + errorMessage);
        }
        else
        {
            
        }
    }

    public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

    }

    private void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
    {

    }
}
