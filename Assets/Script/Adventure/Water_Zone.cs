using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water_Zone : MonoBehaviour, IInteractable
{
    private GameObject waterBox;
    private Text[] waterText = new Text[2];
    private bool wantSlide;

    private void Start()
    {
        waterBox = GameManager.Instance.WaterBox;

        int i = 0;
        foreach (Transform child in waterBox.transform)
        {
            waterText[i] = child.GetComponent<Text>();
            i++;
        }
    }

    public void Interact()
    {
        wantSlide = true;
        waterBox.SetActive(true);
        waterText[0].text = "Oui        ◄";
        waterText[1].text = "Non";
    }

    private void SwitchText()
    {
        if (wantSlide)
        {
            waterText[0].text = "Oui";
            waterText[1].text = "Non        ◄";
            wantSlide = false;
        }
        else
        {
            waterText[0].text = "Oui        ◄";
            waterText[1].text = "Non";
            wantSlide = true;
        }
    }
}
