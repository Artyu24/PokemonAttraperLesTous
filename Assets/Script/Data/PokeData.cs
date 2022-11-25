using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class PokeData : BaseData
    {
        // editor pour modifier 
        [Header("Stats")] 
        public int dmg;
        public float hp;
        public float hpMax;
        public int def;
        public int speed;

        [Header("Visuel")] 
        public Sprite sprite;
        public Sprite BackSprite;
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

        private PokeData(string name, PokeData pokedata, int id = 0, string caption = "", PokeType pokeType = PokeType.NORMALE) : base(name, id, caption, pokeType)
        {
            dmg = pokedata.dmg;
            hp = pokedata.hp;
            hpMax = pokedata.hpMax ;
            def = pokedata.def;
            speed = pokedata.speed;
            sprite = pokedata.sprite;
            BackSprite = pokedata.BackSprite;
            animator = pokedata.animator;
            attackIDlist = pokedata.attackIDlist;
        }

        public PokeData CopyPokeData()
        {
            return new PokeData(name, this, ID, desc, TYPE);
        }
    }
}
