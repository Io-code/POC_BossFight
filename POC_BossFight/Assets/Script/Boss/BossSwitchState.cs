using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwitchState : StateMachineBehaviour
{
	public delegate void SwitchBossPattern(Animator animator);
	public static event SwitchBossPattern OnEndPattern,OnStartMove;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName("MoveIn"))
			OnStartMove(animator);

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//Debug.Log("Tag " + stateInfo.IsName("Attack") + " layer " + layerIndex);

		if (stateInfo.IsName("Free"))
			OnEndPattern(animator);
	}
}
