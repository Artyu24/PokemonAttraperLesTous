using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Object.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDatabase", menuName = "Database/Attack", order = 1)]
public class AttackDatabase : ScriptableObject
{
    //custom editor pour pouvoir modifier l'inspector
    public List<AttackData> AttackData = new List<AttackData>();

    void Reset()
    {
        AttackData.Add(new AttackData("Charge"));
    }
}
