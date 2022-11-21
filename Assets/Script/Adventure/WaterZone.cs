using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterZone : MonoBehaviour, IInteractable
{
    public static WaterZone Instance;

    [SerializeField] private Dialogue dialogue;

    private GameObject waterBox;
    private Text[] waterText = new Text[2];
    private bool wantSlide;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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
        DialogueManager.Instance.InitDialogue(this, dialogue);
    }

    public void InitInteraction()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.WaterInteraction;
        wantSlide = true;
        waterBox.SetActive(true);
        waterText[0].text = "Oui        ◄";
        waterText[1].text = "Non";

        PlayerMovement.Instance.ActualInteractionDelegate = ValidateChoice;
    }

    public void SwitchText()
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

    public void ValidateChoice()
    {
        waterBox.SetActive(false);
        GameManager.Instance.ActualPlayerState = PlayerState.Idle;
        PlayerMovement.Instance.ResetInteractionFunction();
        DialogueManager.Instance.DisplayNextSentence();

        if (wantSlide)
        {
            //ICI JM LA REGARDE C EST L EAU
            PlayerMovement.Instance.WalkOnWater = true;
        }
        
    }
}
