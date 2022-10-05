using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPos, endPos;

    public int moveSpeed = 20;

    public LayerMask collisionLayer;
    public float raycastDistance = 2;
    private bool northCollision, southCollision, eastCollision, westCollision;

    private bool isMovementFinish;
    private bool walkOnWater = false;

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
            if (Input.GetKey(KeyCode.F))
            {
                walkOnWater = true;
            }

            if (Input.GetKey(KeyCode.Z))
            {
                AnimPlayer(Direction.HAUT, "up");
                if (!northCollision)
                {
                    endPos = new Vector3(transform.position.x, transform.position.y + GameManager.Instance.GetMoveDistance, transform.position.z);
                    InMovement(new Vector3(0, GameManager.Instance.GetMoveDistance, 0));
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                AnimPlayer(Direction.BAS, "bottom");
                if (!southCollision)
                {
                    endPos = new Vector3(transform.position.x, transform.position.y - GameManager.Instance.GetMoveDistance, transform.position.z);
                    InMovement(new Vector3(0, -GameManager.Instance.GetMoveDistance, 0));

                }
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                AnimPlayer(Direction.GAUCHE, "left");
                if (!westCollision)
                {
                    endPos = new Vector3(transform.position.x - GameManager.Instance.GetMoveDistance, transform.position.y, transform.position.z);
                    InMovement(new Vector3(-GameManager.Instance.GetMoveDistance, 0, 0));
                }


            }
            else if (Input.GetKey(KeyCode.D))
            {
                AnimPlayer(Direction.DROITE, "right");
                if (!eastCollision)
                {
                    endPos = new Vector3(transform.position.x + GameManager.Instance.GetMoveDistance, transform.position.y, transform.position.z);
                    InMovement(new Vector3(GameManager.Instance.GetMoveDistance, 0, 0));
                }
            }
        }

        if (transform.position == endPos && GameManager.Instance.ActualGameState == GameState.Adventure && GameManager.Instance.ActualPlayerState == PlayerState.PlayerInMovement)
        {
            endPos = GetComponent<BoxCenter>().CenterObject();
            transform.position = endPos;
            //anim.SetBool("Walking", false);
            CheckWall();

            GameManager.Instance.ActualPlayerState = PlayerState.PlayerStartMove;
            isMovementFinish = true;

            isTP = false;
            
            
            if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.S))
            {
                anim.SetTrigger("Idl");
                lastAnim = Direction.RIEN;
            }

        }
        else if (transform.position != endPos)
        {
            isMovementFinish = false;
        }

        if (GameManager.Instance.ActualGameState != GameState.Paused && GameManager.Instance.ActualPlayerState == PlayerState.PlayerInMovement)
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.blue);
        Debug.DrawRay(transform.position, -transform.up * raycastDistance, Color.blue);
        Debug.DrawRay(transform.position, -transform.right * raycastDistance, Color.blue);
        Debug.DrawRay(transform.position, transform.right * raycastDistance, Color.blue);

    }

    private void InMovement(Vector3 dir)
    {
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
        lastDir = dir;

        //anim.SetBool("Walking", true);
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

        //Debug.Log(northCollision + "/" + southCollision + "/" + westCollision + "/" + eastCollision);
    }

    private void Check(Vector3 direction, ref bool isCollision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, collisionLayer);
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                isCollision = true;
                return;
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water") && !walkOnWater)
            {
                isCollision = true;
                return;
            }
        }
        
        isCollision = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            walkOnWater = false;
        }
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
            
            StartCoroutine(WaitTP());
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