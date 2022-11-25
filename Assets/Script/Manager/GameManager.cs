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

    [Header("UI")]
    [SerializeField] private GameObject areaFrame;
    [SerializeField] private GameObject waterBox;
    [SerializeField] private GameObject waterAnimation;
    public GameObject AreaFrame => areaFrame;
    public GameObject WaterBox => waterBox;
    public GameObject WaterAnimation => waterAnimation;

    private GameState actualGameState = GameState.Adventure;
    private PlayerState actualPlayerState = PlayerState.Idle;
    public GameState ActualGameState { get => actualGameState; set => actualGameState = value; }
    public PlayerState ActualPlayerState { get => actualPlayerState; set => actualPlayerState = value; }

    [Header("Deplacement Player & PNJ")]
    private Dictionary<PotentialDirection, DirectionData> dictDirection = new Dictionary<PotentialDirection, DirectionData>();
    public Dictionary<PotentialDirection, DirectionData> DictDirection => dictDirection;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        ldGrid = FindObjectOfType<Grid>();
        moveDistance = ldGrid.cellSize.x;

        #region Init Direction Dictionnary

        dictDirection.Add(PotentialDirection.HAUT, new DirectionData(PotentialDirection.HAUT, "up", new Vector3(0, GameManager.Instance.GetMoveDistance, 0), transform.up));
        dictDirection.Add(PotentialDirection.BAS, new DirectionData(PotentialDirection.BAS, "bottom", new Vector3(0, -GameManager.Instance.GetMoveDistance, 0), -transform.up));
        dictDirection.Add(PotentialDirection.GAUCHE, new DirectionData(PotentialDirection.GAUCHE, "left", new Vector3(-GameManager.Instance.GetMoveDistance, 0, 0), -transform.right));
        dictDirection.Add(PotentialDirection.DROITE, new DirectionData(PotentialDirection.DROITE, "right", new Vector3(GameManager.Instance.GetMoveDistance, 0, 0), transform.right));
        dictDirection.Add(PotentialDirection.RIEN, new DirectionData(PotentialDirection.RIEN, "Idl"));
        dictDirection.Add(PotentialDirection.SEE_HAUT, new DirectionData(PotentialDirection.SEE_HAUT, "Idl_up"));
        dictDirection.Add(PotentialDirection.SEE_BAS, new DirectionData(PotentialDirection.SEE_BAS, "Idl_bottom"));
        dictDirection.Add(PotentialDirection.SEE_GAUCHE, new DirectionData(PotentialDirection.SEE_GAUCHE, "Idl_left"));
        dictDirection.Add(PotentialDirection.SEE_DROITE, new DirectionData(PotentialDirection.SEE_DROITE, "Idl_right"));

        #endregion
    }

    public void ActivateFade(int i)
    {
        if (i == 1)
            fadeAnim.SetTrigger("In");
        else if (i == 2)
            fadeAnim.SetTrigger("Out");
        else
            fadeAnim.SetTrigger("All");
        
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
