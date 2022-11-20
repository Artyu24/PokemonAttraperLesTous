using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class PokeData : BaseData
    {
        // editor pour modifier 
        [Header("Stats")] 
        public int dmg;
        public int hp;
        public int hpMax;
        public int def;
        public int speed;

        [Header("Visuel")] 
        public Sprite sprite;
        public Animator animator;

        [Header("Attack")]
        public int[] attackIDlist = new int[4];

        public PokeData(string name, int id = 0, string caption = "", PokeType pokeType = PokeType.NORMALE) : base(name, id, caption, pokeType)
        {
            dmg = 10;
            hp = 50;
            hpMax = 50;
            def = 5;
            speed = 1;
        }
    }
}
