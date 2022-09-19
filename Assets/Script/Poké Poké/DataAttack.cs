using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAttack : ScriptableObject
{
    [Header("Name & Desc")] 
    public string name;
    public string desc;

    [Header("Stats")]
    public int dmg;
    public int pp;
    public string type;
    public int ID;

}
