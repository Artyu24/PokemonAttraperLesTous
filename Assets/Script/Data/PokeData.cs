using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class PokeData : BaseData
    {
        // editor pour modifier 
        [Header("Stats")] 
        public string type;
        public int dmg;
        public int hp;
        public int hpMax;
        public int def;
        public int speed;

        [Header("Visuel")] 
        public Sprite sprite;
        public Animator animator;

        [Header("Attack")] 
        //property drawer/custom atribut pour drop down
        public AttackData[] attacklist = new AttackData[4];

        public PokeData(string name, string id = "", string caption = "") : base(name, id, caption)
        {
        }
    }
}
