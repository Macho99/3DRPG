using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckDist : StateMachineBehaviour
{
    Transform target;
    Transform myTf;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<DeathKnight>().target;
        myTf = animator.GetComponent<Transform>();
        agent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        if (Vector3.Distance(myTf.position, target.position) > agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
