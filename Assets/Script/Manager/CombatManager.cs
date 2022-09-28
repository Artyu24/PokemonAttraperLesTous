using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public Text playerPokémonName;
    public Slider playerPokémonHP;
    public Text playerPokémonHPText;
    //public Text playerPokémonLvl;
    public Text enemiePokémonName;
    public Slider enemiePokémonHP;
    //public Text enemiePokémonLvl;
    
    
    void Update()
    {
        
    }

    public void StartCombat(DataPoke wildPoke, DataPoke playerPoke)
    {
        enemiePokémonName.text = wildPoke.name;
        enemiePokémonHP.value = wildPoke.hp;

        playerPokémonName.text = playerPoke.name;
        playerPokémonHPText.text = playerPoke.hp.ToString() + "/" + playerPoke.hpMax;
        playerPokémonHP.value = playerPoke.hp;
    }
}
