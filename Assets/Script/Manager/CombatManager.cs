using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public Text playerPok�monName;
    public Slider playerPok�monHP;
    public Text playerPok�monHPText;
    //public Text playerPok�monLvl;
    public Text enemiePok�monName;
    public Slider enemiePok�monHP;
    //public Text enemiePok�monLvl;
    
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartCombat(DataPoke wildPoke/*, DataPoke playerPoke*/)
    {
        enemiePok�monName.text = wildPoke.name;
        enemiePok�monHP.value = wildPoke.hp;

        /*playerPok�monName.text = playerPoke.name;
        playerPok�monHPText.text = playerPoke.hp.ToString() + "/" + playerPoke.hpMax;
        playerPok�monHP.value = playerPoke.hp;*/
    }
}
