using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public TeamPokes playerPokes;
    public AttackDatabase attackDatabase;
    public PokeDataBase pokeDataBase;
    public GameObject combatWindow;

    [Header("Pok�monsUI")]
    public Text playerPok�monName;
    public Slider playerPok�monHP;
    public Text playerPok�monHPText;
    public Image playerPok�monSprite;
    public Text enemiePok�monName;
    public Slider enemiePok�monHP;
    public Image enemiePok�monSprite;

    [Header("Attacks")]
    public Text chatText;
    public GameObject attackWindow;
    public Text attackButton1;
    public Text attackButton2;
    public Text attackButton3;
    public Text attackButton4;
    
    [Header("Pok�mons")]
    public GameObject pokemonWindow;
    public Text pokemonButton1;
    public Text pokemonButton2;
    public Text pokemonButton3;
    public Text pokemonButton4;
    public Text pokemonButton5;
    public Text pokemonButton6;

    [Header("Boucle")] 
    [Tooltip("false : tour de l'ordi, true : tour du joueur")]
    public bool aQuiLeTour;
    public PokeData playerPoke;
    public PokeData enemiePoke;


    private Dictionary<int, PokeData> dictPokeData = new Dictionary<int, PokeData>();
    private Dictionary<int, AttackData> dictAttackData = new Dictionary<int, AttackData>();

    public Dictionary<int, PokeData> DictPokeData => dictPokeData;
    public Dictionary<int, AttackData> DictAttackData => dictAttackData;

    void Awake()
    {
        if (Instance == null)
            Instance = this; 
        foreach (var pokeData in pokeDataBase.PokeData)
        {
            DictPokeData.Add(pokeData.ID, pokeData);
        }
        foreach (var attackData in attackDatabase.AttackData)
        {
            Debug.Log("Ajouter au dicossssssss");
            dictAttackData.Add(attackData.ID, attackData);
        }
    }

    void Start()
    {
        enemiePoke = DictPokeData[Random.Range(0,6)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Return();
        }

        /*if (playerPoke.speed < enemiePoke.speed)
        {
            aQuiLeTour = false;
        }
        else
        {
            aQuiLeTour = true;
        }

        switch (aQuiLeTour)
        {
            case false:
                int atkId = Random.Range(0,4);
                Debug.Log("Le pok�mon adverse � utilis� " + DictAttackData[enemiePoke.attackIDlist[atkId]]);
                aQuiLeTour = true;
                break;
            case true:
                aQuiLeTour = false;
                break;
        }*/
    }

    public void StartCombat(PokeData wild)
    {
        playerPoke = playerPokes.poke1;
        enemiePok�monName.text = wild.name;
        enemiePok�monHP.value = wild.hp;
        enemiePok�monHP.maxValue = wild.hp;
        enemiePok�monSprite.sprite = wild.sprite;

        playerPok�monName.text = playerPoke.name;
        playerPok�monHPText.text = playerPoke.hp + "/" + playerPoke.hpMax;
        playerPok�monHP.value = playerPoke.hp;
        playerPok�monHP.maxValue = playerPoke.hp;
        playerPok�monSprite.sprite = playerPoke.sprite;

        attackButton1.text = DictAttackData[playerPoke.attackIDlist[0]].name;
        attackButton2.text = DictAttackData[playerPoke.attackIDlist[1]].name;
        attackButton3.text = DictAttackData[playerPoke.attackIDlist[2]].name;
        attackButton4.text = DictAttackData[playerPoke.attackIDlist[3]].name;

        chatText.text = wild.name + " est apparu !!!";
    }

    public void FlyFight()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
        GameManager.Instance.ActualGameState = GameState.Adventure;
        combatWindow.SetActive(false);
    }

    public void Attack(int attackNumber)
    {
        chatText.text = playerPok�monName.text + " utilise " + DictAttackData[playerPoke.attackIDlist[attackNumber]].name + " !";
        enemiePok�monHP.value -= DictAttackData[playerPoke.attackIDlist[attackNumber]].dmg;
        attackWindow.SetActive(false);
        if (enemiePok�monHP.value <= 0)
        {
            GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
            GameManager.Instance.ActualGameState = GameState.Adventure;
            combatWindow.SetActive(false);
        }
    }

    public void Return()
    {
        pokemonWindow.SetActive(false);
    }
}
