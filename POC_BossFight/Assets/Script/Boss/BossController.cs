using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { ATTACK, MOVE, FREE }
    public BossState bossState;

    public BossInput bInput;
    Vector3 velocity;
    float targetDist;

    [Header("Animator")]
    public Animator animator;
    public AnimatorOverrideController overrider;
    public AnimationClip refAttackClip;
    [Header("Pattern")]
    public List<B_Pattern> bPatterns;
    public int pIndex = 0;
    public AnimationCurve randP;
    float patternTimer = 0;


    #region Suscribe Event
    private void OnEnable()
    {
        BossSwitchState.OnEndPattern += SelectPattern;
        BossSwitchState.OnStartMove += SwitchStateMove;
    }

    private void OnDisable()
    {
        BossSwitchState.OnEndPattern -= SelectPattern;
        BossSwitchState.OnStartMove -= SwitchStateMove;
    }
    #endregion

    private void Start()
    {
        //pIndex = Random.Range(0, bPatterns.Count);
        animator.runtimeAnimatorController = overrider;
        
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
            bossState = BossState.ATTACK;
            randP = new AnimationCurve();
            float pWeightSumm = 0;
            for(int i = 0; i< bPatterns[pIndex].nextPatterns.Count; i++)
            {
                pWeightSumm += bPatterns[pIndex].nextPatterns[i].weight;
                randP.AddKey(pWeightSumm, i);
            }

            float randChoose = Random.Range(0, pWeightSumm);
            pIndex = (int)randP.Evaluate(randChoose);
            patternTimer = bPatterns[pIndex].pattern.startDelay;

            // replace animation clip
            //List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrider.overridesCount);
           // overrider.GetOverrides(overrides);
            //Debug.Log("Pattern nb " + overrider.overridesCount);
            //overrides[2] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[2].Key, bPatterns[pIndex].pattern.attackClip);
            //overrides[1] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[1].Key, bPatterns[pIndex].pattern.moveInClip);

            //overrider.ApplyOverrides(overrides);


            Debug.Log("Select pattern " + pIndex + " at " + randChoose + " range " + pWeightSumm);
            
        }
    }
    public void SwitchPattern()
    {
        // replace animation clip
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrider.overridesCount);
        overrider.GetOverrides(overrides);
        Debug.Log("Pattern nb " + overrides.Count);
        for (int i = 0; i < overrides.Count; i++)
        {
            Debug.Log("Override "+i+":" + overrides[i]);
            if(overrides[i].Key == refAttackClip )
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, bPatterns[pIndex].pattern.attackClip);
        }
        //overrides[2] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[2].Key, bPatterns[pIndex].pattern.attackClip);
        //overrides[1] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[1].Key, bPatterns[pIndex].pattern.moveInClip);

        overrider.ApplyOverrides(overrides);
    }
    public void SwitchStateMove(Animator animator)
    {
        if(animator == this.animator)
            bossState = BossState.MOVE;       
    }
    public void Move()
    {
        
        float offsetRad = Mathf.Deg2Rad * bInput.angleOffset;
        float dirRad = Mathf.Atan2(bInput.targetDir.z, bInput.targetDir.x);

        Vector3 offsetDir = new Vector3(Mathf.Cos(dirRad + offsetRad), 0, Mathf.Sin(dirRad + offsetRad));
        float dirSign = Mathf.Clamp( Vector3.Distance(transform.position, bInput.targetPos) - bPatterns[pIndex].pattern.attackDist,-1,1);
        //Vector3 moveDir = new Vector3(bInput.targetDir.x, 0, bInput.targetDir.z);
        velocity = offsetDir * bInput.speed * ((bossState == BossState.MOVE)?dirSign : 1 );

        transform.position += velocity * Time.deltaTime;
        targetDist = (transform.position - bInput.targetPos).magnitude - 1.2f;

        if (!bInput.lockDir) 
            transform.right = (velocity != Vector3.zero)? velocity.normalized : bInput.targetDir;
    }
    public void SetAnimParam()
    {
        animator.SetFloat("Move", Mathf.Clamp01(velocity.magnitude*0.8f));

        // start move in
        patternTimer -= Time.deltaTime;
        if (patternTimer <= 0)
            animator.SetTrigger("Pattern");

        // start attack
        //if (Mathf.Abs(Vector3.Distance(transform.position, bInput.targetPos) - bPatterns[pIndex].pattern.attackDist) < bPatterns[pIndex].pattern.marginDist * 0.5f)
            //animator.SetTrigger("Attack");
    }

    #region BossPattern
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
        public float weight ;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bInput.targetPos, bPatterns[pIndex].pattern.attackDist);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(bInput.targetPos + (bPatterns[pIndex].pattern.attackDist - bPatterns[pIndex].pattern.marginDist) * -bInput.targetDir, bInput.targetPos + (bPatterns[pIndex].pattern.attackDist + bPatterns[pIndex].pattern.marginDist) * -bInput.targetDir);
    }
}
