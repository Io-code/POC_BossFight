using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossInput bInput;
    Vector3 velocity;
    float targetDist;

    [Header("Animator")]
    public Animator animator;
    public AnimatorOverrideController overrider;

    [Header("Pattern")]
    public List<BossPattern> patterns;
    public float patternDelay = 1;
    float patternTimer;
    int pattern;

    [Header("Debug")]
    public List<AnimatorOverrideController> overriders; // debug
    int currentOverride = 0; // debug

    private void Update()
    {
        #region Select pattern
        if (pattern == 0)
            patternTimer -= Time.deltaTime;

        if(patternTimer <= 0)
        {
            pattern = (Random.value > 0.5f) ? 1 : 2;
            patternTimer = patternDelay;
        }
        #endregion

        Move();
        //SetAnimParam();
    }
    public void Move()
    {
        
        float offsetRad = Mathf.Deg2Rad * bInput.angleOffset;
        float dirRad = Mathf.Atan2(bInput.targetDir.z, bInput.targetDir.x);

        Vector3 offsetDir = new Vector3(Mathf.Cos(dirRad + offsetRad), 0, Mathf.Sin(dirRad + offsetRad));
        Vector3 moveDir = new Vector3(bInput.targetDir.x, 0, bInput.targetDir.z);
        velocity = offsetDir * bInput.speed;

        transform.position += velocity * Time.deltaTime;
        targetDist = (transform.position - bInput.targetPos).magnitude - 1.2f;

        transform.right = (velocity != Vector3.zero)? velocity.normalized : bInput.targetDir;
    }

    #region Animator
    public void SetAnimParam()
    {
        
        animator.SetFloat("Blend",Mathf.Clamp01(velocity.magnitude * 0.6f));
        if(targetDist > 1)
            animator.SetFloat("Blend", 1);
        if(targetDist < 0.5)
            animator.SetFloat("Blend", 0);

        // apply pattern
        if (pattern == 1 && targetDist <= 0.7)
        {
            animator.SetTrigger("TrigAtt");
            pattern = 0;
        }

        if (pattern == 2 && targetDist <= 3.5)
        {
            animator.SetTrigger("TrigDash");
            pattern = 0;
        }
    }

    public void SetAnimOverride(AnimatorOverrideController overrider)
    {
        animator.runtimeAnimatorController = overrider;
    }
    public void SetAnimationClip(AnimationClip clip, AnimatorOverrideController overrider)
    {
        overrider.animationClips[1] = clip;
    }

    public void DebugOverride()
    {
        SetAnimOverride(overriders[currentOverride]);
        currentOverride = (currentOverride + 1) % overriders.Count;
    }

    public void DebugPlayAnim()
    {
        animator.SetTrigger("TrigP");
    }
    #endregion

}
