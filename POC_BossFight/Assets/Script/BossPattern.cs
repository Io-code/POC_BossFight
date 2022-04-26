using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : ScriptableObject
{

    public float playerDist = 3, distAcceptance = 0.5f;
    public float patternWeight = 1;

    public AnimationClip animPattern;

    public enum PatternType { ATTACK, DISPLACEMENT };
    public PatternType patternType;

    public int damage;
    public float strength, stunDuration;
}
