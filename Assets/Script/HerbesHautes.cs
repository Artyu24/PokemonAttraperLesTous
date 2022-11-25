using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;

public class HerbesHautes : MonoBehaviour
{
    [SerializeField]
    private int spawnRate;
    [SerializeField]
    private int maxSpawnRate;

    public GameObject combatWindow;
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
            
            GameManager.Instance.ActualPlayerState = PlayerState.InFight;
            GameManager.Instance.ActualGameState = GameState.Fight;
            CombatManager.Instance.ActualCombatState = CombatState.Init;
            CombatManager.Instance.StartCombat(wildPoke);
        }
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Stop("MainTheme");
            FindObjectOfType<AudioManager>().Play("PokemonSauvage");
        }

    }
}
