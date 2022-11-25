using Object.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresseur : MonoBehaviour
{
    [SerializeField]
    private int spawnRate;
    [SerializeField]
    private int maxSpawnRate;

    [Tooltip("0 : Téo/ 1 : Antoine/ 2 : Arthur/ 3 : JM/ 4 : FranckO")]
    public int dresseur;

    public GameObject combatWindow;
    public GameObject blackBackground;
    public PokeData[] wildPokes = new PokeData[4];
    public PokeData wildPoke;

    private PokeData WildPoke { get => wildPoke; }

    void Start()
    {
        if (CombatManager.Instance.DictPokeData.Count == 0)
        {
            Debug.Log("Le dictionnaire de Pokémon n'est pas bon");
            return;
        }

        for (int i = 0; i < wildPokes.Length; i++)
        {
            wildPokes[i] = CombatManager.Instance.DictPokeData[Random.Range(0, 6)];
        }
    }

    public void SpawnPokemon()
    {
        int random = Random.Range(0, maxSpawnRate);
        if (random <= spawnRate)
        {
            int temp = Random.Range(0, wildPokes.Length);
            wildPoke = wildPokes[temp].CopyPokeData();

            combatWindow.SetActive(true);
            blackBackground.SetActive(true);


            GameManager.Instance.ActualPlayerState = PlayerState.InFight;
            GameManager.Instance.ActualGameState = GameState.Fight;
            CombatManager.Instance.ActualCombatState = CombatState.Init;
            CombatManager.Instance.StartCombat(wildPoke, false, dresseur) ;
        }
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Stop("MainTheme");
            FindObjectOfType<AudioManager>().Play("PokemonSauvage");//JM changer pour Dresseur
        }
    }
}
