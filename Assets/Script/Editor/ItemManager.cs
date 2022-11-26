using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private ObjectsDataBase objectsDataBase;
    private PokeDatabase pokeDataBase;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objectsDataBase = (ObjectsDataBase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/ObjectDatabase.asset", typeof(ObjectsDataBase));
        pokeDataBase = (PokeDatabase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/PokeDatabase.asset", typeof(PokeDatabase));

        //Debug.Log(objectsDataBase.objectsData[0]);

        objectsDataBase.Init();
        objectsDataBase.objectsData[0].OnEffect.Invoke(pokeDataBase.PokeData[0]);
    }

}
