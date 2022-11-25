using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PNJ : MonoBehaviour, IInteractable
{
    //private PotentialDirection lastDirEnum = PotentialDirection.BAS;

    [Header("Movement")]
    public int moveSpeed = 20;
    private Vector3 endPos;
    private Vector3 startPos;
    private BoxCenter boxCenter;
    [SerializeField] private float fixMouvement; 
    [SerializeField] private List<PotentialDirection> pnjDir = new List<PotentialDirection>();
    private int pnjIndex = 0;
    private PNJState actualPnjState = PNJState.Idle;
    public PNJState ActualPnjState { get => actualPnjState; set => actualPnjState = value; }
    private bool canStart = true;
    private BoxCollider2D collider;

    [Header("Interaction")]
    public LayerMask playerLayer;
    public float raycastDistance = 2;
    [SerializeField] private DialogueID[] dialogues;

    [Header("Animation")]
    public Animator anim;
    private PotentialDirection lastAnim = PotentialDirection.RIEN;

    private void Start()
    {
        boxCenter = GetComponent<BoxCenter>();
        endPos = boxCenter.CenterObject();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        #region MOVEMENT

        #region Input

        if (canStart && GameManager.Instance.ActualGameState == GameState.Adventure && actualPnjState == PNJState.Idle)
        {
            canStart = false;
            StartCoroutine(PNJMouv());
        }

        if (actualPnjState == PNJState.Interaction)
        {
            canStart = true;
            StopAllCoroutines();
        }

        #endregion

        #region End Movement

        if (transform.position == endPos && GameManager.Instance.ActualGameState == GameState.Adventure && actualPnjState == PNJState.InMovement)
        {
            endPos = boxCenter.CenterObject();
            transform.position = endPos;
            actualPnjState = PNJState.Idle;
            NextMove(PotentialDirection.RIEN);
        }

        #endregion

        #region In Movement

        if (GameManager.Instance.ActualGameState != GameState.Paused && actualPnjState == PNJState.InMovement)
        {
            #region PNJ Reset Pos if player move on the same place

            int lastIndex = pnjIndex - 1;
            if (lastIndex < 0)
                lastIndex = pnjDir.Count - 1;

            DirectionData dirChoose = GameManager.Instance.DictDirection[pnjDir[lastIndex]];

            if (Check(dirChoose.transformDir))
            {
                transform.position = startPos;
                endPos = startPos;
                actualPnjState = PNJState.Idle;
                pnjIndex = lastIndex;
                NextMove(PotentialDirection.RIEN);
            }

            #endregion
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            collider.offset = endPos - transform.position;
        }

        #endregion

        #endregion
    }

    #region MOVEMENT FUNCTION

    private void NextMove(PotentialDirection dirEnum)
    {
        DirectionData dirChoose = GameManager.Instance.DictDirection[dirEnum];
        AnimPNJ(dirChoose.dirEnum, dirChoose.animName);

        if (dirEnum == PotentialDirection.HAUT || dirEnum == PotentialDirection.BAS || dirEnum == PotentialDirection.DROITE || dirEnum == PotentialDirection.GAUCHE)
        {
            if (!Check(dirChoose.transformDir))
            {
                endPos = transform.position + dirChoose.mouv;
                startPos = transform.position;
                actualPnjState = PNJState.InMovement;
                collider.offset = endPos - transform.position;
            }
        }
    }

    private IEnumerator PNJMouv()
    {
        float random = 0 ;

        if (fixMouvement == 0)
            random = Random.Range(2f, 8f);
        else
            random = fixMouvement;
        
        yield return new WaitForSeconds(random);

        if (actualPnjState == PNJState.Idle)
        {
            DirectionData dirChoose = GameManager.Instance.DictDirection[pnjDir[pnjIndex]];
            if (!Check(dirChoose.transformDir))
            {
                NextMove(pnjDir[pnjIndex]);
                
                if (pnjIndex + 1 == pnjDir.Count)
                    pnjIndex = 0;
                else
                    pnjIndex++;
            }
        }
        canStart = true;
    }

    #endregion

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

    #region Check Collision

    private bool Check(Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.cyan, 2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, playerLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    #endregion

    public void Interact()
    {
        if (actualPnjState == PNJState.Idle)
        {
            actualPnjState = PNJState.Interaction;

            NextMove(GetOppositePosition(PlayerMovement.Instance.LastDirEnum));

            GameManager.Instance.ActualPlayerState = PlayerState.Interaction;
            PlayerMovement.Instance.ActualInteractionDelegate = null;
            DialogueManager.Instance.InitDialogue(this, dialogues);
        }
    }

    private PotentialDirection GetOppositePosition(PotentialDirection playerDir)
    {
        PotentialDirection pnjDirection = PotentialDirection.RIEN;
        
        switch (playerDir)
        {
            case PotentialDirection.HAUT:
                pnjDirection = PotentialDirection.SEE_BAS;
                break;
            case PotentialDirection.BAS:
                pnjDirection = PotentialDirection.SEE_HAUT;
                break;
            case PotentialDirection.GAUCHE:
                pnjDirection = PotentialDirection.SEE_DROITE;
                break;
            case PotentialDirection.DROITE:
                pnjDirection = PotentialDirection.SEE_GAUCHE;
                break;
        }

        return pnjDirection;
    }
}
