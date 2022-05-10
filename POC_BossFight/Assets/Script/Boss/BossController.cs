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
    public List<B_Pattern> patterns;
    int pIndex = 0;
    float patternTimer = 0;


    [Header("Debug")]
    public List<AnimatorOverrideController> overriders; // debug
    int currentOverride = 0; // debug

    #region Suscribe Event
    private void OnEnable()
    {
        BossSwitchState.OnEndPattern += SelectPattern;
    }

    private void OnDisable()
    {
        BossSwitchState.OnEndPattern += SelectPattern;
    }
    #endregion

    private void Start()
    {
        pIndex = Random.Range(0, patterns.Count);
    }
    private void Update()
    {

        Move();
        SetAnimParam();
    }
    public void SelectPattern( Animator animator)
    {
        if(animator == this.animator)
        {
            
        }
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

        if (bInput.updateDir) 
            transform.right = (velocity != Vector3.zero)? velocity.normalized : bInput.targetDir;
    }

    #region Animator
    public void SetAnimParam()
    {
        animator.SetFloat("Move", Mathf.Clamp01(velocity.magnitude*0.8f));

    }

    public void SetAnimationClip(AnimatorOverrideController overrider, int clipIndex, AnimationClip clip )
    {
        overrider.animationClips[clipIndex] = clip;
    }

    #endregion

    [System.Serializable]
    public struct B_Pattern
    {
        public BossPattern pattern;
        public List<NextPattern> nextPatterns;
    }

    [System.Serializable]
    public struct NextPattern
    {
        public BossPattern pattern;
        public float weight;
    }

}
