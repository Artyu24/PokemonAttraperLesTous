using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEngine;

public class TeamPokes : MonoBehaviour
{
    //public AttackDatabase attackDatabase; //scriptable de toutes les attaques du jeu
    public PokeDataBase pokeDataBase; //scriptable de tout les pokémons du jeu
    public List<PokeData> pokes = null;

    private void Awake()
    {
        if (pokeDataBase != null)
        {
            AddPokeToTeam(pokeDataBase.PokeData);
        }
        else
        {
            Debug.Log("Il manque la database Pokémon");
        }
    }

    public void AddPokeToTeam(List<PokeData> newPokes)
    {
        for (int i = 0; i < newPokes.Count; i++)
        {
            pokes.Add(newPokes[i]);
        }
    }
}
