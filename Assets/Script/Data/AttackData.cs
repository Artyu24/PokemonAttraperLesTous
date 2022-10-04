using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class AttackData : BaseData
    {
        public enum AttackType
        {
            NORMAL,
            EAU,
        }

        [Header("Stats")]
        public int dmg;
        public int pp;
        public AttackType type;

        public AttackData(string name, string id = "", string caption ="") : base(name, id, caption)
        {
            dmg = 10;
            pp = 10;
            type = AttackType.NORMAL;
        }
    }
}
