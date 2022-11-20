using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPos, endPos;

    private Vector2 inputDir;
    public int moveSpeed = 20;

    public LayerMask wallLayer;
    public LayerMask waterLayer;
    public LayerMask interactLayer;
    public float raycastDistance = 2;
    private bool northCollision, southCollision, eastCollision, westCollision;

    private bool isMovementFinish;
    private bool inWater = false;
    private bool inGrass = false;
    private HerbesHautes herbesHautes;
    private bool walkOnWater = false;
    public bool WalkOnWater { get => walkOnWater; set => walkOnWater = value; }

    private bool isTP = false;
    private Collider2D actualDoor;
    private Vector3 lastDir = Vector3.zero;

    public Animator anim;

    enum Direction
    {
        HAUT,
        BAS,
        GAUCHE, 
        DROITE,
        RIEN    
    }
    private Direction lastAnim = Direction.RIEN;

    private void Start()
    {
        endPos = GetComponent<BoxCenter>().CenterObject();
        CheckWall();
        isMovementFinish = true;
    }


    private void Update()
    {
        CheckWall();

        if (isMovementFinish && GameManager.Instance.ActualGameState == GameState.Adventure && GameManager.Instance.ActualPlayerState == PlayerState.PlayerStartMove)
        {
            if (inputDir.y > 0)
            {
                AnimPlayer(Direction.HAUT, "up");
                if (!northCollision)
                {
                    endPos = new Vector3(transform.position.x, transform.position.y + GameManager.Instance.GetMoveDistance, transform.position.z);
                    InMovement(new Vector3(0, GameManager.Instance.GetMoveDistance, 0));
                }
            }
            else if (inputDir.y < 0)
            {
                AnimPlayer(Direction.BAS, "bottom");
                if (!southCollision)
                {
                    endPos = new Vector3(transform.position.x, transform.position.y - GameManager.Instance.GetMoveDistance, transform.position.z);
                    InMovement(new Vector3(0, -GameManager.Instance.GetMoveDistance, 0));
                }
            }
            else if (inputDir.x < 0)
            {
                AnimPlayer(Direction.GAUCHE, "left");
                if (!westCollision)
                {
                    endPos = new Vector3(transform.position.x - GameManager.Instance.GetMoveDistance, transform.position.y, transform.position.z);
                    InMovement(new Vector3(-GameManager.Instance.GetMoveDistance, 0, 0));
                }
            }
            else if (inputDir.x > 0)
            {
                AnimPlayer(Direction.DROITE, "right");
                if (!eastCollision)
                {
                    endPos = new Vector3(transform.position.x + GameManager.Instance.GetMoveDistance, transform.position.y, transform.position.z);
                    InMovement(new Vector3(GameManager.Instance.GetMoveDistance, 0, 0));
                }
            }
            else
                AnimPlayer(Direction.RIEN, "Idl");
        }

        if (transform.position == endPos && GameManager.Instance.ActualGameState == GameState.Adventure && GameManager.Instance.ActualPlayerState == PlayerState.PlayerInMovement)
        {
            endPos = GetComponent<BoxCenter>().CenterObject();
            transform.position = endPos;
            CheckWall();

            GameManager.Instance.ActualPlayerState = PlayerState.PlayerStartMove;
            isMovementFinish = true;

            isTP = false;
            if (herbesHautes != null)
            {
                herbesHautes.SpawnPokemon();
            }
        }
        else if (transform.position != endPos)
        {
            isMovementFinish = false;
        }


        if (GameManager.Instance.ActualGameState != GameState.Paused && GameManager.Instance.ActualPlayerState == PlayerState.PlayerInMovement)
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

        //Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.blue);
        //Debug.DrawRay(transform.position, -transform.up * raycastDistance, Color.blue);
        //Debug.DrawRay(transform.position, -transform.right * raycastDistance, Color.blue);
        //Debug.DrawRay(transform.position, transform.right * raycastDistance, Color.blue);
    }
    public void OnMove(InputAction.CallbackContext ctx) => inputDir = ctx.ReadValue<Vector2>();

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            RaycastHit2D hit = new RaycastHit2D();
            if (lastDir == new Vector3(0, GameManager.Instance.GetMoveDistance, 0))
            {
                hit = Physics2D.Raycast(transform.position, transform.up, raycastDistance, interactLayer);
                Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.yellow);
            }
            else if (lastDir == new Vector3(0, -GameManager.Instance.GetMoveDistance, 0))
            {
                hit = Physics2D.Raycast(transform.position, -transform.up, raycastDistance, interactLayer);
                Debug.DrawRay(transform.position, -transform.up * raycastDistance, Color.yellow);
            }
            else if (lastDir == new Vector3(GameManager.Instance.GetMoveDistance, 0, 0))
            {
                hit = Physics2D.Raycast(transform.position, transform.right, raycastDistance, interactLayer);
                Debug.DrawRay(transform.position, transform.right * raycastDistance, Color.yellow);
            }
            else if (lastDir == new Vector3(-GameManager.Instance.GetMoveDistance, 0, 0))
            {
                hit = Physics2D.Raycast(transform.position, -transform.right, raycastDistance, interactLayer);
                Debug.DrawRay(transform.position, -transform.right * raycastDistance, Color.yellow);
            }

            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<IInteractable>() != null)
                    hit.collider.GetComponent<IInteractable>().Interact();
            }
        }
    }

    private void InMovement(Vector3 dir)
    {
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
        lastDir = dir;
    }

    private void AnimPlayer(Direction actualDir, string animTrigger)
    {
        if (lastAnim != actualDir)
        {
            anim.SetTrigger(animTrigger);
        }
        lastAnim = actualDir;
    }

    private void CheckWall()
    {
        Check(transform.up, ref northCollision);
        Check(-transform.up, ref southCollision);
        Check(-transform.right, ref westCollision);
        Check(transform.right, ref eastCollision);
    }

    private void Check(Vector3 direction, ref bool isCollision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, wallLayer);
        if (hit.collider != null)
        {
            isCollision = true;
            FindObjectOfType<AudioManager>().Play("BlockSound");
            return;
        }


        if (!walkOnWater)
        {
            RaycastHit2D hitWater = Physics2D.Raycast(transform.position, direction, raycastDistance, waterLayer);
            if (hitWater.collider != null)
            {
                isCollision = true;
                return;
            }
        }
        
        isCollision = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Door") && !isTP)
        {
            isTP = true;
            actualDoor = collision;

            GameManager.Instance.ActualPlayerState = PlayerState.PlayerEnterTP;
            GameManager.Instance.ActivateFade(true);
            
            anim.SetTrigger("Idl");
            lastAnim = Direction.RIEN;
            FindObjectOfType<AudioManager>().Play("DoorSound");
            StartCoroutine(WaitTP());
        }

        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            if (!inGrass)
            {
                inGrass = true;
                herbesHautes = collision.GetComponent<HerbesHautes>();
            }
            else
            {
                inGrass = false;
                herbesHautes = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(collision.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (!inWater)
                inWater = true;
            else
            {
                inWater = false;
                walkOnWater = false;
            }

        }
    }

    private IEnumerator WaitTP()
    {
        yield return new WaitForSeconds(1);
        TeleportPlayer();
    }

    private void TeleportPlayer()
    {
        transform.position = TP_Manager.Instance.DictHouseDoor[actualDoor].transform.position;
        endPos = GetComponent<BoxCenter>().CenterObject() + lastDir;
        transform.position = GetComponent<BoxCenter>().CenterObject();
        GameManager.Instance.ActivateFade(false);
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
    }
}