using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Diagnostics;

/*public class Loader : MonoBehaviour
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

        *//*if (data == null)
        {
            ResourceRequest dbLoader = Resources.LoadAsync<TextAsset>("db");
            while (!dbLoader.isDone)
            {
                yield return null;
            }
            TextAsset db = dbLoader.asset as TextAsset;
            data = db.text;
        }*//*

        Manager.instance.translator.termData = new TermData.Terms();

        int currLineIndex = 0;
        bool inQuote = false;
        int linesSinceUpdate = 0;
        int kLinesBetweenUpdate = 15;

        string currEntry = "";
        int currCharIndex = 0;
        bool currEntryContainedQuote = false;
        List<string> currLinesEntries = new List<string>();

        char lineEnding = Utils.IsIOS() ? '\n' : '\r';
        int lineEndingLength = Utils.IsIOS() ? 1 : 2;

        while (currCharIndex < data.Length)
        {
            if (!inQuote && (data[currCharIndex] == lineEnding))
            {
                currCharIndex += lineEndingLength;

                if (currEntryContainedQuote)
                {
                    currEntry = currEntry.Substring(1, currEntry.Length - 2);
                }

                currLinesEntries.Add(currEntry);
                currEntry = "";
                currEntryContainedQuote = false;

                ProcessLineFromCSV(currLinesEntries, currLineIndex);
                currLineIndex++;
                currLinesEntries = new List<string>();

                linesSinceUpdate++;
                if (linesSinceUpdate > kLinesBetweenUpdate)
                {
                    linesSinceUpdate = 0;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                if (data[currCharIndex] == "")
                {
                    inQuote = !inQuote;
                    currEntryContainedQuote = true;

                }
            }
        }

    }

    private void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
    {

    }
}*/
