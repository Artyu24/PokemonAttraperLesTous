using System;
using UnityEngine;

namespace Object.Data
{
    [Serializable]
    public class ObjectsData : BaseData
    {
        public int value;
        public enum Categorie
        {
            RESTORE,
            BOOST,
            CAPTURE
        }

        public Categorie categorie;

        public enum SubCategorie
        {
            PP,
            PV,
            ATK,
            DEF,
            SPE,
            BALL,
        }

        public SubCategorie subCategorie;
        
        public ObjectsData(string name, int id, string caption, PokeType pokeType, int value) : base(name, id, caption, pokeType)
        {
            value = this.value;
            Debug.Log("azertyu");
        }

        public delegate void EffectDelegate<T>(T cible);

        public EffectDelegate<PokeData> OnEffect;

        public void ApplyEffect()
        {
            ObjectAction();
        }

        private void ObjectAction()
        {
           switch (categorie)
           {
               case Categorie.RESTORE:
                   OnEffect = RestoreEffect;
                   break;
               case Categorie.BOOST:
                   OnEffect = BoostEffect;
                   break;
               //case Categorie.CAPTURE:
               //    OnEffect = CaptureEffect;
               //    break;
           }
        }

        void RestoreEffect(PokeData pokeCible)
        {
            switch (subCategorie)
            {
                case SubCategorie.PV : 
                     _ = (pokeCible.hp + value) < pokeCible.hpMax ? pokeCible.hp += value : pokeCible.hp = pokeCible.hpMax; break;
            }             
        }
        void RestoreEffect(PokeData pokeCible, int ID)
        {
            switch (subCategorie)
            {
                case SubCategorie.PP: 
                    Debug.Log($"l'attaque {pokeCible.attackIDlist[ID]} regagne {value} PP"); 
                    break;

            }
        }


       void BoostEffect(PokeData pokeCible)
       {
           switch (subCategorie)
           {
               case SubCategorie.ATK: Debug.Log("BOOST ATTACK DE : " + value); break;
               case SubCategorie.DEF: Debug.Log("BOOST DEF DE : " + value); break;
               case SubCategorie.SPE: Debug.Log("BOOST SPE DE : " + value); break;
           }

       }

       void CaptureEffect(PokeData pokeCible)
       {
           switch (subCategorie)
           {
               case SubCategorie.BALL: Debug.Log($"TAUX DE CAPTURE DE : {value}" ); break;
           }

       }
    }

}