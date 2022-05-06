using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pattern",menuName ="Scriptable/Pattern")]
public class BossPattern : ScriptableObject
{
    public AnimationClip moveInClip, attackClip;
    public float startDelay = 1; 

    [Header("Trigger Attack")]
    public float attackDist = 1; 
    public float marginDist = 0.5f;
    public List<NextPattern> nextPatterns;

    [System.Serializable]
    public struct NextPattern
    {
        public BossPattern pattern;
        public float weight;
    }
}
