using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pattern",menuName ="Scriptable/Pattern")]
public class ScriptablePattern : ScriptableObject
{
    public Sprite icon;
    public AnimationClip attackClip;
    public float startDelay = 1; 

    [Header("Trigger Attack")]
    public float attackDist = 1; 
    public float marginDist = 0.5f;
    
}
