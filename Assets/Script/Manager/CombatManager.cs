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

    [Header("Pok�monsUI")]
    public Text playerPok�monName;
    public Slider playerPok�monHP;
    public Text playerPok�monHPText;
    public Image playerPok�monSprite;
    //public Text playerPok�monLvl;
    public Text enemiePok�monName;
    public Slider enemiePok�monHP;
    public Image enemiePok�monSprite;
    //public Text enemiePok�monLvl;

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
        enemiePok�monName.text = wild.name;
        enemiePok�monHP.value = wild.hp;
        enemiePok�monHP.maxValue = wild.hp;
        enemiePok�monSprite.sprite = wild.sprite;

        playerPok�monName.text = _player.name;
        playerPok�monHPText.text = _player.hp + "/" + _player.hpMax;
        playerPok�monHP.value = _player.hp;
        playerPok�monHP.maxValue = _player.hp;
        playerPok�monSprite.sprite = _player.sprite;

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
        chatText.text = playerPok�monName.text + " utilise " + _player.attacklist[attackNumber].name + " !";
        enemiePok�monHP.value -= _player.attacklist[attackNumber].dmg;
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
