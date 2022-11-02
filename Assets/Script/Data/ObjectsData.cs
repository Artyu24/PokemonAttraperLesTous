using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class ObjectData : BaseData
    {
        public ObjectData(string name, int id = 0, string caption = "", Type type = Type.NORMALE) : base(name, id, caption, type)
        {

        }
    }
}
