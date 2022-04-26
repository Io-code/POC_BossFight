using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LifeBehaviour : MonoBehaviour
{
    public bool invincible;
    public int maxLifePoint;
    public int lifePoint;
    private void Awake()
    {
        lifePoint = maxLifePoint;
    }

    public void Hurt( int damage, Vector3 attackStrength, float stunDuration)
    {
        lifePoint -= damage;
    }
}
