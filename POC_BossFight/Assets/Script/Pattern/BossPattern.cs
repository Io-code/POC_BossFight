using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPattern 
{

        public ScriptablePattern pattern;
        public List<NextPattern> nextPatterns;

    public BossPattern(ScriptablePattern pattern)
    {
        this.pattern = pattern;
        nextPatterns = new List<NextPattern>();
    }
    
}
