using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialogueBox;
    public GameObject DialogueBox => dialogueBox;
    [SerializeField] private GameObject interactionImage;
    [SerializeField] private Text dialogueText;

    public delegate void DialogueDelegate();
    private DialogueDelegate actualDialogueDelegate = null;

    //Cree une liste ranger dans l ordre d apparition les objets present
    public Queue<string> sentences = new Queue<string>();
    private string actualSentence;
    private string playerName;

    private DialogueState actualDialogueState;
    [SerializeField] private float cdAction, cdDialogue;

    private ReadGoogleSheet readGoogleSheet;
    private PNJ actualPNJ;
    private BlockPlayerPNJ blockPNJ;

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
        PlayerMovement.Instance.ActualInteractionDelegate = null;
        
        switch (type)
        {
            case WaterZone:
                actualDialogueState = DialogueState.DRAW_ACTION;
                actualDialogueDelegate = WaterZone.Instance.InitInteraction;
                break;
            case PNJ:
                actualDialogueState = DialogueState.INTERACTION;
                actualPNJ = type as PNJ;
                break;
            case BlockPlayerPNJ:
                blockPNJ = type as BlockPlayerPNJ;
                actualDialogueState = DialogueState.INTERACTION;
                break;
            case CombatManager:
                actualDialogueState = DialogueState.DRAW;
                break;
            default:
                actualDialogueState = DialogueState.INTERACTION;
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

    public void DisplayNextSentence()
    {
        interactionImage.SetActive(false);
        
        if(sentences.Count == 0)
        {
            switch (actualDialogueState)
            {
                #region Draw
                case DialogueState.DRAW:

                    break;
                #endregion

                #region Draw & Action
                case DialogueState.DRAW_ACTION:

                    if (actualDialogueDelegate != null)
                    {
                        actualDialogueDelegate();
                        actualDialogueDelegate = null;
                    }

                    break;
                #endregion

                #region Interaction
                case DialogueState.INTERACTION:
                    dialogueBox.SetActive(false);

                    if (WaterZone.Instance.IsOpen)
                    {
                        PlayerMovement.Instance.ActualInteractionDelegate = null;
                        WaterZone.Instance.ActivateEnterAnimation();
                        return;
                    }

                    if (blockPNJ != null)
                    {
                        blockPNJ.ResetPos();
                        return;
                    }

                    if (actualPNJ != null)
                    {
                        actualPNJ.ActualPnjState = PNJState.Idle;
                        actualPNJ = null;
                    }

                    PlayerMovement.Instance.ResetInteractionFunction();
                    break;
                #endregion

                #region Interaction & Action
                case DialogueState.INTERACTION_ACTION:

                    if (actualDialogueDelegate != null)
                    {
                        actualDialogueDelegate();
                        actualDialogueDelegate = null;
                    }

                    dialogueBox.SetActive(false);
                    break;
                #endregion
            }

            if (GameManager.Instance.ActualGameState != GameState.Fight)
                GameManager.Instance.ActualPlayerState = PlayerState.Idle;

            return;
        }

        if(actualDialogueState == DialogueState.INTERACTION || actualDialogueState == DialogueState.INTERACTION_ACTION)
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayTextInstant;

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

        //TEXTE FINIS D ETRE ECRIS

        if (actualDialogueState == DialogueState.INTERACTION || actualDialogueState == DialogueState.INTERACTION_ACTION)
        {
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
            interactionImage.SetActive(true);
        }
        else
            StartCoroutine(CoolDownNextSentence());
    }

    private IEnumerator CoolDownNextSentence()
    {
        if (sentences.Count == 0 && actualDialogueState == DialogueState.DRAW_ACTION)
            yield return new WaitForSeconds(cdAction);
        else
            yield return new WaitForSeconds(cdDialogue);

        DisplayNextSentence();
    }

    public void DisplayTextInstant()
    {
        if (actualDialogueState == DialogueState.INTERACTION || actualDialogueState == DialogueState.INTERACTION_ACTION)
        {
            StopAllCoroutines();
            dialogueText.text = actualSentence;
            interactionImage.SetActive(true);
            PlayerMovement.Instance.ActualInteractionDelegate = DisplayNextSentence;
        }
    }

    private void FixText(ref string texte)
    {
        texte = texte.Replace("PLAYER", playerName).Replace("POKEPL", CombatManager.Instance.playerPoke.data.name).Replace("POKEEN", CombatManager.Instance.enemiePoke.data.name).Replace("ATKPL", CombatManager.Instance.PlayerAttackName).Replace("ATKEN", CombatManager.Instance.EnemieAttackName);
    }
}
