using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbesHautes : MonoBehaviour
{
    public int spawnRate;
    private Sprite movementSprite;
    private BoxCollider2D herbeCollier;

    public GameObject combatWindow;
    public DataPoke[] wildPokes;
    public DataPoke wildPoke;

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

    /*public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")*//*other.gameObject.layer.ToString() == "Grass"*//* && GameManager.Instance.ActualPlayerState == PlayerState.PlayerInMovement)
        {
            SpawnPokemon();
            //Faire sur le player
        }
    }*/

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
