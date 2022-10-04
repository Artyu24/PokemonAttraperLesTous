using System;
using UnityEngine;

namespace Object.Data
{
    [Serializable]
    public class BaseData
    {
        [Header("Base")] 
        public string name;
        public string ID;
        public string desc;
        public Type TYPE;

        public enum Type
        {
            NORMAL,
            EAU,
            PLANTE,
            FEU,
            ACIER,
            PSY,
            COMBAT,
            ELECTRIQUE,
            GLACE,
            ROCHE,
            VOL,
            FÉE,
            DRAGON,
            TÉNEBRE,
            POISION,
            SOL,
            INSECT,
            SPECTRE,
        }

        public BaseData(string name, string id, string caption, Type type)
        {
            this.name = name;
            ID = id;
            desc = caption;
            TYPE = type;
        }
    }
}
