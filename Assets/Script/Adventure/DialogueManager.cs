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
    private string playerName;

    private ReadGoogleSheet readGoogleSheet;
    private PNJ actualPNJ;
    private bool onlyShowText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        readGoogleSheet = GetComponent<ReadGoogleSheet>();
    }

    private void Start()
    {
        if (SaveSystemManager.Instance != null)
            playerName = SaveSystemManager.Instance.GetNameSave();
        else
            playerName = "PLAYER";
    }

    public void InitDialogue<T>(T type, DialogueID[] dialogue)
    {
        switch (type)
        {
            case WaterZone:
                PlayerMovement.Instance.ActualInteractionDelegate = null;
                actualDialogueDelegate = WaterZone.Instance.InitInteraction;
                break;
            case PNJ:
                actualPNJ = type as PNJ;
                PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;
                actualDialogueDelegate = null;
                break;
            case CombatManager:
                onlyShowText = true;
                PlayerMovement.Instance.ActualInteractionDelegate = null;
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

            if (GameManager.Instance.ActualGameState != GameState.Fight)
                GameManager.Instance.ActualPlayerState = PlayerState.Idle;
            
            PlayerMovement.Instance.ResetInteractionFunction();

            return;
        }

        PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;
        interactionImage.SetActive(false);

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
        else if (!onlyShowText)
        {
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
            interactionImage.SetActive(true);
        }
    }

    private void FixText(ref string texte)
    {
        Debug.Log(CombatManager.Instance.playerPoke.data.name);
        Debug.Log(CombatManager.Instance.enemiePoke.data.name);
        Debug.Log(CombatManager.Instance.PlayerAttackName);
        Debug.Log(CombatManager.Instance.EnemieAttackName);
        texte = texte.Replace("PLAYER", playerName).Replace("POKEPLAYER", CombatManager.Instance.playerPoke.data.name).Replace("POKEENEMIE", CombatManager.Instance.enemiePoke.data.name).Replace("ATTACKPLAYER", CombatManager.Instance.PlayerAttackName).Replace("ATTACKENEMIE", CombatManager.Instance.EnemieAttackName);
    }
}
