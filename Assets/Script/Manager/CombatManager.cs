using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    public PokeDatabase pokeDataBase; //scriptable de tout les pok�mons du jeu
    public ObjectsDataBase objectsDataBase;
    private Dictionary<int, PokeData> dictPokeData = new Dictionary<int, PokeData>(); //dictionnaire de tout les pok�mons du jeu
    private Dictionary<int, AttackData> dictAttackData = new Dictionary<int, AttackData>(); //dictionnaire de toutes les attaques du jeu
    public Dictionary<int, PokeData> DictPokeData => dictPokeData;
    public Dictionary<int, AttackData> DictAttackData => dictAttackData;

    public GameObject combatWindow; //fen�tre de l'interface de combat
    public GameObject blackBackground;
    public Text chatText; //zone de text qui affiche les infos, ex : trucmuche utilise charge !

    #region Pok�monsUI
    [Header("Pok�monsUI")]
    public Text playerPok�monName;
    public Slider playerPok�monHP;
    public Text playerPok�monHPText;
    public Image playerPok�monSprite;
    public Text enemiePok�monName;
    public Slider enemiePok�monHP;
    public Image enemiePok�monSprite;
    public GameObject pokemonWindow;
    public Text pokemonButton1;
    public Text pokemonButton2;
    public Text pokemonButton3;
    public Text pokemonButton4;
    public Text pokemonButton5;
    public Text pokemonButton6;

    //public Dialogue dialogue;
    #endregion

    #region Anims
    public bool isInHerbeHautes = false;
    public Animator combatAnimator;
    //public Animator playerAnimator;


    #endregion

    #region AttacksInfo&UI
    [Header("Attacks")]
    public GameObject attackWindow;
    public GameObject objectWindow;
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
    private List<CombatState> combatStates = new List<CombatState>();
    public float actionTime = 5.0f;
    public Button[] attackButtons = new Button[4];
    private Text[] attackButtonsText = new Text[4];
    private bool isUsingPotion = false;
    private int objectUseID;

    public Image dresseurImage;
    public List<Sprite> dresseurSprite = new List<Sprite>();
    private int dresseurID;
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
            Debug.Log("Il manque la database de pok�mon");
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
        combatDictionaire.Add(CombatState.CallButton, CallButton);
        combatDictionaire.Add(CombatState.UsePotion, HealPlayerPoke);
        combatDictionaire.Add(CombatState.PlayerAttack, PlayerAttack);
        combatDictionaire.Add(CombatState.EnemyAttack, EnemyAttack);
        combatDictionaire.Add(CombatState.PlayerDeath, PlayerLoose);
        combatDictionaire.Add(CombatState.EnemyDeath, EnemyLoose);
        combatDictionaire.Add(CombatState.Victory, Victory);
        combatDictionaire.Add(CombatState.End, QuitCombat);

        for (int i = 0; i < attackButtons.Length; i++)
        {
            if (attackButtons[i] != null)
                attackButtonsText[i] = attackButtons[i].GetComponentInChildren<Text>();
        }

        if (playerPokes.pokes.Count == 0)
        {
            Debug.Log("Le joueur n'a pas de pok�mon");
            return;
        }
        
        playerPoke = new Pokemon(playerPokes.pokes[0].CopyPokeData(), true); //copie cheum du playerpoke
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Return();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            QuitCombat();
        }
    }

    public void StartCombat(PokeData wild, bool isInHH, int qui)
    {
        dresseurID = qui; 
        switch (dresseurID)
        {
            case 0:
                dresseurImage.sprite = dresseurSprite[0];
                break;
            case 1:
                dresseurImage.sprite = dresseurSprite[1];
                break;
            case 2:
                dresseurImage.sprite = dresseurSprite[2];
                break;
            case 3:
                dresseurImage.sprite = dresseurSprite[3];
                break;
            case 4:
                dresseurImage.sprite = dresseurSprite[4];
                break;
        }
        isInHerbeHautes = isInHH;
        enemiePoke = new Pokemon (wild, false);
        chatText.text = wild.name + " est apparu !!!";

        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().PlayFade("PokemonSauvage");
        }
        foreach (var poke in dictPokeData)
        {
            int i = 0;
            dictPokeData[i].hp = dictPokeData[i].hpMax;
            playerPok�monHP.value = playerPoke.data.hp;
            enemiePok�monHP.value = enemiePoke.data.hp;
        }

        //Play anim enemie/player spawn
        #region SetupUICombat

        enemiePok�monName.text = enemiePoke.data.name;
        enemiePok�monHP.maxValue = enemiePoke.data.hpMax;
        enemiePok�monHP.value = enemiePoke.data.hp;
        playerPok�monName.text = playerPoke.data.name;
        playerPok�monHPText.text = playerPoke.data.hp + "/" + playerPoke.data.hpMax;
        playerPok�monHP.maxValue = playerPoke.data.hpMax;
        playerPok�monHP.value = playerPoke.data.hpMax;
        
        playerPok�monSprite.sprite = playerPoke.data.BackSprite;
        enemiePok�monSprite.sprite = enemiePoke.data.sprite;

        for (int i = 0; i < attackButtonsText.Length; i++)
        {
            attackButtonsText[i].text = DictAttackData[playerPoke.data.attackIDlist[i]].name;
        }

        #endregion

        if (combatAnimator != null)
        {
            if (isInHerbeHautes)
            {
                
                combatAnimator.SetTrigger("PasDresseur");
                combatAnimator.SetTrigger("IsInHautesHerbes");
            }
            else
            {
                combatAnimator.SetTrigger("Dresseur");
                switch (dresseurID)
                {
                    case 0:
                        combatAnimator.SetInteger("Dresseurs", 0);
                        //T�o
                        break;
                    case 1:
                        combatAnimator.SetInteger("Dresseurs", 1);
                        //Antoine
                        break; 
                    case 2:
                        combatAnimator.SetInteger("Dresseurs", 2);
                        //Arthur
                        break; 
                    case 3:
                        combatAnimator.SetInteger("Dresseurs", 3);
                        //JM
                        break;
                    case 4:
                        combatAnimator.SetTrigger("EnemyThrowPoke");
                        //FranckO
                        break;
                }
            }
            combatAnimator.SetTrigger("EnemyPokeAppear");
            combatAnimator.SetTrigger("PlayerThrowPoke");
            combatAnimator.SetTrigger("PlayerPokeAppear");
        }
    }

    public void CallButton()
    {
        attackWindow.SetActive(false);
        objectWindow.SetActive(false);
    }

    public void PlayerChoose()
    {
        int attackID = System.Array.IndexOf(attackButtons, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());

        playerPoke.attackId = attackID;
        attackWindow.SetActive(false);

        EnemyChoose();
    }

    public void UsePotion(int objectID)
    {
        objectUseID = objectID;
        isUsingPotion = true;

        objectWindow.SetActive(false);
        EnemyChoose();
    }
    public void HealPlayerPoke()
    {
        if (combatWindow.activeSelf)
        {
            combatAnimator.SetTrigger("UsePotion");// JM
            if (FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().Play("Heal");
            }
        }
        if(playerPoke.data.hp + objectsDataBase.objectsData[objectUseID].value > playerPoke.data.hpMax)
        {
            playerPoke.data.hp = playerPoke.data.hpMax;
            playerPok�monHP.value = playerPoke.data.hpMax;
            playerPok�monHPText.text = playerPoke.data.hpMax.ToString() + "/" + playerPoke.data.hpMax.ToString();
        }
        else
        {
            playerPoke.data.hp += objectsDataBase.objectsData[objectUseID].value;
            playerPok�monHP.value += objectsDataBase.objectsData[objectUseID].value;
            playerPok�monHPText.text = playerPoke.data.hp.ToString() + "/" + playerPoke.data.hpMax.ToString();
        }
        isUsingPotion = false;
    }

    private void EnemyChoose()
    {
        enemiePoke.attackId = UnityEngine.Random.Range(0, 4);

        PresShotRound();
    }

    private void PresShotRound()
    {
        Pokemon fastestPoke;
        Pokemon slowestPoke;
        combatStates.Clear();

        if (isUsingPotion)
        {
            combatStates.Add(CombatState.UsePotion);
        }

        if (playerPoke.data.speed > enemiePoke.data.speed)
        {
            fastestPoke = playerPoke;
            slowestPoke = enemiePoke;
            if (!isUsingPotion)
            {
                combatStates.Add(CombatState.PlayerAttack);
            }
        }
        else
        {
            fastestPoke = enemiePoke;
            slowestPoke = playerPoke;
            combatStates.Add(CombatState.EnemyAttack);
        }

        bool isDead = false;
        if(!isUsingPotion && fastestPoke != playerPoke)
            if (IsDead(slowestPoke.data.hp, DictAttackData[fastestPoke.data.attackIDlist[playerAttackNbr]].dmg))
                isDead = true;

        if(fastestPoke.isPlayer)
        {
            if (isDead)
            {
                combatStates.Add(CombatState.Victory);
                combatStates.Add(CombatState.EnemyDeath);
                combatStates.Add(CombatState.End);
            }
            else
                combatStates.Add(CombatState.EnemyAttack);
        }
        else
        {
            if (isDead)
            {
                combatStates.Add(CombatState.Victory);
                combatStates.Add(CombatState.PlayerDeath);
                combatStates.Add(CombatState.End);
            }
            else
            {
                if (!isUsingPotion)
                {
                    combatStates.Add(CombatState.PlayerAttack);
                }
            }
            
        }

        if (combatStates[combatStates.Count - 1] != CombatState.EnemyDeath && combatStates[combatStates.Count - 1] != CombatState.PlayerDeath)
        {
            isDead = false;
            if (!isUsingPotion && slowestPoke != enemiePoke)
                if (IsDead(fastestPoke.data.hp, DictAttackData[slowestPoke.data.attackIDlist[playerAttackNbr]].dmg))
                    isDead = true;

            if (slowestPoke.isPlayer)
            {
                if (isDead)
                {
                    combatStates.Add(CombatState.Victory);
                    combatStates.Add(CombatState.EnemyDeath);
                    combatStates.Add(CombatState.End);
                }
                else
                    combatStates.Add(CombatState.CallButton);
            }
            else
            {
                if (isDead)
                {
                    combatStates.Add(CombatState.Victory);
                    combatStates.Add(CombatState.PlayerDeath);
                    combatStates.Add(CombatState.End);
                }
                else
                    combatStates.Add(CombatState.CallButton);
            }
        }
        StartCoroutine(PlayRound());
    }

    private bool IsDead(float pokeLife, float attackDmg)
    {
        if (pokeLife <= attackDmg)
            return true;
        return false;
    }

    private IEnumerator PlayRound()
    {
        foreach (CombatState combatState in combatStates.ToList())
        {
            actualCombatState = combatState;
            combatDictionaire[combatState](); //Appel des fonctions dans l'ordre defini par PreShotRound
            yield return new WaitForSeconds(actionTime);
        }
    }

    private void Victory()
    {
        /*if (combatStates[combatStates.Count-1] == CombatState.PlayerDeath)
        {
        }
        if (combatStates[combatStates.Count - 1] == CombatState.EnemyDeath)
        {
            Debug.Log("Kaput");
        }*/
    }
    private void PlayerLoose()
    {
        combatAnimator.SetTrigger("PlayerPokeDeath");// JM
        chatText.text = playerPok�monName.text + " n'as plus de force, tu dois retourner au centre pok�mon le plus proche";
    }
    private void EnemyLoose()
    {
        if (isInHerbeHautes)
        {
            combatAnimator.SetTrigger("EnemiePokeDeath");// JM
        }
        else
        {
            switch (dresseurID)
            {
                case 0:
                    combatAnimator.SetTrigger("TeoDeath");
                    //T�o
                    break;
                case 1:
                    combatAnimator.SetTrigger("AntoineDeath");
                    //Antoine
                    break;
                case 2:
                    combatAnimator.SetTrigger("ArthurDeath");
                    //Arthur
                    break;
                case 3:
                    combatAnimator.SetTrigger("JMDeath");
                    //JM
                    break;
                case 4:
                    combatAnimator.SetTrigger("Dresseur&Death");
                    // Francko
                    break;
            }
            //combatAnimator.SetInteger("DresseurDeath", 5);
        }
        chatText.text = enemiePok�monName.text + " a �t� vaincu. F�licitation :";
    }

    private void Damage(Pokemon attaquant, Pokemon defenseur)
    {
        ReadPokeTypes.instance.ObtainSheetData(defenseur.data.TYPE.GetHashCode(), attaquant.data.TYPE.GetHashCode());
        float multipliyer = ReadPokeTypes.instance.multiplyer;
        defenseur.data.hp -= attaquant.data.dmg * multipliyer;
    }

    public void PlayerAttack()
    {
        Damage(enemiePoke, playerPoke);
        if (enemiePoke.data.hp <= 0)
        {
            enemiePok�monHP.value = 0;
            enemiePoke.data.hp = 0;
        }
        else
        {
            enemiePoke.data.hp -= DictAttackData[playerPoke.data.attackIDlist[playerPoke.attackId]].dmg;
            enemiePok�monHP.value -= DictAttackData[playerPoke.data.attackIDlist[playerPoke.attackId]].dmg;
        }
        combatAnimator.SetTrigger("PlayerAttackRange");// JM
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Play("Attack");
        }
        chatText.text = playerPok�monName.text + " utilise " + DictAttackData[playerPoke.data.attackIDlist[playerPoke.attackId]].name + " !";
    }
    private void EnemyAttack()
    {
        Damage(playerPoke, enemiePoke);
        if (playerPoke.data.hp <= 0)
        {
            playerPoke.data.hp = 0;
            playerPok�monHP.value = 0;
            playerPok�monHPText.text = 0 + "/" + playerPoke.data.hpMax;
        }
        else
        {
            playerPoke.data.hp -= DictAttackData[enemiePoke.data.attackIDlist[enemiePoke.attackId]].dmg;
            playerPok�monHP.value -= DictAttackData[enemiePoke.data.attackIDlist[enemiePoke.attackId]].dmg;
            playerPok�monHPText.text = playerPoke.data.hp + "/" + playerPoke.data.hpMax;
        }
        combatAnimator.SetTrigger("EnemieAttackCac");// JM
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Play("Attack");
        }
        chatText.text = enemiePok�monName.text + " utilise " + DictAttackData[enemiePoke.data.attackIDlist[enemiePoke.attackId]].name + " !";
    }

    public void QuitCombat()
    {
        StartCoroutine(QuitAnim());
    }
    public void FlyFight()
    {
        StartCoroutine(QuitAnim());
    }

    private IEnumerator QuitAnim()
    {
        GameManager.Instance.ActivateFade(true);
        yield return new WaitForSeconds(2.5f);
        combatAnimator.SetTrigger("FinFight");// JM
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().PlayFade("RouteMusic");
        }
        blackBackground.SetActive(false);
        combatWindow.SetActive(false);
        GameManager.Instance.ActualPlayerState = PlayerState.Idle;
        GameManager.Instance.ActualGameState = GameState.Adventure;
        GameManager.Instance.ActivateFade(false);
    }

    public void Return()
    {
        attackWindow.SetActive(false);
        pokemonWindow.SetActive(false);
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