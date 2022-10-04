using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public TeamPokes playerPokes;
    private PokeData _player;
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

    public void StartCombat(PokeData wild)
    {
        _player = playerPokes.poke1;
        enemiePokémonName.text = wild.name;
        enemiePokémonHP.value = wild.hp;
        enemiePokémonHP.maxValue = wild.hp;
        enemiePokémonSprite.sprite = wild.sprite;

        playerPokémonName.text = _player.name;
        playerPokémonHPText.text = _player.hp + "/" + _player.hpMax;
        playerPokémonHP.value = _player.hp;
        playerPokémonHP.maxValue = _player.hp;
        playerPokémonSprite.sprite = _player.sprite;

        attackButton1.text = _player.attacklist[0].name;
        attackButton2.text = _player.attacklist[1].name;
        attackButton3.text = _player.attacklist[2].name;
        attackButton4.text = _player.attacklist[3].name;

        /*for (int i = 0; i < pokes.length; i++)
        {
            pokemonButton1.text = playerPokes[i].name;
        }*/

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
        chatText.text = playerPokémonName.text + " utilise " + _player.attacklist[attackNumber].name + " !";
        enemiePokémonHP.value -= _player.attacklist[attackNumber].dmg;
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
