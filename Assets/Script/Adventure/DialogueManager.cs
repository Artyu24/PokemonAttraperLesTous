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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void InitDialogue<T>(T type, string[] dialogue)
    {
        switch (type)
        {
            case WaterZone:
                PlayerMovement.Instance.ActualInteractionDelegate = null;
                actualDialogueDelegate = WaterZone.Instance.InitInteraction;
                break;
            default:
                PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;
                actualDialogueDelegate = null;
                break;
        }

        StartDialogue(dialogue);
    }

    private void StartDialogue(string[] dialogue)
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
                WaterZone.Instance.ActivateAnimation();
                return;
            }
            
            GameManager.Instance.ActualPlayerState = PlayerState.Idle;
            PlayerMovement.Instance.ResetInteractionFunction();


            return;
        }

        actualSentence = sentences.Dequeue();
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

        if(actualDialogueDelegate != null)
            actualDialogueDelegate();
        else
        {
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
            interactionImage.SetActive(true);
        }
    }
}
