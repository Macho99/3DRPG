using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ApplyRootMotion : StateMachineBehaviour
{
    Transform target;
    Transform myTf;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        target = animator.GetComponent<DeathKnight>().target;
        myTf = animator.GetComponent<Transform>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            return;
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackDelay", true);
    }
}
