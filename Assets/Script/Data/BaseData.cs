using System;
using UnityEngine;

namespace Object.Data
{
    [Serializable]
    public class BaseData
    {
        [Header("Base")] 
        public string name;
        public int ID;
        public string desc;
        public Type TYPE;

        public enum Type
        {
            ACIER,
            COMBAT,
            DRAGON,
            EAU,
            ELECTRIK,
            FEE,
            FEU,
            GLACE,
            INSECTE,
            NORMALE,
            PLANTE,
            POISON,
            PSY,
            ROCHE,
            SOL,
            SPECTRE,
            TENEBRES,
            VOL
        }

        public BaseData(string name, int id, string caption, Type type)
        {
            this.name = name;
            ID = id;
            desc = caption;
            TYPE = type;
        }
    }
}
