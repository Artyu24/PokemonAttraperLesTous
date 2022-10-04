using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class AttackData : BaseData
    {

        [Header("Stats")]
        public int dmg;
        public int pp;

        public AttackData(string name, int id = 0, string caption = "", Type type = Type.NORMAL) : base(name, id, caption, type)
        {
            dmg = 10;
            pp = 10;
        }
    }
}
