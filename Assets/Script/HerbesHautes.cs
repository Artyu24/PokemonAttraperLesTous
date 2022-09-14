using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbesHautes : MonoBehaviour
{
    public int spawnRate;
    private Sprite movementSprite;
    private BoxCollider2D herbeCollier;
    
    void Awake()
    {
        herbeCollier = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        spawnRate = Random.Range(0, 100);
    }

    public void OnTriggerEnter2D(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Random.Range(1, 101) <= spawnRate)
            {
                SpawnPokemon();
            }
        }
    }

    private void SpawnPokemon()
    {
        Debug.Log("Ratata dans ta gueule !!!");
    }
}
