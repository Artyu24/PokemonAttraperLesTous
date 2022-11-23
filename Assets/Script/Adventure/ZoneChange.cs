using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum Zone
{
    WATER,
    CITY,
    FOREST,
    ROAD,
    CAVE
}

public class ZoneChange : MonoBehaviour
{
    [SerializeField] private Zone zoneState;
    [SerializeField] private String zoneName;
    [SerializeField] private String oldSound;
    [SerializeField] private String newSound;
    private Sprite background;

    void Awake()
    {
        int id = 3;
        switch (zoneState)
        {
            case Zone.WATER:
                id = 0;
                break;
            case Zone.CITY:
                id = 3;
                break;
            case Zone.FOREST:
                id = 7;
                break;
            case Zone.ROAD:
                id = 1;
                break;
            case Zone.CAVE:
                id = 2;
                break;
        }

        Sprite[] tabArea = Resources.LoadAll<Sprite>("Area_Icone/Area_Frame");
        background = tabArea[id];
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameObject areaFrame = GameManager.Instance.AreaFrame;
            if (areaFrame != null)
            {
                areaFrame.GetComponent<Image>().sprite = background;
                areaFrame.GetComponent<Animator>().SetTrigger("Area");
                Transform textArea = areaFrame.transform.GetChild(0);
                if (textArea != null)
                {
                    textArea.gameObject.GetComponent<Text>().text = zoneName;
                }
                if (FindObjectOfType<AudioManager>() != null && oldSound != "" && newSound != "")
                {
                    FindObjectOfType<AudioManager>().StopFade(oldSound);
                    FindObjectOfType<AudioManager>().PlayFade(newSound);
                }
            }
            
        }
    }
}
