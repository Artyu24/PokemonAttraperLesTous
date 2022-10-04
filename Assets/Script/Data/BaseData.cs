using UnityEngine;

namespace Object.Data
{
    [System.Serializable]
    public class BaseData
    {
        [Header("Base")] 
        public string name;
        public string ID;
        public string desc;

        public BaseData(string name, string id, string caption)
        {
            this.name = name;
            ID = id;
            desc = caption;
        }
    }
}
