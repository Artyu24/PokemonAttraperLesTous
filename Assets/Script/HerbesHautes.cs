using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbesHautes : MonoBehaviour
{
    public int spawnRate;
    private Sprite movementSprite;
    private BoxCollider2D herbeCollier;

    public GameObject combatWindow;
    
    void Awake()
    {
        herbeCollier = GetComponent<BoxCollider2D>();
    }
    
    void Start()
    {
        spawnRate = Random.Range(0, 100);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Random.Range(1, 101) <= spawnRate)
            {
                SpawnPokemon();
                spawnRate = Random.Range(0, 100);
            }
            else
            {
                Debug.Log("Pas de pokémon");
            }
        }
    }

    private void SpawnPokemon()
    {
        Debug.Log("Ratata dans ta gueule !!!");
        combatWindow.SetActive(true);
    }
}
