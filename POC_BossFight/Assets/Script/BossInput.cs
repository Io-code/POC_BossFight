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
    public bool selfOriented;
    public float speed;
    public float dist;
    Vector3 dir,startPos;

    public bool relativDist;
    bool triggerRDist;

    public bool relativPos;
    bool triggerRPos;



    private void Start()
    {
        UpdateTarget();
    }
    private void Update()
    {

        SetOrientation();
        Move();
        ApplyUpdateTarget();
    }
    #region Self displacement
    public Vector3 MoveVec()
    {
        Vector3 targetVec = ((T_OffsetPos() - transform.position).normalized) * ((T_OffsetPos() - transform.position).magnitude - dist);
        Vector3 relativVec = (Vector3.Distance(transform.position, startPos + T_OffsetDir().normalized * dist)) * ((startPos + T_OffsetDir().normalized * dist) - transform.position).normalized;

        return (relativDist) ? relativVec : targetVec;
    }
    public void Move()
    {
        Vector3 velocity = MoveVec().normalized * Mathf.Clamp(MoveVec().magnitude *speed, 0,speed) ;
        if(move)
            transform.position += velocity * Time.deltaTime;
    }
    public void SetOrientation()
    {
        Vector3 newDir =(selfOriented)? MoveVec().normalized : (T_OffsetPos() - transform.position);

        if (newDir != Vector3.zero)
            dir = newDir.normalized;

        transform.right = dir.normalized;
    }
    #endregion
    #region target
    [ContextMenu("UpdateTarget")]
    public void UpdateTarget()
    {
        if (relativPos)
        {
            if(MoveVec().magnitude < 0.1f)
            {
                startPos = transform.position;
                targetPos = transform.position + transform.right * 0.1f;
                targetDir = targetPos - startPos;
                targetDir.y = 0;
            }
        }
        else
        {
            if (Vector3.Distance(target.position, targetPos) > 0.5f)
            {
                startPos = transform.position;
                targetPos = target.position;
                targetDir = targetPos - startPos;
                targetDir.y = 0;
            }
        }
    }
    public void ApplyUpdateTarget()
    {
        if (updateTarget)
            UpdateTarget();
        if (triggerRDist != relativDist || triggerRPos != relativPos)
        {
            triggerRDist = relativDist;
            triggerRPos = relativPos;

            UpdateTarget();
        }
    }
    public Vector3 T_OffsetPos()
    {
        float dirRad = Mathf.Atan2(targetDir.z, targetDir.x);
        if (relativPos)
            dirRad = 0;

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
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, 0.2f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetPos, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(T_OffsetPos(), 0.2f);
        Gizmos.DrawRay(transform.position, T_OffsetDir().normalized * MoveVec().magnitude);
        Gizmos.DrawWireSphere((relativDist)?startPos:T_OffsetPos(), Mathf.Abs( dist));
    }

}
