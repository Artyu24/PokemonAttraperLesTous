using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Grid ldGrid;
    private float moveDistance = 1;
    public float GetMoveDistance => moveDistance;

    [SerializeField] private Animator fadeAnim;
    public GameObject AreaFrame;

    private GameState actualGameState = GameState.Adventure;
    private PlayerState actualPlayerState = PlayerState.PlayerStartMove;
    public GameState ActualGameState { get => actualGameState; set => actualGameState = value; }
    public PlayerState ActualPlayerState { get => actualPlayerState; set => actualPlayerState = value; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        ldGrid = FindObjectOfType<Grid>();
        moveDistance = ldGrid.cellSize.x;
    }

    public void ActivateFade(bool trigger)
    {
        if (trigger)
            fadeAnim.SetTrigger("In");
        else
            fadeAnim.SetTrigger("Out");
    }
}
