using System.Collections;
using System.Collections.Generic;
using Object.Data;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private ObjectsDataBase objectsDataBase;
    [SerializeField] private ObjectsData.EffectDelegate test;


    // Start is called before the first frame update
    void Start()
    {
        objectsDataBase = (ObjectsDataBase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/ObjectDatabase.asset", typeof(ObjectsDataBase));

        //Debug.Log(objectsDataBase.objectsData[0]);

        objectsDataBase.Init();
        objectsDataBase.objectsData[0].OnEffect.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
