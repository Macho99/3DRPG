using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalIdleState : StateMachineBehaviour
{
    private Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<DeathKnight>().target;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<DeathKnight>().target;

        if (target != null)
        {
            animator.SetTrigger("isChasing");
            animator.SetTrigger("Skill1");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
