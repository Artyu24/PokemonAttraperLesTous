using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Object.Data;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    #region Variables
    public static CombatManager Instance;

    private CombatState actualCombatState = CombatState.Init;

    public CombatState ActualCombatState { get => actualCombatState; set => actualCombatState = value; }

    public TeamPokes playerPokes; //pokemons du joueur
    public AttackDatabase attackDatabase; //scriptable de toutes les attaques du jeu
    public PokeDataBase pokeDataBase; //scriptable de tout les pokémons du jeu
    private Dictionary<int, PokeData> dictPokeData = new Dictionary<int, PokeData>(); //dictionnaire de tout les pokémons du jeu
    private Dictionary<int, AttackData> dictAttackData = new Dictionary<int, AttackData>(); //dictionnaire de toutes les attaques du jeu
    public Dictionary<int, PokeData> DictPokeData => dictPokeData;
    public Dictionary<int, AttackData> DictAttackData => dictAttackData;

    public GameObject combatWindow; //fenêtre de l'interface de combat
    public Text chatText; //zone de text qui affiche les infos, ex : trucmuche utilise charge !

    #region PokémonsUI
    [Header("PokémonsUI")]
    public Text playerPokémonName;
    public Slider playerPokémonHP;
    public Text playerPokémonHPText;
    public Image playerPokémonSprite;
    public Text enemiePokémonName;
    public Slider enemiePokémonHP;
    public Image enemiePokémonSprite;
    public GameObject pokemonWindow;
    public Text pokemonButton1;
    public Text pokemonButton2;
    public Text pokemonButton3;
    public Text pokemonButton4;
    public Text pokemonButton5;
    public Text pokemonButton6;
    #endregion

    #region AttacksInfo&UI
    [Header("Attacks")]
    public GameObject attackWindow;
    public Text attackButton1;
    public Text attackButton2;
    public Text attackButton3;
    public Text attackButton4;
    #endregion

    #region BoucleCombat
    [Header("Boucle")] 
    [Tooltip("false : tour de l'ordi, true : tour du joueur")]
    public PokeData playerPoke;
    public PokeData enemiePoke;
    private int playerAttackNbr;
    private int enemyAttackNbr;
    public int[,] types;
    #endregion

    #endregion

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        //remettre dans chaque database et utiliser le get
        if (pokeDataBase != null)
        {
            foreach (var pokeData in pokeDataBase.PokeData)
            {
                DictPokeData.Add(pokeData.ID, pokeData);
            }
        }
        else
        {
            Debug.Log("Il manque la database de pokémon");
        }


        if (attackDatabase != null)
        {
            foreach (var attackData in attackDatabase.AttackData)
            {
                dictAttackData.Add(attackData.ID, attackData);
            }
        }
        else
        {
            Debug.Log("Il manque la database d'attack");
        }
    }
    
    void Start()
    {
        if (playerPokes.pokes.Count == 0)
        {
            Debug.Log("Le joueur n'a pas de pokémon");
            return;
        }

        playerPoke = playerPokes.pokes[0];
        foreach (var poke in dictPokeData)
        {
            int i = 0;
            dictPokeData[i].hp = dictPokeData[i].hpMax;
            playerPokémonHP.value = playerPoke.hpMax;
            enemiePokémonHP.value = enemiePoke.hpMax;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Return();
        }

        //Debug.Log("Update combat state = " + actualCombatState);
        if (actualCombatState == CombatState.PlayerVictory)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameManager.Instance.ActualPlayerState = PlayerState.InMovement;
                GameManager.Instance.ActualGameState = GameState.Adventure;
                combatWindow.SetActive(false);
            }
        }
    }

    public void StartCombat(PokeData wild)
    {
        //INIT
        Debug.Log("Initialisation du combat : anim + textes");
        chatText.text = wild.name + " est apparu !!!";

        enemiePoke = wild;

        #region SetupUICombat
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
        #endregion

        actualCombatState = CombatState.PlayerChoose;
    }

    public void PlayerChoose(int attackNumber)
    {
        Debug.Log("Combat state Player choose = " + actualCombatState);
        if (actualCombatState == CombatState.PlayerChoose)
        {
            playerAttackNbr = attackNumber;
            attackWindow.SetActive(false);
            actualCombatState = CombatState.EnemyChoose;
            EnemyChoose();
        }
    }

    private void EnemyChoose()
    {
        Debug.Log("Combat state Enemy choose = " + actualCombatState);
        if (actualCombatState == CombatState.EnemyChoose)
        {
            int enemyAttackNbr = Random.Range(0, 5);
            actualCombatState = CombatState.PlayerAttack;
            PlayerAttack();
        }
    }

    public void PlayerAttack()
    {
        Debug.Log("Combat state Player Attack = " + actualCombatState);
        if (actualCombatState == CombatState.PlayerAttack)
        {
            //CheckType(enemiePoke, DictAttackData[playerPoke.attackIDlist[playerAttackNbr]]);
            enemiePokémonHP.value -= DictAttackData[playerPoke.attackIDlist[playerAttackNbr]].dmg;
            chatText.text = playerPokémonName.text + " utilise " + DictAttackData[playerPoke.attackIDlist[playerAttackNbr]].name + " !";
            if (enemiePokémonHP.value <= 0)
            {
                chatText.text = "Player win !!!";
                actualCombatState = CombatState.PlayerVictory;
            }
            else
            {
                actualCombatState = CombatState.EnemyAttack;
                EnemyAttack();
            }

        }
    }
    // pour l'instant ca va trop vite faudrait mettre des coroutines, ou jouer avec les animes et les events pour qu'on ai le temps de voir chaque truc qui se passe
    private void EnemyAttack()
    {
        Debug.Log("Combat state Enemy Attack = " + actualCombatState);
        if (actualCombatState == CombatState.EnemyAttack)
        {
            playerPokémonHP.value -= DictAttackData[enemiePoke.attackIDlist[enemyAttackNbr]].dmg;
            playerPoke.hp -= DictAttackData[enemiePoke.attackIDlist[enemyAttackNbr]].dmg;
            chatText.text = enemiePokémonName.text + " utilise " + DictAttackData[playerPoke.attackIDlist[enemyAttackNbr]].name + " !";
            if (playerPokémonHP.value <= 0)
            {
                chatText.text = "Enemy win !!!";
                actualCombatState = CombatState.PlayerVictory; //Changer pour EnemyVictory
            }
            else
            {
                actualCombatState = CombatState.PlayerChoose;
            }

        }
    }

    public void FlyFight()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.InMovement;
        GameManager.Instance.ActualGameState = GameState.Adventure;
        combatWindow.SetActive(false);
    }

    public void Return()
    {
        attackWindow.SetActive(false);
        pokemonWindow.SetActive(false);
    }


    //c'est pour les types avec le excel, mais j'ai pas eu le temps de finir l'integration du dit excel, en gros ca marche pas encore

    public void CheckType(PokeData pokeType, AttackData attackType)
    {
        Debug.Log((int) attackType.TYPE); //ordonnée
        Debug.Log((int) pokeType.TYPE); //abscisse
    }
}
