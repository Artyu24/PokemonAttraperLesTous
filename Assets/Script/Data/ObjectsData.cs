using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class ObjectData : BaseData
    {
        public ObjectData(string name, int id = 0, string caption = "", PokeType pokePokeType = PokeType.NORMALE) : base(name, id, caption, pokePokeType)
        {

        }
    }
}
