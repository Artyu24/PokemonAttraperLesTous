using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public TeamPokes playerPokes;

    public Text playerPok�monName;
    public Slider playerPok�monHP;
    public Text playerPok�monHPText;
    public Image playerPok�monSprite;
    //public Text playerPok�monLvl;
    public Text enemiePok�monName;
    public Slider enemiePok�monHP;
    public Image enemiePok�monSprite;
    //public Text enemiePok�monLvl;

    public Text chatText;
    public GameObject attackWindow;
    public Text attackButton1;
    public Text attackButton2;
    public Text attackButton3;
    public Text attackButton4;
    
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

    public void StartCombat(DataPoke wildPoke)
    {
        enemiePok�monName.text = wildPoke.name;
        enemiePok�monHP.value = wildPoke.hp;
        enemiePok�monHP.maxValue = wildPoke.hp;
        enemiePok�monSprite.sprite = wildPoke.sprite;

        DataPoke playerPoke = playerPokes.poke1;
        playerPok�monName.text = playerPoke.name;
        playerPok�monHPText.text = playerPoke.hp.ToString() + "/" + playerPoke.hpMax;
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

        chatText.text = wildPoke.name.ToString() + " est apparu !!!";
    }

    public void FlyFight()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.PlayerInMovement;
        GameManager.Instance.ActualGameState = GameState.Adventure;
    }

    public void Attack(DataAttack attack)
    {
        chatText.text = playerPok�monName.text + " utilise " + attack.name + " !";
        enemiePok�monHP.value -= attack.dmg;
        attackWindow.SetActive(false);

    }
}
