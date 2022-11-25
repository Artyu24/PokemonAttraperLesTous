using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject interactionImage;
    [SerializeField] private Text dialogueText;

    public delegate void DialogueDelegate();
    private DialogueDelegate actualDialogueDelegate = null;

    //Cree une liste ranger dans l ordre d apparition les objets present
    public Queue<string> sentences = new Queue<string>();
    private string actualSentence;

    private ReadGoogleSheet readGoogleSheet;
    private PNJ actualPNJ;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        readGoogleSheet = GetComponent<ReadGoogleSheet>();
    }

    public void InitDialogue<T>(T type, DialogueID[] dialogue, PNJ pnj = null)
    {
        switch (type)
        {
            case WaterZone:
                PlayerMovement.Instance.ActualInteractionDelegate = null;
                actualDialogueDelegate = WaterZone.Instance.InitInteraction;
                break;
            case PNJ:
                actualPNJ = pnj;
                PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;
                actualDialogueDelegate = null;
                break;
            default:
                PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;
                actualDialogueDelegate = null;
                break;
        }

        readGoogleSheet.GetTextString(dialogue);
    }

    public void StartDialogue(string[] dialogue)
    {
        sentences.Clear();

        dialogueBox.SetActive(true);

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayTextInstant()
    {
        StopAllCoroutines();
        dialogueText.text = actualSentence;
        interactionImage.SetActive(true);
        PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            dialogueBox.SetActive(false);
            interactionImage.SetActive(false);

            if (WaterZone.Instance.IsOpen)
            {
                PlayerMovement.Instance.ActualInteractionDelegate = null;
                WaterZone.Instance.ActivateEnterAnimation();
                return;
            }

            if (actualPNJ != null)
            {
                actualPNJ.ActualPnjState = PNJState.Idle;
                actualPNJ = null;
            }

            GameManager.Instance.ActualPlayerState = PlayerState.Idle;
            PlayerMovement.Instance.ResetInteractionFunction();

            return;
        }

        actualSentence = sentences.Dequeue();
        FixText(ref actualSentence);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(actualSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }

        if (actualDialogueDelegate != null)
            actualDialogueDelegate();
        else
        {
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
            interactionImage.SetActive(true);
        }
    }

    private void FixText(ref string texte)
    {
        texte = texte.Replace("PLAYER", "Franck'o").Replace("POKEMON", "Pikachu").Replace("VILLE", "Bourgeon");
    }
}
