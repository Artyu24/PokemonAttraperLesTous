using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Grid ldGrid;
    private float moveDistance = 1;
    public float GetMoveDistance => moveDistance;

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
}
