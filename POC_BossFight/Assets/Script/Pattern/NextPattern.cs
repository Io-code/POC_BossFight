using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NextPattern 
{
    public string name;
    public ScriptablePattern pattern;
    public float weight;

    public NextPattern(ScriptablePattern pattern, float weight)
    {
        name = pattern.name;
        this.pattern = pattern;
        this.weight = weight;
    }
}
