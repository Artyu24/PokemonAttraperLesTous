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

    public void StartCombat(DataPoke wildPoke)
    {
        playerPoke = playerPokes.poke1;
        enemiePok�monName.text = wildPoke.name;
        enemiePok�monHP.value = wildPoke.hp;
        enemiePok�monHP.maxValue = wildPoke.hp;
        enemiePok�monSprite.sprite = wildPoke.sprite;

        playerPok�monName.text = playerPoke.name;
        playerPok�monHPText.text = playerPoke.hp + "/" + playerPoke.hpMax;
        playerPok�monHP.value = playerPoke.hp;
        playerPok�monHP.maxValue = playerPoke.hp;
        playerPok�monSprite.sprite = playerPoke.sprite;

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
        chatText.text = playerPok�monName.text + " utilise " + playerPoke.attacklist[attackNumber].name + " !";
        enemiePok�monHP.value -= playerPoke.attacklist[attackNumber].dmg;
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
