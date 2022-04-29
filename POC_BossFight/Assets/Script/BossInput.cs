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
    public bool move;
    public float speed;
    public float dist;
    Vector3 movedir,startPos;
    public bool relativDist;





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
        Vector3 targetVec = ((T_OffsetPos() - transform.position).normalized) * ((T_OffsetPos() - transform.position).magnitude - dist);
        Vector3 relativVec = (Vector3.Distance(transform.position, startPos + T_OffsetDir().normalized * dist)) * ((startPos + T_OffsetDir().normalized * dist) - transform.position).normalized;

        Vector3 moveVec = (relativDist) ? relativVec : targetVec;

        Vector3 velocity = moveVec.normalized * Mathf.Clamp(moveVec.magnitude *speed, 0,speed) ;

        if(move)
            transform.position += velocity * Time.deltaTime;
    }
    public void SetOrientation()
    {
        Vector3 newMoveDir = T_OffsetPos() - transform.position;

        if (newMoveDir != Vector3.zero)
            movedir = newMoveDir.normalized;

        transform.right = movedir.normalized;
    }

    #region target
    [ContextMenu("UpdateTarget")]
    public void UpdateTarget()
    {
 

            if (Vector3.Distance(target.position, targetPos) > 0.5f)
            {
                startPos = transform.position;
                targetPos = target.position;
                targetDir = targetPos - startPos;
                targetDir.y = 0;
            }
        
    }
    
    public Vector3 T_OffsetPos()
    {
        float dirRad = Mathf.Atan2(targetDir.z, targetDir.x);
        Vector3 offset = new Vector3(Mathf.Cos(dirRad + angleOffset * Mathf.Deg2Rad), 0, Mathf.Sin(dirRad + angleOffset * Mathf.Deg2Rad));

        Vector3 pos = targetPos+ offset.normalized * offsetDist;
        pos.y = 0;
        return pos;
    }
    public Vector3 T_OffsetDir()
    {
        Vector3 dir =  T_OffsetPos() - startPos;
        dir.y = 0;
        return dir;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetPos, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(T_OffsetPos(), 0.2f);
        Gizmos.DrawRay(transform.position, T_OffsetDir());
        Gizmos.DrawWireSphere((relativDist)?startPos:T_OffsetPos(), Mathf.Abs( dist));
    }

}
