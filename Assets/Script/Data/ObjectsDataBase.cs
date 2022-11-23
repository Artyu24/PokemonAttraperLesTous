using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Object.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "Database/Object", order = 1)]
public class ObjectsDataBase : ScriptableObject
{
    //custom editor pour pouvoir modifier l'inspector
    public List<ObjectsData> objectsData = new List<ObjectsData>();

    public void Init()
    {
        foreach (ObjectsData oData in objectsData)
        {
            oData.ApplyEffect();
        }
    }
    void Reset()
    {
        objectsData.Add(new ObjectsData("Potion", objectsData.Count, "ça heal quoi" , BaseData.PokeType.OBJECT, 1));
    }
}