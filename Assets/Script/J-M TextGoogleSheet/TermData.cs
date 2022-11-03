using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TermData 
{
    public class Terms
    {
        public Dictionary<string, int> languageIndicies;

        public Dictionary<string, string[]> termTranslation;

        public Terms() 
        {
            languageIndicies = new Dictionary<string, int>();
            termTranslation = new Dictionary<string, string[]>();
        }
    }
}
