using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Different Area")] 
    [SerializeField] private GameObject waterPokemon;
    private bool inGrass = false;
    private HerbesHautes herbesHautes;
    private bool walkOnWater = false;
    public GameObject WaterPokemon => waterPokemon;
    public bool WalkOnWater { get => walkOnWater; set => walkOnWater = value; }

    [Header("Teleportation")]
    private bool isTP = false;
    private Collider2D actualDoor;
    private PotentialDirection lastDirEnum = PotentialDirection.BAS;

    [Header("Movement")]
    public int moveSpeed = 20;
    private Vector3 endPos;
    private Vector2 inputDir;
    private BoxCenter boxCenter;
    private Dictionary<PotentialDirection, DirectionData> dictDirection = new Dictionary<PotentialDirection, DirectionData>();

    [Header("Interaction")]
    public LayerMask wallLayer;
    public LayerMask waterLayer;
    public LayerMask interactLayer;
    public float raycastDistance = 2;

    public delegate void InteractionDelegate();
    private InteractionDelegate actualInteractionDelegate = null;
    public InteractionDelegate ActualInteractionDelegate { get => actualInteractionDelegate; set => actualInteractionDelegate = value; }

    [Header("Animation")]
    public Animator anim;
    private Animator animPokeWater;
    private PotentialDirection lastAnim = PotentialDirection.RIEN;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        boxCenter = GetComponent<BoxCenter>();
        if(waterPokemon != null)
            animPokeWater = waterPokemon.GetComponent<Animator>();

        if (SaveSystemManager.Instance.LastPosPlayer != Vector3.zero)
        {
            transform.position = SaveSystemManager.Instance.LastPosPlayer;
        }
        endPos = boxCenter.CenterObject();

        #region Init Direction Dictionnary

        dictDirection.Add(PotentialDirection.HAUT, new DirectionData(PotentialDirection.HAUT, "up", new Vector3(0, GameManager.Instance.GetMoveDistance, 0), transform.up));
        dictDirection.Add(PotentialDirection.BAS, new DirectionData(PotentialDirection.BAS, "bottom", new Vector3(0, -GameManager.Instance.GetMoveDistance, 0), -transform.up));
        dictDirection.Add(PotentialDirection.GAUCHE, new DirectionData(PotentialDirection.GAUCHE, "left", new Vector3(-GameManager.Instance.GetMoveDistance, 0, 0), -transform.right));
        dictDirection.Add(PotentialDirection.DROITE, new DirectionData(PotentialDirection.DROITE, "right", new Vector3(GameManager.Instance.GetMoveDistance, 0, 0), transform.right));
        dictDirection.Add(PotentialDirection.RIEN, new DirectionData(PotentialDirection.RIEN, "Idl"));

        #endregion

        actualInteractionDelegate = LaunchInteraction;
    }


    private void Update()
    {
        #region MOVEMENT

        #region Input

        if (GameManager.Instance.ActualGameState == GameState.Adventure && GameManager.Instance.ActualPlayerState == PlayerState.Idle)
        {
            PotentialDirection dirEnum = PotentialDirection.RIEN;
            if (inputDir.y > 0)
                dirEnum = PotentialDirection.HAUT;
            else if (inputDir.y < 0)
                dirEnum = PotentialDirection.BAS;
            else if (inputDir.x < 0)
                dirEnum = PotentialDirection.GAUCHE;
            else if (inputDir.x > 0)
                dirEnum = PotentialDirection.DROITE;

            DirectionData dirChoose = dictDirection[dirEnum];
            AnimPlayer(dirChoose.dirEnum, dirChoose.animName);

            if (dirEnum != PotentialDirection.RIEN)
            {
                lastDirEnum = dirEnum;
                
                if (!Check(dirChoose.transformDir))
                {
                    endPos = transform.position + dirChoose.mouv;

                    GameManager.Instance.ActualPlayerState = PlayerState.InMovement;
                }
                else
                {
                    //BONK
                    if (FindObjectOfType<AudioManager>() != null)
                    {
                        FindObjectOfType<AudioManager>().Play("BlockSound");
                    }
                }
            }
        }

        #endregion

        #region End Movement

        if (transform.position == endPos && GameManager.Instance.ActualGameState == GameState.Adventure && GameManager.Instance.ActualPlayerState == PlayerState.InMovement)
        {
            endPos = boxCenter.CenterObject();
            transform.position = endPos;

            GameManager.Instance.ActualPlayerState = PlayerState.Idle;

            isTP = false;
            if (herbesHautes != null)
            {
                herbesHautes.SpawnPokemon();
            }
        }

        #endregion

        #region In Movement

        if (GameManager.Instance.ActualGameState != GameState.Paused && GameManager.Instance.ActualPlayerState == PlayerState.InMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            if(waterPokemon != null)
                if(waterPokemon.activeInHierarchy && walkOnWater)
                    waterPokemon.transform.position = transform.position;
        }

        #endregion

        #endregion
    }

    #region Input Action
    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();

        if (ctx.started)
        {
            if (GameManager.Instance.ActualPlayerState == PlayerState.WaterInteraction && GameManager.Instance.ActualGameState == GameState.Adventure)
            {
                if(Math.Abs(inputDir.y) > 0)
                    WaterZone.Instance.SwitchText();
            }

            if (GameManager.Instance.ActualGameState == GameState.Paused)
            {
                if (Math.Abs(inputDir.y) > 0)
                    PauseMenu.Instance.SwitchPauseSelection(inputDir.y);
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (actualInteractionDelegate != null)
            {
                DirectionData dirChoose = dictDirection[PotentialDirection.RIEN];
                AnimPlayer(dirChoose.dirEnum, dirChoose.animName);

                actualInteractionDelegate();

                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("SFXMenuClick");
                }
            }
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.ActualGameState == GameState.Adventure || GameManager.Instance.ActualGameState == GameState.Paused)
            {
                PauseMenu.Instance.OpenPauseMenu();
            }
        }
    }

    #endregion

    #region Animation

    private void AnimPlayer(PotentialDirection actualDir, string animTrigger)
    {
        if (lastAnim != actualDir)
        {
            anim.SetTrigger(animTrigger);
            if (waterPokemon != null)
                if (waterPokemon.activeInHierarchy && animTrigger != "Idl" && walkOnWater)
                    animPokeWater.SetTrigger(animTrigger);
        }
        lastAnim = actualDir;
    }

    #endregion

    #region Check Collision

    private bool Check(Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.blue, 2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, wallLayer);
        if (hit.collider != null)
        {
            //if(FindObjectOfType<AudioManager>() != null)
            //    FindObjectOfType<AudioManager>().Play("BlockSound");
            
            return true;
        }

        RaycastHit2D hitWater = Physics2D.Raycast(transform.position, direction, raycastDistance, waterLayer);
        if (hitWater.collider != null)
        {
            if (walkOnWater)
            {
                WaterZone.Instance.ActivateExitAnimation();      
            }
            return true;
        }
        return false;
    }

    #endregion

    #region Interaction

    public void ResetInteractionFunction()
    {
        actualInteractionDelegate = LaunchInteraction;
    }

    private void LaunchInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dictDirection[lastDirEnum].transformDir, raycastDistance, interactLayer);
        Debug.DrawRay(transform.position, dictDirection[lastDirEnum].transformDir * raycastDistance, Color.red, 2f);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                if (walkOnWater && hit.collider.gameObject.layer == 4)
                    return;

                hit.collider.GetComponent<IInteractable>().Interact();

                

                //SON TICK DE DIALOGUE
            }
        }
    }

    #endregion

    #region Enter in Door or Grass (Trigger)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Door") && !isTP)
        {
            isTP = true;
            actualDoor = collision;

            GameManager.Instance.ActualPlayerState = PlayerState.Teleportation;
            GameManager.Instance.ActivateFade(true);
            
            anim.SetTrigger("Idl");
            lastAnim = PotentialDirection.RIEN;

            if (FindObjectOfType<AudioManager>() != null)
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

    #endregion

    #region Teleportation in Building

    private IEnumerator WaitTP()
    {
        yield return new WaitForSeconds(1);
        TeleportPlayer();
    }

    private void TeleportPlayer()
    {
        transform.position = TP_Manager.Instance.DictHouseDoor[actualDoor].transform.position;
        endPos = GetComponent<BoxCenter>().CenterObject() + dictDirection[lastDirEnum].mouv;
        transform.position = GetComponent<BoxCenter>().CenterObject();
        GameManager.Instance.ActivateFade(false);
        GameManager.Instance.ActualPlayerState = PlayerState.InMovement;
    }

    #endregion

    #region Water Pokemon

    public void InitWaterPokemon()
    {
        DirectionData lastDirData = GetLastDirection();
        waterPokemon.transform.position = transform.position + lastDirData.mouv;
        waterPokemon.SetActive(true);
        animPokeWater.SetTrigger(lastDirData.animName);
    }

    #endregion
    
    public DirectionData GetLastDirection()
    {
        return dictDirection[lastDirEnum];
    }
}

#region Direction Data Class

public class DirectionData
{
    public PotentialDirection dirEnum;
    public string animName;
    public Vector3 mouv;
    public Vector3 transformDir;

    public DirectionData(PotentialDirection dirEnum, string animName, Vector3 mouv, Vector3 transformDir)
    {
        this.dirEnum = dirEnum;
        this.animName = animName;
        this.mouv = mouv;
        this.transformDir = transformDir;
    }

    public DirectionData(PotentialDirection dirEnum, string animName)
    {
        this.dirEnum = dirEnum;
        this.animName = animName;
        mouv = Vector3.zero;
        transformDir = Vector3.zero;
    }
}

#endregion