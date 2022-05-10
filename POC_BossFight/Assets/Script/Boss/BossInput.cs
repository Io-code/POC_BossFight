using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInput : MonoBehaviour
{
    public Transform target;
    public Vector3 targetPos { get; private set; }
    public Vector3 targetDir { get; private set; }

    [HideInInspector] public bool updateTarget, updateDir;
    [HideInInspector] public float speed;
    [HideInInspector] public float angleOffset; // [Range(-180,180)]

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
