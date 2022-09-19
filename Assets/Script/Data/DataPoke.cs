using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DataPoke : ScriptableObject
{
    [Header("Description")]
    public string name;
    public int ID;

    [Header("Stats")] 
    public string type;
    public int dmg;
    public int hp;
    public int def;
    public int speed;

    [Header("Visuel")] 
    public Sprite sprite;
    public Animator animator;

    [Header("Attack")] 
    public DataAttack[] attacklist = new DataAttack[4];
}
