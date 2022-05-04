using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInput : MonoBehaviour
{
    public Transform target;
    public Vector3 targetPos { get; private set; }
    public Vector3 targetDir { get; private set; }

    public bool updateTarget;
    public float speed;
    [Range(-180,180)]
    public float angleOffset;

    private void Update()
    {
        if (updateTarget)
        {
            targetPos = target.position;
            targetDir = (targetPos - transform.position).normalized * Mathf.Clamp((targetPos - transform.position).magnitude,0,1);
            targetDir = new Vector3(targetDir.x,0,targetDir.z);
        }
    }
}
