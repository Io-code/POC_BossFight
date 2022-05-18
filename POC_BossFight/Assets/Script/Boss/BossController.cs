using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { ATTACK, FREE }
    public BossState bossState;

    public BossInput bInput;
    Vector3 velocity;
    float targetDist;

    [Header("Animator")]
    public Animator animator;
    public AnimatorOverrideController overrider;
    public AnimationClip refAttackClip;
    float move;
    [Header("Pattern")]
    public List<BossPattern> bPatterns;
    public int pIndex = 0;
    public AnimationCurve randP;
    float patternTimer = 1;


    #region Suscribe Event
    private void OnEnable()
    {
        BossSwitchState.OnStartPattern += SelectPattern;
        BossSwitchState.OnEndAttack += EndPattern;
    }

    private void OnDisable()
    {
        BossSwitchState.OnStartPattern -= SelectPattern;
        BossSwitchState.OnEndAttack -= EndPattern;
    }
    #endregion

    private void Start()
    {
        //pIndex = Random.Range(0, bPatterns.Count);
        animator.runtimeAnimatorController = overrider;

        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrider.overridesCount);
        overrider.GetOverrides(overrides);
        //Debug.Log("Pattern nb " + overrides.Count);
        for (int i = 0; i < overrides.Count; i++)
        {
            // Debug.Log("Override "+i+":" + overrides[i]);
            if (overrides[i].Key == refAttackClip)
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, null);
        }

        overrider.ApplyOverrides(overrides);

    }
    private void Update()
    {
        Move();
        SetAnimParam();
    }
    #region Pattern
    public void SelectPattern( Animator animator)
    {
        
        if(animator == this.animator )
        {
            if(pIndex >= bPatterns.Count)
            {
                pIndex =(int)Mathf.Clamp(bPatterns.Count - 1,0, (bPatterns.Count));
                patternTimer = 1;
                return;
            }

            bossState = BossState.ATTACK;
            randP = new AnimationCurve();
            randP.AddKey(0, 0);

            float pWeightSumm = 0;
            for(int i = 0; i< bPatterns[pIndex].nextPatterns.Count; i++)
            {
                pWeightSumm += bPatterns[pIndex].nextPatterns[i].weight;
                randP.AddKey(pWeightSumm, i);
            }

            float randChoose = Random.Range(0, pWeightSumm);
            pIndex = (int)Mathf.Ceil(randP.Evaluate(randChoose));
            if(pIndex < bPatterns.Count)
            {
                patternTimer = bPatterns[pIndex].pattern.startDelay;
                //Debug.Log("Select pattern " + pIndex + " at " + randChoose + " range " + pWeightSumm);
                SwitchAttackP(pIndex);
            }
            
            
        }
    }
    public void SwitchAttackP( int index)
    {
        // replace animation clip
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrider.overridesCount);
        overrider.GetOverrides(overrides);
        //Debug.Log("Pattern nb " + overrides.Count);
        for (int i = 0; i < overrides.Count; i++)
        {
           // Debug.Log("Override "+i+":" + overrides[i]);
            if(overrides[i].Key == refAttackClip )
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, bPatterns[index].pattern.attackClip);
        }
        //overrides[2] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[2].Key, bPatterns[pIndex].pattern.attackClip);
        //overrides[1] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[1].Key, bPatterns[pIndex].pattern.moveInClip);

        overrider.ApplyOverrides(overrides);
    }
    public void EndPattern(Animator animator)
    {
        bossState = BossState.FREE;
    }
    [ContextMenu("Update NextPattern")]
    public void UpdateNextPattern()
    {
       
        for (int i = 0; i < bPatterns.Count;i++)
        {
            List<NextPattern> newNextP = new List<NextPattern>();
            //Debug.Log("Next U p " + newNextP.Count +" " +bPatterns.Count);
            for (int j = 0; j < bPatterns.Count; j++)
            {
                NextPattern nextP;
                /*if (j < bPatterns[i].nextPatterns.Count)
                    nextP = bPatterns[i].nextPatterns[j];
                else*/
                    nextP = new NextPattern(bPatterns[j].pattern, 1);

                Debug.Log("Next p " + nextP);
                newNextP.Add(nextP);
            }

            bPatterns[i].nextPatterns.Clear();
            foreach (NextPattern np in newNextP)
            {
                bPatterns[i].nextPatterns.Add(np);
            }

                /*
                for (int j = 0; j < bPatterns.Count; j++)
                {

                    if (j < bPatterns[i].nextPatterns.Count)
                        bPatterns[i].nextPatterns[j].pattern = bPatterns[j].pattern;
                    else
                        bPatterns[i].nextPatterns.Add(new NextPattern(bPatterns[j].pattern, 1));
                }*/
        }
    }
    #endregion

    public void Move()
    {
        
        float offsetRad = Mathf.Deg2Rad * bInput.angleOffset;
        float dirRad = Mathf.Atan2(bInput.targetDir.z, bInput.targetDir.x);

        Vector3 offsetDir = new Vector3(Mathf.Cos(dirRad + offsetRad), 0, Mathf.Sin(dirRad + offsetRad));
        //Vector3 moveDir = new Vector3(bInput.targetDir.x, 0, bInput.targetDir.z);
        velocity = offsetDir * bInput.speed * ((bInput.lockDir)? 1 : Mathf.Sign( InAttackRange()));

        transform.position += velocity * Time.deltaTime;
        targetDist = (transform.position - bInput.targetPos).magnitude - 1.2f;

        if (!bInput.lockDir) 
            transform.right = (velocity.magnitude > 10f)? velocity.normalized : bInput.targetDir;
    }
    public void SetAnimParam()
    {
        //Mathf.Clamp01(Mathf.Abs(InAttackRange()) - 0.1)
        //if(bossState == BossState.FREE)
        move = Mathf.Clamp01((Mathf.Abs(InAttackRange()) < 0.1) ? move -= Time.deltaTime : move += Time.deltaTime);
            animator.SetFloat("Move",move);

        // start attack
        if (pIndex < bPatterns.Count && Mathf.Abs(Vector3.Distance(transform.position, bInput.targetPos) - bPatterns[pIndex].pattern.attackDist) < bPatterns[pIndex].pattern.marginDist * 0.5f)
            bossState = BossState.ATTACK;

        if(bossState == BossState.ATTACK)
        {
            patternTimer -= Time.deltaTime;
            animator.SetFloat("Move", 0);
        }
            

        if (patternTimer <= 0)
        {
            patternTimer = 10; // debug height value
            animator.SetTrigger("Attack");
        }
            
    }

    public float InAttackRange()
    {
        if (pIndex < bPatterns.Count)
        {
            float dirSign = Mathf.Clamp(Vector3.Distance(transform.position, bInput.targetPos) - bPatterns[pIndex].pattern.attackDist, -1, 1);
            return ((bInput.targetPos - transform.position).magnitude / bPatterns[pIndex].pattern.attackDist) * dirSign;
        }
        else
            return 0;
      
    }


    private void OnDrawGizmos()
    {
        if(pIndex < bPatterns.Count)
        {
            Gizmos.DrawWireSphere(bInput.targetPos, bPatterns[pIndex].pattern.attackDist);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bInput.targetPos + (bPatterns[pIndex].pattern.attackDist - bPatterns[pIndex].pattern.marginDist) * -bInput.targetDir, bInput.targetPos + (bPatterns[pIndex].pattern.attackDist + bPatterns[pIndex].pattern.marginDist) * -bInput.targetDir);
        }
    }
        
}
