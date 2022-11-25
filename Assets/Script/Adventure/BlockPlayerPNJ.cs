using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockPlayerPNJ : MonoBehaviour, IInteractable
{
    public static BlockPlayerPNJ Instance;

    [Header("Movement")]
    public int moveSpeed = 5;
    private Vector3 endPos;
    private Vector3 startPos;
    private bool canPass;
    public bool CanPass { get => canPass; set => canPass = value; }
    private bool isTrigger;
    private BoxCenter boxCenter;
    private PNJState actualPnjState = PNJState.Idle;
    public PNJState ActualPnjState { get => actualPnjState; set => actualPnjState = value; }

    [Header("Interaction")]
    [SerializeField] private DialogueID[] canPassDialogue;
    [SerializeField] private DialogueID[] cantPassDialogue;

    [Header("Animation")]
    public Animator anim;
    private PotentialDirection lastAnim = PotentialDirection.IDLE;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        boxCenter = GetComponent<BoxCenter>();
        endPos = boxCenter.CenterObject();
        startPos = transform.position;
    }

    private void Update()
    {
        #region MOVEMENT

        #region End Movement

        if (transform.position == endPos && GameManager.Instance.ActualGameState == GameState.Adventure && actualPnjState == PNJState.InMovement)
        {
            if (isTrigger)
            {
                isTrigger = false;
                DirectionData dirChoose = GameManager.Instance.DictDirection[PotentialDirection.SEE_DROITE];
                AnimPNJ(dirChoose.dirEnum, dirChoose.animName);
                DialogueManager.Instance.InitDialogue(this, cantPassDialogue);
            }
            else
            {
                DirectionData dirChoose = GameManager.Instance.DictDirection[PotentialDirection.SEE_HAUT];
                AnimPNJ(dirChoose.dirEnum, dirChoose.animName);
                GameManager.Instance.ActualPlayerState = PlayerState.Idle;
                PlayerMovement.Instance.ResetInteractionFunction();
            }

            endPos = boxCenter.CenterObject();
            transform.position = endPos;
            actualPnjState = PNJState.Idle;
        }

        #endregion

        #region In Movement

        if (GameManager.Instance.ActualGameState != GameState.Paused && actualPnjState == PNJState.InMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
        }

        #endregion

        #endregion
    }

    #region Animation

    private void AnimPNJ(PotentialDirection actualDir, string animTrigger)
    {
        if (lastAnim != actualDir)
        {
            anim.SetTrigger(animTrigger);
        }
        lastAnim = actualDir;
    }

    #endregion

    public void Interact()
    {
        if (actualPnjState == PNJState.Idle)
        {
            actualPnjState = PNJState.Interaction;
            DirectionData dirChoose = GameManager.Instance.DictDirection[GetOppositePosition(PlayerMovement.Instance.LastDirEnum)];
            AnimPNJ(dirChoose.dirEnum, dirChoose.animName);

            GameManager.Instance.ActualPlayerState = PlayerState.Interaction;
            PlayerMovement.Instance.ActualInteractionDelegate = null;

            if (canPass)
                DialogueManager.Instance.InitDialogue(GameManager.Instance, canPassDialogue);
            else
                DialogueManager.Instance.InitDialogue(GameManager.Instance, cantPassDialogue);
        }
    }

    public void ResetPos()
    {
        endPos = startPos;
        actualPnjState = PNJState.InMovement;

        PlayerMovement.Instance.MakeAMove(PotentialDirection.RIGHT);
        GameManager.Instance.ActualPlayerState = PlayerState.ForcedMove;

        DirectionData dirChoose = GameManager.Instance.DictDirection[PotentialDirection.DOWN];
        AnimPNJ(dirChoose.dirEnum, dirChoose.animName);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            endPos = new Vector3(transform.position.x, col.transform.position.y);

            DirectionData dirChoose = GameManager.Instance.DictDirection[PotentialDirection.UP];
            AnimPNJ(dirChoose.dirEnum, dirChoose.animName);

            PlayerMovement.Instance.MakeAMove(PotentialDirection.IDLE);
            GameManager.Instance.ActualPlayerState = PlayerState.Interaction;
            PlayerMovement.Instance.ActualInteractionDelegate = null;

            actualPnjState = PNJState.InMovement;
            isTrigger = true;
        }
    }

    private PotentialDirection GetOppositePosition(PotentialDirection playerDir)
    {
        PotentialDirection pnjDirection = PotentialDirection.IDLE;

        switch (playerDir)
        {
            case PotentialDirection.UP:
                pnjDirection = PotentialDirection.SEE_BAS;
                break;
            case PotentialDirection.DOWN:
                pnjDirection = PotentialDirection.SEE_HAUT;
                break;
            case PotentialDirection.LEFT:
                pnjDirection = PotentialDirection.SEE_DROITE;
                break;
            case PotentialDirection.RIGHT:
                pnjDirection = PotentialDirection.SEE_GAUCHE;
                break;
        }

        return pnjDirection;
    }
}
