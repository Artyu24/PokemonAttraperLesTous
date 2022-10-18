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

    [Header("PokémonsUI")]
    public Text playerPokémonName;
    public Slider playerPokémonHP;
    public Text playerPokémonHPText;
    public Image playerPokémonSprite;
    public Text enemiePokémonName;
    public Slider enemiePokémonHP;
    public Image enemiePokémonSprite;

    [Header("Attacks")]
    public Text chatText;
    public GameObject attackWindow;
    public Text attackButton1;
    public Text attackButton2;
    public Text attackButton3;
    public Text attackButton4;
    
    [Header("Pokémons")]
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
                Debug.Log("Le pokémon adverse à utilisé " + DictAttackData[enemiePoke.attackIDlist[atkId]]);
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
        enemiePokémonName.text = wild.name;
        enemiePokémonHP.value = wild.hp;
        enemiePokémonHP.maxValue = wild.hp;
        enemiePokémonSprite.sprite = wild.sprite;

        playerPokémonName.text = playerPoke.name;
        playerPokémonHPText.text = playerPoke.hp + "/" + playerPoke.hpMax;
        playerPokémonHP.value = playerPoke.hp;
        playerPokémonHP.maxValue = playerPoke.hp;
        playerPokémonSprite.sprite = playerPoke.sprite;

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
        chatText.text = playerPokémonName.text + " utilise " + DictAttackData[playerPoke.attackIDlist[attackNumber]].name + " !";
        enemiePokémonHP.value -= DictAttackData[playerPoke.attackIDlist[attackNumber]].dmg;
        attackWindow.SetActive(false);
        if (enemiePokémonHP.value <= 0)
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
