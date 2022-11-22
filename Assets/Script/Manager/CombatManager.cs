using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Object.Data;
using UnityEngine;
using UnityEngine.LowLevel;
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

    public Dialogue dialogue;
    #endregion

    #region Anims

    public bool isInHerbeHautes = false;
    public Animator combatAnimator;
    //public Animator playerAnimator;


    #endregion

    #region AttacksInfo&UI
    [Header("Attacks")]
    public GameObject attackWindow;
    #endregion

    #region BoucleCombat
    [Header("Boucle")] 
    [Tooltip("false : tour de l'ordi, true : tour du joueur")]
    public Pokemon playerPoke;
    public Pokemon enemiePoke;
    private int playerAttackNbr;
    private int enemyAttackNbr;
    public int[,] types;
    #endregion

    public delegate void combatDelegate();
    public Dictionary<CombatState, combatDelegate> combatDictionaire = new Dictionary<CombatState, combatDelegate>();
    private List<CombatState> combatStates;
    public float actionTime = 5.0f;
    public Button[] attackButtons = new Button[4];
    private Text[] attackButtonsText = new Text[4];

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
        combatDictionaire.Add(CombatState.PlayerAttack, PlayerAttack);
        combatDictionaire.Add(CombatState.PlayerDeath, Victory);
        combatDictionaire.Add(CombatState.EnemyAttack, EnemyAttack);
        combatDictionaire.Add(CombatState.EnemyDeath, Victory);
        combatDictionaire.Add(CombatState.PlayerChoose, PlayerChoose);

        for (int i = 0; i < attackButtons.Length; i++)
        {
            attackButtonsText[i] = attackButtons[i].GetComponentInChildren<Text>();
        }

        if (playerPokes.pokes.Count == 0)
        {
            Debug.Log("Le joueur n'a pas de pokémon");
            return;
        }

        playerPoke = new Pokemon(playerPokes.pokes[0], true);
        
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
        enemiePoke = new Pokemon (wild, false);
        chatText.text = wild.name + " est apparu !!!";

        foreach (var poke in dictPokeData)
        {
            int i = 0;
            dictPokeData[i].hp = dictPokeData[i].hpMax;
            playerPokémonHP.value = playerPoke.data.hpMax;
            enemiePokémonHP.value = enemiePoke.data.hpMax;
        }

        //Play anim enemie/player spawn
        #region SetupUICombat

        enemiePokémonName.text = enemiePoke.data.name;
        enemiePokémonHP.value = enemiePoke.data.hp;
        enemiePokémonHP.maxValue = enemiePoke.data.hp;
        playerPokémonName.text = playerPoke.data.name;
        playerPokémonHPText.text = playerPoke.data.hp + "/" + playerPoke.data.hpMax;
        playerPokémonHP.value = playerPoke.data.hp;
        playerPokémonHP.maxValue = playerPoke.data.hp;
        
        playerPokémonSprite.sprite = playerPoke.data.sprite;
        enemiePokémonSprite.sprite = enemiePoke.data.sprite;

        for (int i = 0; i < attackButtonsText.Length; i++)
        {
            attackButtonsText[i].text = DictAttackData[playerPoke.data.attackIDlist[i]].name;
        }

        #endregion

        if (combatAnimator != null)
        {
            combatAnimator.SetTrigger("EnemyThrowPoke");
            combatAnimator.SetTrigger("EnemyPokeAppear");
            combatAnimator.SetTrigger("PlayerThrowPoke");
            combatAnimator.SetTrigger("PlayerPokeApper");
        }
    }

    public void PlayerChoose()
    {
        int attackID = Array.IndexOf(attackButtons, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());

        playerPoke.attackId = attackID;

        EnemyChoose();
    }

    private void EnemyChoose()
    {
        // RANDOM pour trouver son attack

        PresShotRound();
    }

    private void PresShotRound()
    {
        Pokemon fastestPoke;
        Pokemon slowestPoke;
        combatStates.Clear();

        if (playerPoke.data.speed > enemiePoke.data.speed)
        {
            fastestPoke = playerPoke;
            slowestPoke = enemiePoke;
            combatStates.Add(CombatState.PlayerAttack);
        }
        else
        {
            fastestPoke = enemiePoke;
            slowestPoke = playerPoke;
            combatStates.Add(CombatState.EnemyAttack);
        }

        bool isDead = false;
        if (IsDead(slowestPoke.data.hp, DictAttackData[fastestPoke.data.attackIDlist[playerAttackNbr]].dmg))
            isDead = true;

        if(fastestPoke.isPlayer)
        {
            if(isDead)
                combatStates.Add(CombatState.EnemyDeath);
            else
                combatStates.Add(CombatState.EnemyAttack);
        }
        else
        {
            if (isDead)
                combatStates.Add(CombatState.PlayerDeath);
            else
                combatStates.Add(CombatState.PlayerAttack);
        }

        if (combatStates[combatStates.Count - 1] != CombatState.EnemyDeath && combatStates[combatStates.Count - 1] != CombatState.PlayerDeath)
        {
            isDead = false;
            if (IsDead(slowestPoke.data.hp, DictAttackData[fastestPoke.data.attackIDlist[playerAttackNbr]].dmg))
                isDead = true;

            if (slowestPoke.isPlayer)
            {
                if (isDead)
                    combatStates.Add(CombatState.EnemyDeath);
                else
                    combatStates.Add(CombatState.PlayerChoose);
            }
            else
            {
                if (isDead)
                    combatStates.Add(CombatState.PlayerDeath);
                else
                    combatStates.Add(CombatState.PlayerChoose);
            }
        }
        StartCoroutine(PlayRound());
    }

    private bool IsDead(int pokeLife, int attackDmg)
    {
        if (pokeLife <= attackDmg)
            return true;
        return false;
    }

    private IEnumerator PlayRound()
    {
        foreach (CombatState combatState in combatStates)
        {
            actualCombatState = combatState;
            combatDictionaire[combatState](); //Appel des fonctions dans l'ordre defini par PreShotRound
            yield return new WaitForSeconds(actionTime);
        }
    }
    

    private void Victory()
    {
        if (combatStates[combatStates.Count-1] == CombatState.PlayerDeath)
        {
            PlayerLoose();
        }
        else
        {
            EnemyLoose();
        }
    }
    private void PlayerLoose()
    {

    }
    private void EnemyLoose()
    {

    }


    public void PlayerAttack()
    {
        if (actualCombatState == CombatState.PlayerAttack)
        {
            //Dégat player poke to EnemiePokeHP (slider)
            enemiePokémonHP.value -= DictAttackData[playerPoke.data.attackIDlist[playerPoke.attackId]].dmg;
            chatText.text = playerPokémonName.text + " utilise " + DictAttackData[playerPoke.data.attackIDlist[playerAttackNbr]].name + " !";
            if (enemiePokémonHP.value <= 0)
            {
                //Play anim texte qui s'écrit
                //Play anim poke qui meurt
                chatText.text = "Player win !!! \n Press C to quit";
                actualCombatState = CombatState.PlayerVictory;
            }
            else
            {
                //Play anim texte qui s'écrit
                //Play anim Attack
                actualCombatState = CombatState.EnemyAttack;
                EnemyAttack();
            }
        }
    }
    private void EnemyAttack()
    {
        if (actualCombatState == CombatState.EnemyAttack)
        {
            playerPokémonHP.value -= DictAttackData[enemiePoke.data.attackIDlist[enemyAttackNbr]].dmg;
            playerPoke.data.hp -= DictAttackData[enemiePoke.data.attackIDlist[enemyAttackNbr]].dmg;
            chatText.text = enemiePokémonName.text + " utilise " + DictAttackData[playerPoke.data.attackIDlist[enemyAttackNbr]].name + " !";
            if (playerPokémonHP.value <= 0)
            {
                //Play anim texte qui s'écrit
                //Play anim poke qui meurt
                chatText.text = "Enemy win !!!";
                actualCombatState = CombatState.PlayerVictory; //Changer pour EnemyVictory
            }
            else
            {
                //Play anim texte qui s'écrit
                //Play anim Attack
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

    public void CheckType(PokeData pokeType, AttackData attackType)
    {
        Debug.Log((int) attackType.TYPE); //ordonnée
        Debug.Log((int) pokeType.TYPE); //abscisse
    }
}

public class Pokemon
{
    public PokeData data;
    public bool isPlayer;
    public int attackId = 0;

    public Pokemon(PokeData data, bool isPlayer)
    {
        this.data = data;
        this.isPlayer = isPlayer;
    }   
}