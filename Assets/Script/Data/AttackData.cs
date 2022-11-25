using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class AttackData : BaseData
    {

        [Header("Stats")]
        public int dmg;
        public int pp;
        public bool isRange;

        public AttackData(string name, int id = 0, string caption = "", PokeType pokeType = PokeType.NORMALE) : base(name, id, caption, pokeType)
        {
            dmg = 10;
            pp = 10;
        }
    }
}
