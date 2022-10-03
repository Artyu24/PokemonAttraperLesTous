using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public TeamPokes playerPokes;
    private DataPoke playerPoke;
    public GameObject combatWindow;

    [Header("PokémonsUI")]
    public Text playerPokémonName;
    public Slider playerPokémonHP;
    public Text playerPokémonHPText;
    public Image playerPokémonSprite;
    //public Text playerPokémonLvl;
    public Text enemiePokémonName;
    public Slider enemiePokémonHP;
    public Image enemiePokémonSprite;
    //public Text enemiePokémonLvl;

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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Return();
        }
    }

    public void StartCombat(DataPoke wildPoke)
    {
        playerPoke = playerPokes.poke1;
        enemiePokémonName.text = wildPoke.name;
        enemiePokémonHP.value = wildPoke.hp;
        enemiePokémonHP.maxValue = wildPoke.hp;
        enemiePokémonSprite.sprite = wildPoke.sprite;

        playerPokémonName.text = playerPoke.name;
        playerPokémonHPText.text = playerPoke.hp + "/" + playerPoke.hpMax;
        playerPokémonHP.value = playerPoke.hp;
        playerPokémonHP.maxValue = playerPoke.hp;
        playerPokémonSprite.sprite = playerPoke.sprite;

        attackButton1.text = playerPoke.attacklist[0].name;
        attackButton2.text = playerPoke.attacklist[1].name;
        attackButton3.text = playerPoke.attacklist[2].name;
        attackButton4.text = playerPoke.attacklist[3].name;

        /*for (int i = 0; i < pokes.length; i++)
        {
            pokemonButton1.text = playerPokes[i].name;
        }*/

        chatText.text = wildPoke.name + " est apparu !!!";
    }

    public void FlyFight()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
        GameManager.Instance.ActualGameState = GameState.Adventure;
        combatWindow.SetActive(false);
    }

    public void Attack(int attackNumber)
    {
        chatText.text = playerPokémonName.text + " utilise " + playerPoke.attacklist[attackNumber].name + " !";
        enemiePokémonHP.value -= playerPoke.attacklist[attackNumber].dmg;
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
