using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Object.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "PokeDatabase", menuName = "Database/Pokemon", order = 1)]
public class PokeDatabase : ScriptableObject
{
    //custom editor pour pouvoir modifier l'inspector
    public List<PokeData> PokeData = new List<PokeData>();

    void Reset()
    {
        PokeData.Add(new PokeData("Roucool"));
    }
}
