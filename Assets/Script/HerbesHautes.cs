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
    public PokeData[] wildPokes = new PokeData[4];
    public PokeData wildPoke;

    public PokeData WildPoke { get => wildPoke; }


    void Awake()
    {
        herbeCollier = GetComponent<BoxCollider2D>();
    }
    
    void Start()
    {
        spawnRate = Random.Range(0, 30);

        for (int i = 0; i < wildPokes.Length; i++)
        {
            wildPokes[i] = CombatManager.Instance.DictPokeData[Random.Range(0, 6)];
        }
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
            combatWindow.SetActive(true);
            GameManager.Instance.ActualPlayerState = PlayerState.PlayerInFight;
            GameManager.Instance.ActualGameState = GameState.Fight;
            CombatManager.Instance.ActualCombatState = CombatState.Init;
            CombatManager.Instance.StartCombat(wildPoke);
        }
        else
        {
            spawnRate = Random.Range(0, 100);
        }
    }
}
