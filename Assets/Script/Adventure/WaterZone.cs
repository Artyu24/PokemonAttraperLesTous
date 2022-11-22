using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WaterZone : MonoBehaviour, IInteractable
{
    public static WaterZone Instance;

    [SerializeField] private Dialogue dialogueEnter;
    [SerializeField] private Dialogue dialogueValidation;
    
    private GameObject waterBox;
    private GameObject waterAnimation;
    private Text[] waterText = new Text[2];
    private bool wantSlide;
    private bool isOpen;
    public bool IsOpen => isOpen;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        waterBox = GameManager.Instance.WaterBox;
        waterAnimation = GameManager.Instance.WaterAnimation;

        int i = 0;
        foreach (Transform child in waterBox.transform)
        {
            waterText[i] = child.GetComponent<Text>();
            i++;
        }
    }

    public void Interact()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.WaterInteraction;
        DialogueManager.Instance.InitDialogue(this, dialogueEnter);
        isOpen = true;
    }

    public void InitInteraction()
    {
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

        if (wantSlide)
        {
            //ICI JM LA REGARDE C EST L EAU
            if(FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().StopFade("MainTheme");
                FindObjectOfType<AudioManager>().PlayFade("Surf");
            }
            DialogueManager.Instance.InitDialogue(PlayerMovement.Instance, dialogueValidation);

            PlayerMovement.Instance.WalkOnWater = true;
        }
        else
        {
            isOpen = false;
            DialogueManager.Instance.DisplayNextSentence();
        }
    }

    public void ActivateAnimation()
    {
        StartCoroutine(SetAnimation());
    }

    private IEnumerator SetAnimation()
    {
        waterAnimation.SetActive(true);
        yield return new WaitForSeconds(2.1f);
        waterAnimation.SetActive(false);

        isOpen = false;

        DirectionData lastDirection = PlayerMovement.Instance.GetLastDirection();
        GameObject player = PlayerMovement.Instance.gameObject;
        player.transform.DOJump(lastDirection.mouv + player.transform.position, 1, 1, 1f);

        yield return new WaitForSeconds(1.1f);

        GameManager.Instance.ActualPlayerState = PlayerState.Idle;
        PlayerMovement.Instance.ResetInteractionFunction();
    }
}
