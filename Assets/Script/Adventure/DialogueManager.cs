using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text dialogueText;

    public delegate void DialogueDelegate();
    private DialogueDelegate actualDialogueDelegate = null;

    //Cree une liste ranger dans l ordre d apparition les objets present
    public Queue<string> sentences = new Queue<string>();
    private string actualSentence;

    public void InitDialogue<T>(T type, Dialogue dialogue)
    {
        switch (type)
        {
            case WaterZone:
                actualDialogueDelegate = DisplayTextInstant;
                break;
            default:
                actualDialogueDelegate = null;
                break;
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayTextInstant()
    {
        StopAllCoroutines();
        dialogueText.text = actualSentence;
        //Delegate d'intéraction qui change
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
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
    }
}
