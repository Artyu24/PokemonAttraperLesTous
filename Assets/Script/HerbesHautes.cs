using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;

public class HerbesHautes : MonoBehaviour
{
    private int spawnRate;
    private Sprite movementSprite;
    private BoxCollider2D herbeCollier;

    public GameObject combatWindow;
    public PokeData[] wildPokes;
    public PokeData wildPoke;

    public PokeData WildPoke { get => wildPoke; }


    void Awake()
    {
        herbeCollier = GetComponent<BoxCollider2D>();
    }
    
    void Start()
    {
        spawnRate = Random.Range(0, 30);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnPokemon();
        }
    }

    private void SpawnPokemon()
    {
        if (Random.Range(1, 101) <= spawnRate)
        {
            spawnRate = Random.Range(0, 100);
            int temp = Random.Range(0, wildPokes.Length);
            wildPoke = wildPokes[temp];
            Debug.Log("Ratata dans ta gueule !!!");
            combatWindow.SetActive(true);
            GameManager.Instance.ActualPlayerState = PlayerState.PlayerInFight;
            GameManager.Instance.ActualGameState = GameState.Fight;
            CombatManager.Instance.StartCombat(wildPoke);
        }
        else
        {
            Debug.Log("Pas de pokémon");
            spawnRate = Random.Range(0, 100);
        }
    }
}
