using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInput : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public bool updateTarget;
    [Range(-180,180)]
    public float angleOffset = 0;
    //public Vector3 targetOffset;
    public float offsetDist;

    Vector3 targetDir, targetPos;

    [Header("RelativeParameter")]
    public bool relativ;
    public bool move;
    public float speed;
    public float dist;
    Vector3 movedir,startPos;



    

    private void Start()
    {
        UpdateTarget();
    }
    private void Update()
    {

        SetOrientation();
        Move();

        if (updateTarget)
            UpdateTarget();
    }

    public void Move()
    {
        Vector3 targetVec = TargetPos() - transform.position;
        targetVec.y = 0;
        targetVec = (targetVec.magnitude - dist) * targetVec.normalized;
        
        Vector3 velocity = targetVec.normalized * Mathf.Clamp(targetVec.magnitude *speed, 0,speed) ;

        if(move)
            transform.position += velocity * Time.deltaTime;
    }
    public void SetOrientation()
    {
        Vector3 newMoveDir = TargetPos() - transform.position;

        if (newMoveDir != Vector3.zero)
            movedir = newMoveDir.normalized;

        transform.right = movedir.normalized;
    }

    #region target

    public void UpdateTarget()
    {
        startPos = transform.position;
        targetPos = target.position;
        targetDir = targetPos - startPos;
        targetDir.y = 0;
    }
    
    public Vector3 TargetPos()
    {
        float dirRad = Mathf.Atan2(targetDir.z, targetDir.x);
        Vector3 offset = new Vector3(Mathf.Cos(dirRad + angleOffset * Mathf.Deg2Rad), 0, Mathf.Sin(dirRad + angleOffset * Mathf.Deg2Rad));

        Vector3 pos = ((relativ)?transform.position:targetPos) + offset.normalized * offsetDist;
        pos.y = 0;
        return pos;
    }

    public Vector3 TargetDir()
    {
        Vector3 dir =  TargetPos() - startPos;
        dir.y = 0;
        return dir;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetPos, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(TargetPos(), 0.2f);
        Gizmos.DrawRay(transform.position, TargetDir());
        Gizmos.DrawWireSphere(TargetPos(), Mathf.Abs( dist));
    }

}
