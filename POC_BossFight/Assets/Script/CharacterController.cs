using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : LifeBehaviour
{
    public Animator animator;
    
    public float speed;
    public float dodgeDist, dodgeDuration;
    public enum PlayerState { FREE,DODGE,ATTACK,STUN}
    public PlayerState playerState;

    public HitZone hitZone;
    public int attackDamage;
    public void Displacement( Vector3 dir, float speed)
    {

    }

    public void Attack()
    {

    }

    public void Dodge(float distance, float duration)
    {

    }
    void UpdatePlayerState()
    {

    }
    void UpdateAnimator()
    {

    }


    
}
