using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : LifeBehaviour
{
    public CharacterController target;

    public Animator animator;
    public AnimatorOverrideController animOveride;

    public List<BossPattern> patterns;
    public BossPattern currentPattern;

    public List<HitZone> hitZones;
    public void Displacement( float angle, float distance)
    {
        
    }
    public void UpdateTargetDir( Transform target)
    {

    }

    void LaunchPattern(List<BossPattern> patterns)
    {
        UpdateHitZoneParam();
    }

    void UpdateAnimator(Animator anim, AnimatorOverrideController ovveride, BossPattern currentPattern)
    {

    }

    private void UpdateHitZoneParam()
    {
        
    }
}
